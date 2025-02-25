using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using MagicOnion.Generator.Internal;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.FileSystemGlobbing;

namespace MagicOnion.Generator.Utils;

internal static class PseudoCompilation
{
    public static Task<CSharpCompilation> CreateFromProjectAsync(string[] csprojs, string[] preprocessorSymbols, IMagicOnionGeneratorLogger logger, CancellationToken cancellationToken)
    {
        var parseOption = new CSharpParseOptions(LanguageVersion.Latest, DocumentationMode.Parse, SourceCodeKind.Regular, CleanPreprocessorSymbols(preprocessorSymbols));
        var syntaxTrees = new List<SyntaxTree>();

        logger.Trace($"[{nameof(PseudoCompilation)}] Creating compilation from project(s). (PreprocessorSymbolNames={string.Join(";", parseOption.PreprocessorSymbolNames)})");

        var sources = new HashSet<string>();
        var locations = new List<string>();
        var globalUsings = new HashSet<(string Namespace, bool Static)>();
        foreach (var csproj in csprojs)
        {
            CollectDocument(csproj, sources, locations, globalUsings, logger);
        }

        foreach (var file in sources.Select(Path.GetFullPath).Distinct())
        {
            var path = NormalizeDirectorySeparators(file);
            logger.Trace($"[{nameof(PseudoCompilation)}] Source '{path}'");
            var text = File.ReadAllText(NormalizeDirectorySeparators(file), Encoding.UTF8);
            var syntax = CSharpSyntaxTree.ParseText(text, parseOption, path);
            syntaxTrees.Add(syntax);
        }
        syntaxTrees.Add(CSharpSyntaxTree.ParseText(string.Join(Environment.NewLine, globalUsings.Select(x => $"global using {(x.Static ? "static" : "")} {x.Namespace};"))));

        // ignore Unity's default metadatas(to avoid conflict .NET Core runtime import)
        // MonoBleedingEdge = .NET 4.x Unity metadata
        // 2.0.0 = .NET Standard 2.0 Unity metadata
        var metadata = new Dictionary<string,PortableExecutableReference>(StringComparer.OrdinalIgnoreCase);
        var targetMetadataLocations = locations.Select(Path.GetFullPath)
            .Concat(GetStandardReferences())
            .Distinct()
            .Where(x => !(x.Contains("MonoBleedingEdge") || x.Contains("2.1.0")))
            // .NET (Core) SDK libraries are prioritized.
            .OrderBy(x => x.Contains(".NETCore") ? 1 : 0)
            // We expect the NuGet package path to contain the version. :)
            .ThenByDescending(x => x);

        // DEBUG
        foreach (string location in targetMetadataLocations)
        {
            logger.Trace($"{location}");
        }

        var netCoreBundledAssemblies = new[]
        {
            "Microsoft.Bcl.AsyncInterfaces.dll",
            "System.Buffers.dll",
            "System.Memory.dll",
            "System.Runtime.CompilerServices.Unsafe.dll",
            "System.Threading.Tasks.Extensions.dll",
        };

        var hasNetCoreLibrariesLoaded = false;
        foreach (var item in targetMetadataLocations)
        {
            if (File.Exists(item))
            {
                var assemblyFileName = Path.GetFileName(item);
                if (metadata.ContainsKey(assemblyFileName))
                {
                    logger.Trace($"[{nameof(PseudoCompilation)}] Loading Metadata '{item}' (Skipped. Different version of the library is already loaded.)");
                }
                else if (hasNetCoreLibrariesLoaded && netCoreBundledAssemblies.Contains(assemblyFileName, StringComparer.OrdinalIgnoreCase))
                {
                    logger.Trace($"[{nameof(PseudoCompilation)}] Loading Metadata '{item}' (Skipped. Use .NET Runtime assembly instead.)");
                }
                else
                {
                    logger.Trace($"[{nameof(PseudoCompilation)}] Loading Metadata '{item}'.");
                    metadata.Add(assemblyFileName, MetadataReference.CreateFromFile(item));

                    if (item.Contains("Microsoft.NETCore.App"))
                    {
                        hasNetCoreLibrariesLoaded = true;
                    }
                }
            }
            else
            {
                logger.Trace($"[{nameof(PseudoCompilation)}] Loading Metadata '{item}'. But it could not be found.");
            }
        }


        var compilation = CSharpCompilation.Create(
            "CodeGenTemp",
            syntaxTrees,
            metadata.Values,
            new CSharpCompilationOptions(
                OutputKind.DynamicallyLinkedLibrary,
                allowUnsafe: true,
                metadataImportOptions: MetadataImportOptions.All));

        var diagnostics = compilation.GetDiagnostics(cancellationToken);
        if (diagnostics.Any())
        {
            foreach (var diagnostic in diagnostics.Where(x => x.Severity == DiagnosticSeverity.Error))
            {
                logger.Trace($"[{nameof(PseudoCompilation)}] CompilationDiagnostic: {diagnostic.ToString()}");
            }
        }

        return Task.FromResult(compilation);
    }

    public static async Task<CSharpCompilation> CreateFromDirectoryAsync(string directoryRoot, string[] preprocessorSymbols, string dummyAnnotation, CancellationToken cancellationToken)
    {
        var parseOption = new CSharpParseOptions(LanguageVersion.Latest, DocumentationMode.Parse, SourceCodeKind.Regular, CleanPreprocessorSymbols(preprocessorSymbols));

        var syntaxTrees = new List<SyntaxTree>();
        foreach (var file in IterateCsFileWithoutBinObj(directoryRoot))
        {
            var text = File.ReadAllText(NormalizeDirectorySeparators(file), Encoding.UTF8);
            var syntax = CSharpSyntaxTree.ParseText(text, parseOption);
            syntaxTrees.Add(syntax);
            if (Path.GetFileNameWithoutExtension(file) == "Attributes")
            {
                var root = await syntax.GetRootAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        syntaxTrees.Add(CSharpSyntaxTree.ParseText(dummyAnnotation, parseOption));

        var metadata = GetStandardReferences().Select(x => MetadataReference.CreateFromFile(x)).ToArray();

        var compilation = CSharpCompilation.Create(
            "CodeGenTemp",
            syntaxTrees,
            metadata,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));

        return compilation;
    }

    private static List<string> GetStandardReferences()
    {
        var standardMetadataType = new[]
        {
            typeof(object),
            typeof(Attribute),
            typeof(Enumerable),
            typeof(Task<>),
            typeof(IgnoreDataMemberAttribute),
            typeof(System.Collections.Hashtable),
            typeof(System.Collections.Generic.List<>),
            typeof(System.Collections.Generic.HashSet<>),
            typeof(System.Collections.Immutable.IImmutableList<>),
            typeof(System.Linq.ILookup<,>),
            typeof(System.Tuple<>),
            typeof(System.ValueTuple<>),
            typeof(System.Collections.Concurrent.ConcurrentDictionary<,>),
            typeof(System.Collections.ObjectModel.ObservableCollection<>),
            typeof(System.Net.Http.HttpClient),
        };

        var metadata = standardMetadataType
            .Select(x => x.Assembly.Location)
            .Distinct()
            .ToList();

        var dir = new FileInfo(typeof(object).Assembly.Location).Directory;
        {
            var path = Path.Combine(dir.FullName, "netstandard.dll");
            if (File.Exists(path))
            {
                metadata.Add(path);
            }
        }

        {
            var path = Path.Combine(dir.FullName, "System.Runtime.dll");
            if (File.Exists(path))
            {
                metadata.Add(path);
            }
        }

        return metadata;
    }

    private static IEnumerable<string> CleanPreprocessorSymbols(string[] preprocessorSymbols)
    {
        if (preprocessorSymbols == null)
        {
            return null;
        }

        return preprocessorSymbols.Where(x => !string.IsNullOrWhiteSpace(x));
    }

    private static void CollectDocument(string csproj, HashSet<string> source, List<string> metadataLocations, HashSet<(string Namespace, bool Static)> globalUsings, IMagicOnionGeneratorLogger logger)
    {
        logger.Trace($"[{nameof(PseudoCompilation)}] Open project '{csproj}'");

        XDocument document;
        using (var sr = new StreamReader(csproj, true))
        {
            var reader = new XmlTextReader(sr);
            reader.Namespaces = false;

            document = XDocument.Load(reader, LoadOptions.None);
        }

        var csProjRoot = Path.GetDirectoryName(csproj);
        // .NET Core root
        var framworkRoot = Path.GetDirectoryName(typeof(object).Assembly.Location);

        // Legacy
        // <Project ToolsVersion=...>
        // New
        // <Project Sdk="Microsoft.NET.Sdk">
        var proj = document.Element("Project");
        var legacyFormat = !(proj.Attribute("Sdk")?.Value?.StartsWith("Microsoft.NET.Sdk") ?? false);

        if (!legacyFormat)
        {
            // try to find EnableDefaultCompileItems
            if (document.Descendants("EnableDefaultCompileItems")?.FirstOrDefault()?.Value == "false"
                || document.Descendants("EnableDefaultItems")?.FirstOrDefault()?.Value == "false")
            {
                legacyFormat = true;
            }
        }

        {
            // compile files
            {
                // default include
                if (!legacyFormat)
                {
                    foreach (var path in GetCompileFullPaths(null, "**/*.cs", csProjRoot))
                    {
                        source.Add(path);
                    }
                }

                // custom elements
                foreach (var item in document.Descendants("Compile"))
                {
                    var include = item.Attribute("Include")?.Value;
                    if (include != null)
                    {
                        foreach (var path in GetCompileFullPaths(item, include, csProjRoot))
                        {
                            source.Add(path);
                        }
                    }

                    var remove = item.Attribute("Remove")?.Value;
                    if (remove != null)
                    {
                        foreach (var path in GetCompileFullPaths(item, remove, csProjRoot))
                        {
                            source.Remove(path);
                        }
                    }
                }

                // default remove
                if (!legacyFormat)
                {
                    foreach (var path in GetCompileFullPaths(null, "./bin/**;./obj/**", csProjRoot))
                    {
                        source.Remove(path);
                    }
                }
            }

            // ImplicitUsings / Using
            if (document.Descendants("ImplicitUsings").FirstOrDefault()?.Value?.ToLowerInvariant() is "enable" or "true")
            {
                globalUsings.Add(("global::System", false));
                globalUsings.Add(("global::System.Collections.Generic", false));
                globalUsings.Add(("global::System.IO", false));
                globalUsings.Add(("global::System.Linq", false));
                globalUsings.Add(("global::System.Net.Http", false));
                globalUsings.Add(("global::System.Threading", false));
                globalUsings.Add(("global::System.Threading.Tasks", false));
            }
            foreach (var item in document.Descendants("Using"))
            {
                if (item.Attribute("Include")?.Value is { } includeUsing)
                {
                    globalUsings.Add((includeUsing, string.Equals(item.Attribute("Static")?.Value, "True", StringComparison.OrdinalIgnoreCase)));
                }
                else if (item.Attribute("Remove")?.Value is { } removeUsing)
                {
                    globalUsings.Remove((removeUsing, string.Equals(item.Attribute("Static")?.Value, "True", StringComparison.OrdinalIgnoreCase)));
                }
            }

            // shared
            foreach (var item in document.Descendants("Import"))
            {
                if (item.Attribute("Label")?.Value == "Shared")
                {
                    var sharedRoot = Path.GetDirectoryName(Path.Combine(csProjRoot, item.Attribute("Project").Value));
                    logger.Trace($"[{nameof(PseudoCompilation)}] Import project '{sharedRoot}'");
                    foreach (var file in IterateCsFileWithoutBinObj(sharedRoot))
                    {
                        source.Add(file);
                    }
                }
            }

            // proj-ref
            foreach (var item in document.Descendants("ProjectReference"))
            {
                var refCsProjPath = item.Attribute("Include")?.Value;
                if (refCsProjPath != null)
                {
                    logger.Trace($"[{nameof(PseudoCompilation)}] Resolve ProjectReference '{refCsProjPath}'");
                    CollectDocument(Path.Combine(csProjRoot, NormalizeDirectorySeparators(refCsProjPath)), source, metadataLocations, globalUsings, logger);
                }
            }

            // metadata
            foreach (var item in document.Descendants("Reference"))
            {
                var hintPath = item.Element("HintPath")?.Value;
                if (hintPath == null)
                {
                    var path = Path.Combine(framworkRoot, item.Attribute("Include").Value + ".dll");
                    logger.Trace($"[{nameof(PseudoCompilation)}] Assembly Reference '{path}'");
                    metadataLocations.Add(path);
                }
                else
                {
                    var path = Path.Combine(csProjRoot, NormalizeDirectorySeparators(hintPath));
                    logger.Trace($"[{nameof(PseudoCompilation)}] Assembly Reference '{path}' (from HintPath)");
                    metadataLocations.Add(path);
                }
            }

            // resolve NuGet reference
            var nugetPackagesPath = Environment.GetEnvironmentVariable("NUGET_PACKAGES");
            if (nugetPackagesPath == null)
            {
                // Try default
                // Windows: %userprofile%\.nuget\packages
                // Mac/Linux: ~/.nuget/packages
                nugetPackagesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".nuget", "packages");
            }

            var resolvedDllPaths = new HashSet<string>();
            void CollectNugetPackages(string id, string packageVersion, string originalTargetFramework)
            {
                logger.Trace($"[{nameof(PseudoCompilation)}] NuGet Package '{id} {packageVersion}'");
                var packageAssemblyResolved = false;
                foreach (var targetFramework in NetstandardFallBack(originalTargetFramework).Distinct())
                {
                    var pathpath = Path.Combine(nugetPackagesPath, id, packageVersion, "lib", targetFramework);
                    if (!Directory.Exists(pathpath))
                    {
                        pathpath = pathpath.ToLower(); // try all lower.
                    }

                    if (Directory.Exists(pathpath))
                    {
                        if (resolvedDllPaths.Add(pathpath))
                        {
                            logger.Trace($"[{nameof(PseudoCompilation)}] Resolved assembly '{pathpath}' (targetFramework={targetFramework})");
                            foreach (var dependency in ResolveNuGetDependency(nugetPackagesPath, id, packageVersion, targetFramework))
                            {
                                CollectNugetPackages(dependency.id, dependency.version, originalTargetFramework);
                            }
                        }

                        packageAssemblyResolved = true;
                        break;
                    }
                }

                if (!packageAssemblyResolved)
                {
                    logger.Trace($"[{nameof(PseudoCompilation)}] Could not find package assembly for '{id} {packageVersion}'");
                }
            }

            foreach (var item in document.Descendants("PackageReference"))
            {
                var originalTargetFramework = document.Descendants("TargetFramework").FirstOrDefault()?.Value ?? document.Descendants("TargetFrameworks").First().Value.Split(';').First();
                var includePath = item.Attribute("Include")?.Value.Trim().ToLower(); // maybe lower
                if (includePath == null) continue;

                var packageVersion = item.Attribute("Version").Value.Trim();

                CollectNugetPackages(includePath, packageVersion, originalTargetFramework);
            }

            foreach (var item in resolvedDllPaths)
            {
                foreach (var dllPath in Directory.GetFiles(item, "*.dll"))
                {
                    metadataLocations.Add(Path.GetFullPath(dllPath));
                }
            }
        }
    }

    private static IEnumerable<string> NetstandardFallBack(string originalTargetFramework)
    {
        yield return originalTargetFramework;
        //if (originalTargetFramework.Contains("netcoreapp"))
        //{
        //    yield return "netcoreapp3.1";
        //    yield return "netcoreapp3.0";
        //    yield return "netcoreapp2.1";
        //    yield return "netcoreapp2.0";
        //}

        yield return "netstandard2.1";
        yield return "netstandard2.0";
        //yield return "netstandard1.6";
    }

    private static IEnumerable<(string id, string version)> ResolveNuGetDependency(string nugetPackagesPath, string includePath, string packageVersion, string targetFramework)
    {
        var dirPath = Path.Combine(nugetPackagesPath, includePath, packageVersion);
        if (!Directory.Exists(dirPath))
        {
            dirPath = dirPath.ToLower(); // try all lower.
        }

        var filePath = Path.Combine(dirPath, includePath.ToLower() + ".nuspec");
        if (File.Exists(filePath))
        {
            XDocument document;
            using (var sr = new StreamReader(filePath, true))
            {
                var reader = new XmlTextReader(sr);
                reader.Namespaces = false;

                document = XDocument.Load(reader, LoadOptions.None);
            }

            foreach (var item in document.Descendants().Where(x => x.Name == "dependencies"))
            {
                foreach (var tf in NetstandardFallBack(targetFramework))
                {
                    foreach (var item2 in item.Elements().Where(x => x.Name == "group" && x.Attributes().Any(a => a.Name == "targetFramework" && (a.Value?.Trim('.')?.Equals(tf, StringComparison.OrdinalIgnoreCase) ?? false))))
                    {
                        foreach (var item3 in item2.Descendants().Where(x => x.Name == "dependency"))
                        {
                            yield return (item3.Attribute("id").Value, item3.Attribute("version").Value);
                        }

                        // found, stop search.
                        yield break;
                    }
                }
            }
        }
    }

    private static IEnumerable<string> IterateCsFileWithoutBinObj(string root)
    {
        foreach (var item in Directory.EnumerateFiles(root, "*.cs", SearchOption.TopDirectoryOnly))
        {
            yield return item;
        }

        foreach (var dir in Directory.GetDirectories(root, "*", SearchOption.TopDirectoryOnly))
        {
            var dirName = new DirectoryInfo(dir).Name;
            if (dirName == "bin" || dirName == "obj")
            {
                continue;
            }

            foreach (var item in IterateCsFileWithoutBinObj(dir))
            {
                yield return item;
            }
        }
    }

    private static string NormalizeDirectorySeparators(string path)
    {
        return path.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
    }

    private static IEnumerable<string> GetCompileFullPaths(XElement compile, string includeOrRemovePattern, string csProjRoot)
    {
        // solve macro
        includeOrRemovePattern = includeOrRemovePattern.Replace("$(ProjectDir)", csProjRoot);

        var matcher = new Matcher(StringComparison.OrdinalIgnoreCase);
        matcher.AddIncludePatterns(includeOrRemovePattern.Split(';'));
        var exclude = compile?.Attribute("Exclude")?.Value;
        if (exclude != null)
        {
            matcher.AddExcludePatterns(exclude.Split(';'));
        }

        foreach (var path in matcher.GetResultsInFullPath(csProjRoot))
        {
            yield return path;
        }
    }
}
