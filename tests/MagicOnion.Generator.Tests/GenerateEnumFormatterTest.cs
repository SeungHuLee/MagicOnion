using Xunit.Abstractions;

namespace MagicOnion.Generator.Tests;

public class GenerateEnumFormatterTest
{
    readonly ITestOutputHelper testOutputHelper;

    public GenerateEnumFormatterTest(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task GenerateEnumFormatter_Return()
    {
        using var tempWorkspace = TemporaryProjectWorkarea.Create();
        tempWorkspace.AddFileToProject("IMyService.cs", @"
using System;
using MessagePack;
using MagicOnion;

namespace TempProject
{
    public interface IMyService : IService<IMyService>
    {
        UnaryResult<MyEnum> GetEnumAsync();
    }

    public enum MyEnum
    {
        A, B, C
    }
}
            ");

        var compiler = new MagicOnionCompiler(new MagicOnionGeneratorTestOutputLogger(testOutputHelper), CancellationToken.None);
        await compiler.GenerateFileAsync(
            tempWorkspace.CsProjectPath,
            tempWorkspace.OutputDirectory,
            true,
            "TempProject.Generated",
            "",
            "MessagePack.Formatters",
            SerializerType.MessagePack
        );

        var compilation = tempWorkspace.GetOutputCompilation();
        compilation.GetCompilationErrors().Should().BeEmpty();
        var symbols = compilation.GetNamedTypeSymbolsFromGenerated();
        symbols.Should().Contain(x => x.Name.EndsWith("MyEnumFormatter"));
    }


    [Fact]
    public async Task GenerateEnumFormatter_Return_Nullable()
    {
        using var tempWorkspace = TemporaryProjectWorkarea.Create();
        tempWorkspace.AddFileToProject("IMyService.cs", @"
using System;
using MessagePack;
using MagicOnion;

namespace TempProject
{
    public interface IMyService : IService<IMyService>
    {
        UnaryResult<MyEnum?> GetEnumAsync();
    }

    public enum MyEnum
    {
        A, B, C
    }
}
            ");

        var compiler = new MagicOnionCompiler(new MagicOnionGeneratorTestOutputLogger(testOutputHelper), CancellationToken.None);
        await compiler.GenerateFileAsync(
            tempWorkspace.CsProjectPath,
            tempWorkspace.OutputDirectory,
            true,
            "TempProject.Generated",
            "",
            "MessagePack.Formatters",
            SerializerType.MessagePack
        );

        var compilation = tempWorkspace.GetOutputCompilation();
        compilation.GetCompilationErrors().Should().BeEmpty();
        var symbols = compilation.GetNamedTypeSymbolsFromGenerated();
        symbols.Should().Contain(x => x.Name.EndsWith("MyEnumFormatter"));
    }

    [Fact]
    public async Task GenerateEnumFormatter_Parameter()
    {
        using var tempWorkspace = TemporaryProjectWorkarea.Create();
        tempWorkspace.AddFileToProject("IMyService.cs", @"
using System;
using MessagePack;
using MagicOnion;

namespace TempProject
{
    public interface IMyService : IService<IMyService>
    {
        UnaryResult<Nil> GetEnumAsync(MyEnum a);
    }

    public enum MyEnum
    {
        A, B, C
    }
}
            ");

        var compiler = new MagicOnionCompiler(new MagicOnionGeneratorTestOutputLogger(testOutputHelper), CancellationToken.None);
        await compiler.GenerateFileAsync(
            tempWorkspace.CsProjectPath,
            tempWorkspace.OutputDirectory,
            true,
            "TempProject.Generated",
            "",
            "MessagePack.Formatters",
            SerializerType.MessagePack
        );

        var compilation = tempWorkspace.GetOutputCompilation();
        compilation.GetCompilationErrors().Should().BeEmpty();
        var symbols = compilation.GetNamedTypeSymbolsFromGenerated();
        symbols.Should().Contain(x => x.Name.EndsWith("MyEnumFormatter"));
    }

    [Fact]
    public async Task GenerateEnumFormatter_Parameter_Nullable()
    {
        using var tempWorkspace = TemporaryProjectWorkarea.Create();
        tempWorkspace.AddFileToProject("IMyService.cs", @"
using System;
using MessagePack;
using MagicOnion;

namespace TempProject
{
    public interface IMyService : IService<IMyService>
    {
        UnaryResult<Nil> GetEnumAsync(MyEnum? a);
    }

    public enum MyEnum
    {
        A, B, C
    }
}
            ");

        var compiler = new MagicOnionCompiler(new MagicOnionGeneratorTestOutputLogger(testOutputHelper), CancellationToken.None);
        await compiler.GenerateFileAsync(
            tempWorkspace.CsProjectPath,
            tempWorkspace.OutputDirectory,
            true,
            "TempProject.Generated",
            "",
            "MessagePack.Formatters",
            SerializerType.MessagePack
        );

        var compilation = tempWorkspace.GetOutputCompilation();
        compilation.GetCompilationErrors().Should().BeEmpty();
        var symbols = compilation.GetNamedTypeSymbolsFromGenerated();
        symbols.Should().Contain(x => x.Name.EndsWith("MyEnumFormatter"));
    }

    [Fact]
    public async Task GenerateEnumFormatter_Nested()
    {
        using var tempWorkspace = TemporaryProjectWorkarea.Create();
        tempWorkspace.AddFileToProject("IMyService.cs", @"
using System;
using MessagePack;
using MagicOnion;

namespace TempProject
{
    public interface IMyService : IService<IMyService>
    {
        UnaryResult<Nil> GetEnumAsync(MyClass.MyEnum? a);
    }

    public class MyClass
    {
        public enum MyEnum
        {
            A, B, C
        }
    }
}
            ");

        var compiler = new MagicOnionCompiler(new MagicOnionGeneratorTestOutputLogger(testOutputHelper), CancellationToken.None);
        await compiler.GenerateFileAsync(
            tempWorkspace.CsProjectPath,
            tempWorkspace.OutputDirectory,
            true,
            "TempProject.Generated",
            "",
            "MessagePack.Formatters",
            SerializerType.MessagePack
        );

        var compilation = tempWorkspace.GetOutputCompilation();
        compilation.GetCompilationErrors().Should().BeEmpty();
        var symbols = compilation.GetNamedTypeSymbolsFromGenerated();
        symbols.Should().Contain(x => x.Name.EndsWith("MyClass_MyEnumFormatter"));
    }
}
