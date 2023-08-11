using System;
using Grpc.Core;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MagicOnion.Client
{
    public interface IMagicOnionAwareGrpcChannel
    {
        /// <summary>
        /// Register the StreamingHub under the management of the channel.
        /// </summary>
        void ManageStreamingHubClient(Type streamingHubInterfaceType, IStreamingHubMarker streamingHub, Func<Task> disposeAsync, Task waitForDisconnect);

        /// <summary>
        /// Gets all StreamingHubs that depends on the channel.
        /// </summary>
        /// <returns></returns>
        IReadOnlyCollection<ManagedStreamingHubInfo> GetAllManagedStreamingHubs();

        /// <summary>
        /// Create a <see cref="Grpc.Core.CallInvoker"/> from the channel.
        /// </summary>
        /// <returns></returns>
        CallInvoker CreateCallInvoker();
    }

    public readonly struct ManagedStreamingHubInfo
    {
        public Type StreamingHubType { get; }
        public IStreamingHubMarker Client { get; }

        public ManagedStreamingHubInfo(Type streamingHubType, IStreamingHubMarker client)
        {
            StreamingHubType = streamingHubType;
            Client = client;
        }
    }
}
