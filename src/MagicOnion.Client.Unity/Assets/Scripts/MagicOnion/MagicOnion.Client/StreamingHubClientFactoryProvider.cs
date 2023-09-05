using System;
using System.Linq;
using Grpc.Core;
using MagicOnion.Serialization;

namespace MagicOnion.Client
{
    /// <summary>
    /// Provides to get a StreamingHubClient factory of the specified service type.
    /// </summary>
    public static class StreamingHubClientFactoryProvider
    {
        /// <summary>
        /// Gets or set the StreamingHubClient factory provider to use by default.
        /// </summary>
        public static IStreamingHubClientFactoryProvider Default { get; set; } = DynamicNotSupportedStreamingHubClientFactoryProvider.Instance;
    }

    public delegate TStreamingHub StreamingHubClientFactoryDelegate<out TStreamingHub, in TReceiver>(CallInvoker callInvoker, TReceiver receiver, string host, CallOptions callOptions, IMagicOnionSerializerProvider serializerProvider, IMagicOnionClientLogger logger)
        where TStreamingHub : IStreamingHub<TStreamingHub, TReceiver>;

    /// <summary>
    /// Provides to get a StreamingHubClient factory of the specified service type.
    /// </summary>
    public interface IStreamingHubClientFactoryProvider
    {
        /// <summary>
        /// Gets the StreamingHubClient factory of the specified service type. A return value indicates whether a factory was found or not.
        /// </summary>
        /// <typeparam name="TStreamingHub">A MagicOnion StreamingHub interface type.</typeparam>
        /// <typeparam name="TReceiver">A hub receiver interface type.</typeparam>
        /// <param name="factory">A StreamingHubClient factory of specified service type.</param>
        /// <returns>The value indicates whether a factory was found or not.</returns>
        bool TryGetFactory<TStreamingHub, TReceiver>(out StreamingHubClientFactoryDelegate<TStreamingHub, TReceiver> factory) where TStreamingHub : IStreamingHub<TStreamingHub, TReceiver>;
    }

    public class ImmutableStreamingHubClientFactoryProvider : IStreamingHubClientFactoryProvider
    {
        readonly IStreamingHubClientFactoryProvider[] providers;

        public ImmutableStreamingHubClientFactoryProvider(params IStreamingHubClientFactoryProvider[] providers)
        {
            this.providers = providers;
        }

        public ImmutableStreamingHubClientFactoryProvider Add(IStreamingHubClientFactoryProvider provider)
        {
            return new ImmutableStreamingHubClientFactoryProvider(providers.Append(provider).ToArray());
        }

        public bool TryGetFactory<TStreamingHub, TReceiver>(out StreamingHubClientFactoryDelegate<TStreamingHub, TReceiver> factory) where TStreamingHub : IStreamingHub<TStreamingHub, TReceiver>
        {
            foreach (var provider in providers)
            {
                if (provider.TryGetFactory<TStreamingHub, TReceiver>(out factory))
                {
                    return true;
                }
            }

            factory = default;
            return false;
        }
    }

    public class DynamicNotSupportedStreamingHubClientFactoryProvider : IStreamingHubClientFactoryProvider
    {
        public static IStreamingHubClientFactoryProvider Instance { get; } = new DynamicNotSupportedStreamingHubClientFactoryProvider();

        DynamicNotSupportedStreamingHubClientFactoryProvider() { }

        public bool TryGetFactory<TStreamingHub, TReceiver>(out StreamingHubClientFactoryDelegate<TStreamingHub, TReceiver> factory) where TStreamingHub : IStreamingHub<TStreamingHub, TReceiver>
        {
            throw new InvalidOperationException($"Unable to find a client factory of type '{typeof(TStreamingHub)}'. If the application is running on IL2CPP or AOT, dynamic code generation is not supported. Please use the code generator (moc).");
        }
    }
}
