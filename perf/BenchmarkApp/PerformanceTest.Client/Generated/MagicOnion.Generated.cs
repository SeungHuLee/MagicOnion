// <auto-generated />
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 219
#pragma warning disable 168

// NOTE: Disable warnings for nullable reference types.
// `#nullable disable` causes compile error on old C# compilers (-7.3)
#pragma warning disable 8603 // Possible null reference return.
#pragma warning disable 8618 // Non-nullable variable must contain a non-null value when exiting constructor. Consider declaring it as nullable.
#pragma warning disable 8625 // Cannot convert null literal to non-nullable reference type.

namespace MagicOnion
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::MagicOnion;
    using global::MagicOnion.Client;

    public static partial class MagicOnionInitializer
    {
        static bool isRegistered = false;

#if UNITY_2019_4_OR_NEWER
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
#elif NET5_0_OR_GREATER
        [System.Runtime.CompilerServices.ModuleInitializer]
#endif
        public static void Register()
        {
            if (isRegistered) return;
            isRegistered = true;

            global::MagicOnion.Client.MagicOnionClientFactoryProvider.Default =
                (global::MagicOnion.Client.MagicOnionClientFactoryProvider.Default is global::MagicOnion.Client.ImmutableMagicOnionClientFactoryProvider immutableMagicOnionClientFactoryProvider)
                    ? immutableMagicOnionClientFactoryProvider.Add(MagicOnionGeneratedClientFactoryProvider.Instance)
                    : new global::MagicOnion.Client.ImmutableMagicOnionClientFactoryProvider(MagicOnionGeneratedClientFactoryProvider.Instance);

            global::MagicOnion.Client.StreamingHubClientFactoryProvider.Default =
                (global::MagicOnion.Client.StreamingHubClientFactoryProvider.Default is global::MagicOnion.Client.ImmutableStreamingHubClientFactoryProvider immutableStreamingHubClientFactoryProvider)
                    ? immutableStreamingHubClientFactoryProvider.Add(MagicOnionGeneratedClientFactoryProvider.Instance)
                    : new global::MagicOnion.Client.ImmutableStreamingHubClientFactoryProvider(MagicOnionGeneratedClientFactoryProvider.Instance);
        }
    }

    public partial class MagicOnionGeneratedClientFactoryProvider : global::MagicOnion.Client.IMagicOnionClientFactoryProvider, global::MagicOnion.Client.IStreamingHubClientFactoryProvider
    {
        public static MagicOnionGeneratedClientFactoryProvider Instance { get; } = new MagicOnionGeneratedClientFactoryProvider();

        MagicOnionGeneratedClientFactoryProvider() { }

        bool global::MagicOnion.Client.IMagicOnionClientFactoryProvider.TryGetFactory<T>(out global::MagicOnion.Client.MagicOnionClientFactoryDelegate<T> factory)
            => (factory = MagicOnionClientFactoryCache<T>.Factory) != null;

        bool global::MagicOnion.Client.IStreamingHubClientFactoryProvider.TryGetFactory<TStreamingHub, TReceiver>(out global::MagicOnion.Client.StreamingHubClientFactoryDelegate<TStreamingHub, TReceiver> factory)
            => (factory = StreamingHubClientFactoryCache<TStreamingHub, TReceiver>.Factory) != null;

        static class MagicOnionClientFactoryCache<T> where T : global::MagicOnion.IService<T>
        {
            public readonly static global::MagicOnion.Client.MagicOnionClientFactoryDelegate<T> Factory;

            static MagicOnionClientFactoryCache()
            {
                object factory = default(global::MagicOnion.Client.MagicOnionClientFactoryDelegate<T>);

                if (typeof(T) == typeof(global::PerformanceTest.Shared.IPerfTestService))
                {
                    factory = ((global::MagicOnion.Client.MagicOnionClientFactoryDelegate<global::PerformanceTest.Shared.IPerfTestService>)((x, y) => new PerformanceTest.Shared.PerfTestServiceClient(x, y)));
                }
                Factory = (global::MagicOnion.Client.MagicOnionClientFactoryDelegate<T>)factory;
            }
        }

        static class StreamingHubClientFactoryCache<TStreamingHub, TReceiver> where TStreamingHub : global::MagicOnion.IStreamingHub<TStreamingHub, TReceiver>
        {
            public readonly static global::MagicOnion.Client.StreamingHubClientFactoryDelegate<TStreamingHub, TReceiver> Factory;

            static StreamingHubClientFactoryCache()
            {
                object factory = default(global::MagicOnion.Client.StreamingHubClientFactoryDelegate<TStreamingHub, TReceiver>);

                if (typeof(TStreamingHub) == typeof(global::PerformanceTest.Shared.IPerfTestHub) && typeof(TReceiver) == typeof(global::PerformanceTest.Shared.IPerfTestHubReceiver))
                {
                    factory = ((global::MagicOnion.Client.StreamingHubClientFactoryDelegate<global::PerformanceTest.Shared.IPerfTestHub, global::PerformanceTest.Shared.IPerfTestHubReceiver>)((a, _, b, c, d, e) => new PerformanceTest.Shared.PerfTestHubClient(a, b, c, d, e)));
                }

                Factory = (global::MagicOnion.Client.StreamingHubClientFactoryDelegate<TStreamingHub, TReceiver>)factory;
            }
        }
    }

}

#pragma warning restore 168
#pragma warning restore 219
#pragma warning restore 414
#pragma warning restore 612
#pragma warning restore 618

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 219
#pragma warning disable 168

// NOTE: Disable warnings for nullable reference types.
// `#nullable disable` causes compile error on old C# compilers (-7.3)
#pragma warning disable 8603 // Possible null reference return.
#pragma warning disable 8618 // Non-nullable variable must contain a non-null value when exiting constructor. Consider declaring it as nullable.
#pragma warning disable 8625 // Cannot convert null literal to non-nullable reference type.
namespace MagicOnion
{
    using global::System;
    using global::MemoryPack;
    public class MagicOnionMemoryPackFormatterProvider
    {
        public static void RegisterFormatters()
        {
            global::MemoryPack.MemoryPackFormatterProvider.Register(new global::MagicOnion.Serialization.MemoryPack.DynamicArgumentTupleFormatter<global::System.String, global::System.Int32, global::System.Int32, global::System.Int32, global::System.Byte[]>());
            global::MemoryPack.MemoryPackFormatterProvider.Register(new global::MagicOnion.Serialization.MemoryPack.DynamicArgumentTupleFormatter<global::System.String, global::System.Int32, global::System.Int32, global::System.Int32>());
            global::MemoryPack.MemoryPackFormatterProvider.Register(new global::MagicOnion.Serialization.MemoryPack.DynamicArgumentTupleFormatter<global::System.String, global::System.Int32, global::System.Int32>());
            global::MemoryPack.MemoryPackFormatterProvider.Register(new global::MemoryPack.Formatters.ValueTupleFormatter<global::System.Int32, global::System.Byte[]>());
        }
    }
}
#pragma warning restore 168
#pragma warning restore 219
#pragma warning restore 414
#pragma warning restore 612
#pragma warning restore 618

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 219
#pragma warning disable 168

// NOTE: Disable warnings for nullable reference types.
// `#nullable disable` causes compile error on old C# compilers (-7.3)
#pragma warning disable 8603 // Possible null reference return.
#pragma warning disable 8618 // Non-nullable variable must contain a non-null value when exiting constructor. Consider declaring it as nullable.
#pragma warning disable 8625 // Cannot convert null literal to non-nullable reference type.

namespace PerformanceTest.Shared
{
    using global::System;
    using global::Grpc.Core;
    using global::MagicOnion;
    using global::MagicOnion.Client;
    using global::MessagePack;

    [global::MagicOnion.Ignore]
    public class PerfTestServiceClient : global::MagicOnion.Client.MagicOnionClientBase<global::PerformanceTest.Shared.IPerfTestService>, global::PerformanceTest.Shared.IPerfTestService
    {
        class ClientCore
        {
            public global::MagicOnion.Client.Internal.RawMethodInvoker<global::MessagePack.Nil, global::PerformanceTest.Shared.ServerInformation> GetServerInformationAsync;
            public global::MagicOnion.Client.Internal.RawMethodInvoker<global::MessagePack.Nil, global::MessagePack.Nil> UnaryParameterless;
            public global::MagicOnion.Client.Internal.RawMethodInvoker<global::MagicOnion.DynamicArgumentTuple<global::System.String, global::System.Int32, global::System.Int32>, global::System.String> UnaryArgRefReturnRef;
            public global::MagicOnion.Client.Internal.RawMethodInvoker<global::MagicOnion.DynamicArgumentTuple<global::System.String, global::System.Int32, global::System.Int32, global::System.Int32>, global::System.String> UnaryArgDynamicArgumentTupleReturnRef;
            public global::MagicOnion.Client.Internal.RawMethodInvoker<global::MagicOnion.DynamicArgumentTuple<global::System.String, global::System.Int32, global::System.Int32, global::System.Int32>, global::System.Int32> UnaryArgDynamicArgumentTupleReturnValue;
            public global::MagicOnion.Client.Internal.RawMethodInvoker<global::MagicOnion.DynamicArgumentTuple<global::System.String, global::System.Int32, global::System.Int32, global::System.Int32, global::System.Byte[]>, global::System.ValueTuple<global::System.Int32, global::System.Byte[]>> UnaryLargePayloadAsync;
            public global::MagicOnion.Client.Internal.RawMethodInvoker<global::MagicOnion.DynamicArgumentTuple<global::System.String, global::System.Int32, global::System.Int32, global::System.Int32>, global::PerformanceTest.Shared.ComplexResponse> UnaryComplexAsync;
            public ClientCore(global::MagicOnion.Serialization.IMagicOnionSerializerProvider serializerProvider)
            {
                this.GetServerInformationAsync = global::MagicOnion.Client.Internal.RawMethodInvoker.Create_ValueType_RefType<global::MessagePack.Nil, global::PerformanceTest.Shared.ServerInformation>(global::Grpc.Core.MethodType.Unary, "IPerfTestService", "GetServerInformationAsync", serializerProvider);
                this.UnaryParameterless = global::MagicOnion.Client.Internal.RawMethodInvoker.Create_ValueType_ValueType<global::MessagePack.Nil, global::MessagePack.Nil>(global::Grpc.Core.MethodType.Unary, "IPerfTestService", "UnaryParameterless", serializerProvider);
                this.UnaryArgRefReturnRef = global::MagicOnion.Client.Internal.RawMethodInvoker.Create_ValueType_RefType<global::MagicOnion.DynamicArgumentTuple<global::System.String, global::System.Int32, global::System.Int32>, global::System.String>(global::Grpc.Core.MethodType.Unary, "IPerfTestService", "UnaryArgRefReturnRef", serializerProvider);
                this.UnaryArgDynamicArgumentTupleReturnRef = global::MagicOnion.Client.Internal.RawMethodInvoker.Create_ValueType_RefType<global::MagicOnion.DynamicArgumentTuple<global::System.String, global::System.Int32, global::System.Int32, global::System.Int32>, global::System.String>(global::Grpc.Core.MethodType.Unary, "IPerfTestService", "UnaryArgDynamicArgumentTupleReturnRef", serializerProvider);
                this.UnaryArgDynamicArgumentTupleReturnValue = global::MagicOnion.Client.Internal.RawMethodInvoker.Create_ValueType_ValueType<global::MagicOnion.DynamicArgumentTuple<global::System.String, global::System.Int32, global::System.Int32, global::System.Int32>, global::System.Int32>(global::Grpc.Core.MethodType.Unary, "IPerfTestService", "UnaryArgDynamicArgumentTupleReturnValue", serializerProvider);
                this.UnaryLargePayloadAsync = global::MagicOnion.Client.Internal.RawMethodInvoker.Create_ValueType_ValueType<global::MagicOnion.DynamicArgumentTuple<global::System.String, global::System.Int32, global::System.Int32, global::System.Int32, global::System.Byte[]>, global::System.ValueTuple<global::System.Int32, global::System.Byte[]>>(global::Grpc.Core.MethodType.Unary, "IPerfTestService", "UnaryLargePayloadAsync", serializerProvider);
                this.UnaryComplexAsync = global::MagicOnion.Client.Internal.RawMethodInvoker.Create_ValueType_RefType<global::MagicOnion.DynamicArgumentTuple<global::System.String, global::System.Int32, global::System.Int32, global::System.Int32>, global::PerformanceTest.Shared.ComplexResponse>(global::Grpc.Core.MethodType.Unary, "IPerfTestService", "UnaryComplexAsync", serializerProvider);
            }
        }

        readonly ClientCore core;

        public PerfTestServiceClient(global::MagicOnion.Client.MagicOnionClientOptions options, global::MagicOnion.Serialization.IMagicOnionSerializerProvider serializerProvider) : base(options)
        {
            this.core = new ClientCore(serializerProvider);
        }

        private PerfTestServiceClient(MagicOnionClientOptions options, ClientCore core) : base(options)
        {
            this.core = core;
        }

        protected override global::MagicOnion.Client.MagicOnionClientBase<IPerfTestService> Clone(global::MagicOnion.Client.MagicOnionClientOptions options)
            => new PerfTestServiceClient(options, core);

        public global::MagicOnion.UnaryResult<global::PerformanceTest.Shared.ServerInformation> GetServerInformationAsync()
            => this.core.GetServerInformationAsync.InvokeUnary(this, "IPerfTestService/GetServerInformationAsync", global::MessagePack.Nil.Default);
        public global::MagicOnion.UnaryResult<global::MessagePack.Nil> UnaryParameterless()
            => this.core.UnaryParameterless.InvokeUnary(this, "IPerfTestService/UnaryParameterless", global::MessagePack.Nil.Default);
        public global::MagicOnion.UnaryResult<global::System.String> UnaryArgRefReturnRef(global::System.String arg1, global::System.Int32 arg2, global::System.Int32 arg3)
            => this.core.UnaryArgRefReturnRef.InvokeUnary(this, "IPerfTestService/UnaryArgRefReturnRef", new global::MagicOnion.DynamicArgumentTuple<global::System.String, global::System.Int32, global::System.Int32>(arg1, arg2, arg3));
        public global::MagicOnion.UnaryResult<global::System.String> UnaryArgDynamicArgumentTupleReturnRef(global::System.String arg1, global::System.Int32 arg2, global::System.Int32 arg3, global::System.Int32 arg4)
            => this.core.UnaryArgDynamicArgumentTupleReturnRef.InvokeUnary(this, "IPerfTestService/UnaryArgDynamicArgumentTupleReturnRef", new global::MagicOnion.DynamicArgumentTuple<global::System.String, global::System.Int32, global::System.Int32, global::System.Int32>(arg1, arg2, arg3, arg4));
        public global::MagicOnion.UnaryResult<global::System.Int32> UnaryArgDynamicArgumentTupleReturnValue(global::System.String arg1, global::System.Int32 arg2, global::System.Int32 arg3, global::System.Int32 arg4)
            => this.core.UnaryArgDynamicArgumentTupleReturnValue.InvokeUnary(this, "IPerfTestService/UnaryArgDynamicArgumentTupleReturnValue", new global::MagicOnion.DynamicArgumentTuple<global::System.String, global::System.Int32, global::System.Int32, global::System.Int32>(arg1, arg2, arg3, arg4));
        public global::MagicOnion.UnaryResult<global::System.ValueTuple<global::System.Int32, global::System.Byte[]>> UnaryLargePayloadAsync(global::System.String arg1, global::System.Int32 arg2, global::System.Int32 arg3, global::System.Int32 arg4, global::System.Byte[] arg5)
            => this.core.UnaryLargePayloadAsync.InvokeUnary(this, "IPerfTestService/UnaryLargePayloadAsync", new global::MagicOnion.DynamicArgumentTuple<global::System.String, global::System.Int32, global::System.Int32, global::System.Int32, global::System.Byte[]>(arg1, arg2, arg3, arg4, arg5));
        public global::MagicOnion.UnaryResult<global::PerformanceTest.Shared.ComplexResponse> UnaryComplexAsync(global::System.String arg1, global::System.Int32 arg2, global::System.Int32 arg3, global::System.Int32 arg4)
            => this.core.UnaryComplexAsync.InvokeUnary(this, "IPerfTestService/UnaryComplexAsync", new global::MagicOnion.DynamicArgumentTuple<global::System.String, global::System.Int32, global::System.Int32, global::System.Int32>(arg1, arg2, arg3, arg4));
    }
}


#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 219
#pragma warning disable 168

// NOTE: Disable warnings for nullable reference types.
// `#nullable disable` causes compile error on old C# compilers (-7.3)
#pragma warning disable 8603 // Possible null reference return.
#pragma warning disable 8618 // Non-nullable variable must contain a non-null value when exiting constructor. Consider declaring it as nullable.
#pragma warning disable 8625 // Cannot convert null literal to non-nullable reference type.

namespace PerformanceTest.Shared
{
    using global::System;
    using global::Grpc.Core;
    using global::MagicOnion;
    using global::MagicOnion.Client;
    using global::MessagePack;

    [global::MagicOnion.Ignore]
    public class PerfTestHubClient : global::MagicOnion.Client.StreamingHubClientBase<global::PerformanceTest.Shared.IPerfTestHub, global::PerformanceTest.Shared.IPerfTestHubReceiver>, global::PerformanceTest.Shared.IPerfTestHub
    {
        protected override global::Grpc.Core.Method<global::System.Byte[], global::System.Byte[]> DuplexStreamingAsyncMethod { get; }

        public PerfTestHubClient(global::Grpc.Core.CallInvoker callInvoker, global::System.String host, global::Grpc.Core.CallOptions options, global::MagicOnion.Serialization.IMagicOnionSerializerProvider serializerProvider, global::MagicOnion.Client.IMagicOnionClientLogger logger)
            : base(callInvoker, host, options, serializerProvider, logger)
        {
            var marshaller = global::MagicOnion.MagicOnionMarshallers.ThroughMarshaller;
            DuplexStreamingAsyncMethod = new global::Grpc.Core.Method<global::System.Byte[], global::System.Byte[]>(global::Grpc.Core.MethodType.DuplexStreaming, "IPerfTestHub", "Connect", marshaller, marshaller);
        }

        public global::System.Threading.Tasks.Task<global::System.Int32> CallMethodAsync(global::System.String arg1, global::System.Int32 arg2, global::System.Int32 arg3, global::System.Int32 arg4)
            => base.WriteMessageWithResponseAsync<global::MagicOnion.DynamicArgumentTuple<global::System.String, global::System.Int32, global::System.Int32, global::System.Int32>, global::System.Int32>(-768954304, new global::MagicOnion.DynamicArgumentTuple<global::System.String, global::System.Int32, global::System.Int32, global::System.Int32>(arg1, arg2, arg3, arg4));
        public global::System.Threading.Tasks.Task<global::PerformanceTest.Shared.ComplexResponse> CallMethodComplexAsync(global::System.String arg1, global::System.Int32 arg2, global::System.Int32 arg3, global::System.Int32 arg4)
            => base.WriteMessageWithResponseAsync<global::MagicOnion.DynamicArgumentTuple<global::System.String, global::System.Int32, global::System.Int32, global::System.Int32>, global::PerformanceTest.Shared.ComplexResponse>(-693935228, new global::MagicOnion.DynamicArgumentTuple<global::System.String, global::System.Int32, global::System.Int32, global::System.Int32>(arg1, arg2, arg3, arg4));
        public global::System.Threading.Tasks.Task<global::System.ValueTuple<global::System.Int32, global::System.Byte[]>> CallMethodLargePayloadAsync(global::System.String arg1, global::System.Int32 arg2, global::System.Int32 arg3, global::System.Int32 arg4, global::System.Byte[] arg5)
            => base.WriteMessageWithResponseAsync<global::MagicOnion.DynamicArgumentTuple<global::System.String, global::System.Int32, global::System.Int32, global::System.Int32, global::System.Byte[]>, global::System.ValueTuple<global::System.Int32, global::System.Byte[]>>(-1734099635, new global::MagicOnion.DynamicArgumentTuple<global::System.String, global::System.Int32, global::System.Int32, global::System.Int32, global::System.Byte[]>(arg1, arg2, arg3, arg4, arg5));

        public global::PerformanceTest.Shared.IPerfTestHub FireAndForget()
            => new FireAndForgetClient(this);

        [global::MagicOnion.Ignore]
        class FireAndForgetClient : global::PerformanceTest.Shared.IPerfTestHub
        {
            readonly PerfTestHubClient parent;

            public FireAndForgetClient(PerfTestHubClient parent)
                => this.parent = parent;

            public global::PerformanceTest.Shared.IPerfTestHub FireAndForget() => this;
            public global::System.Threading.Tasks.Task DisposeAsync() => throw new global::System.NotSupportedException();
            public global::System.Threading.Tasks.Task WaitForDisconnect() => throw new global::System.NotSupportedException();

            public global::System.Threading.Tasks.Task<global::System.Int32> CallMethodAsync(global::System.String arg1, global::System.Int32 arg2, global::System.Int32 arg3, global::System.Int32 arg4)
                => parent.WriteMessageFireAndForgetAsync<global::MagicOnion.DynamicArgumentTuple<global::System.String, global::System.Int32, global::System.Int32, global::System.Int32>, global::System.Int32>(-768954304, new global::MagicOnion.DynamicArgumentTuple<global::System.String, global::System.Int32, global::System.Int32, global::System.Int32>(arg1, arg2, arg3, arg4));
            public global::System.Threading.Tasks.Task<global::PerformanceTest.Shared.ComplexResponse> CallMethodComplexAsync(global::System.String arg1, global::System.Int32 arg2, global::System.Int32 arg3, global::System.Int32 arg4)
                => parent.WriteMessageFireAndForgetAsync<global::MagicOnion.DynamicArgumentTuple<global::System.String, global::System.Int32, global::System.Int32, global::System.Int32>, global::PerformanceTest.Shared.ComplexResponse>(-693935228, new global::MagicOnion.DynamicArgumentTuple<global::System.String, global::System.Int32, global::System.Int32, global::System.Int32>(arg1, arg2, arg3, arg4));
            public global::System.Threading.Tasks.Task<global::System.ValueTuple<global::System.Int32, global::System.Byte[]>> CallMethodLargePayloadAsync(global::System.String arg1, global::System.Int32 arg2, global::System.Int32 arg3, global::System.Int32 arg4, global::System.Byte[] arg5)
                => parent.WriteMessageFireAndForgetAsync<global::MagicOnion.DynamicArgumentTuple<global::System.String, global::System.Int32, global::System.Int32, global::System.Int32, global::System.Byte[]>, global::System.ValueTuple<global::System.Int32, global::System.Byte[]>>(-1734099635, new global::MagicOnion.DynamicArgumentTuple<global::System.String, global::System.Int32, global::System.Int32, global::System.Int32, global::System.Byte[]>(arg1, arg2, arg3, arg4, arg5));

        }

        protected override void OnBroadcastEvent(global::System.Int32 methodId, global::System.ArraySegment<global::System.Byte> data)
        {
            switch (methodId)
            {
            }
        }

        protected override void OnResponseEvent(global::System.Int32 methodId, global::System.Object taskCompletionSource, global::System.ArraySegment<global::System.Byte> data)
        {
            switch (methodId)
            {
                case -768954304: // Task<Int32> CallMethodAsync(global::System.String arg1, global::System.Int32 arg2, global::System.Int32 arg3, global::System.Int32 arg4)
                    base.SetResultForResponse<global::System.Int32>(taskCompletionSource, data);
                    break;
                case -693935228: // Task<ComplexResponse> CallMethodComplexAsync(global::System.String arg1, global::System.Int32 arg2, global::System.Int32 arg3, global::System.Int32 arg4)
                    base.SetResultForResponse<global::PerformanceTest.Shared.ComplexResponse>(taskCompletionSource, data);
                    break;
                case -1734099635: // Task<ValueTuple<Int32, Byte[]>> CallMethodLargePayloadAsync(global::System.String arg1, global::System.Int32 arg2, global::System.Int32 arg3, global::System.Int32 arg4, global::System.Byte[] arg5)
                    base.SetResultForResponse<global::System.ValueTuple<global::System.Int32, global::System.Byte[]>>(taskCompletionSource, data);
                    break;
            }
        }

    }
}


