using Grpc.Core;
using MagicOnion.Internal;
using MagicOnion.Serialization;
using MessagePack;

namespace MagicOnion.Server.Internal;

#pragma warning disable CS8604

internal class MagicOnionServerMethod<TRequest, TResponse> : Method<TRequest, TResponse>
{
    public MethodHandler MagicOnionMethodHandler { get; }
    public MagicOnionServerMethod(Method<TRequest, TResponse> method, MethodHandler magicOnionMethodHandler)
        : base(method.Type, method.ServiceName, method.Name, method.RequestMarshaller, method.ResponseMarshaller)
    {
        MagicOnionMethodHandler = magicOnionMethodHandler;
    }
}


internal class MagicOnionMethodHandlerBinder<TRequest, TResponse, TRawRequest, TRawResponse>
    where TRawRequest : class
    where TRawResponse : class
{
    public static MagicOnionMethodHandlerBinder<TRequest, TResponse, TRawRequest, TRawResponse> Instance { get; } = new MagicOnionMethodHandlerBinder<TRequest, TResponse, TRawRequest, TRawResponse>();

    public void BindUnary(ServiceBinderBase binder, Func<TRequest, ServerCallContext, Task<TResponse?>> serverMethod, MethodHandler methodHandler, IMagicOnionSerializer messageSerializer)
    {
        var method = GrpcMethodHelper.CreateMethod<TRequest, TResponse, TRawRequest, TRawResponse>(MethodType.Unary, methodHandler.ServiceName, methodHandler.MethodName, methodHandler.MethodInfo, messageSerializer);
        binder.AddMethod(new MagicOnionServerMethod<TRawRequest, TRawResponse>(method.Method, methodHandler),
            async (request, context) => method.ToRawResponse(await serverMethod(method.FromRawRequest(request), context)));
    }

    public void BindUnaryParameterless(ServiceBinderBase binder, Func<Nil, ServerCallContext, Task<TResponse?>> serverMethod, MethodHandler methodHandler, IMagicOnionSerializer messageSerializer)
    {
        // WORKAROUND: Prior to MagicOnion 5.0, the request type for the parameter-less method was byte[].
        //             DynamicClient sends byte[], but GeneratedClient sends Nil, which is incompatible,
        //             so as a special case we do not serialize/deserialize and always convert to a fixed values.
        var method = GrpcMethodHelper.CreateMethod<TResponse, TRawResponse>(MethodType.Unary, methodHandler.ServiceName, methodHandler.MethodName, methodHandler.MethodInfo, messageSerializer);
        binder.AddMethod(new MagicOnionServerMethod<Box<Nil>, TRawResponse>(method.Method, methodHandler),
            async (request, context) => method.ToRawResponse(await serverMethod(method.FromRawRequest(request), context)));
    }

    public void BindStreamingHub(ServiceBinderBase binder, Func<IAsyncStreamReader<TRequest>, IServerStreamWriter<TResponse>, ServerCallContext, Task> serverMethod, MethodHandler methodHandler, IMagicOnionSerializer messageSerializer)
    {
        // StreamingHub uses the special marshallers for streaming messages serialization.
        // TODO: Currently, MagicOnion expects TRawRequest/TRawResponse to be raw-byte array (`bytes[]`).
        var method = new GrpcMethodHelper.MagicOnionMethod<TRequest, TResponse, TRawRequest, TRawResponse>(new Method<TRawRequest, TRawResponse>(
            MethodType.DuplexStreaming,
            methodHandler.ServiceName,
            methodHandler.MethodName,
            (Marshaller<TRawRequest>)(object)Hubs.StreamingHubMarshaller.CreateForRequest(methodHandler, messageSerializer),
            (Marshaller<TRawResponse>)(object)Hubs.StreamingHubMarshaller.CreateForResponse(methodHandler, messageSerializer)
        ));
        binder.AddMethod(new MagicOnionServerMethod<TRawRequest, TRawResponse>(method.Method, methodHandler),
            async (request, response, context) => await serverMethod(
                new MagicOnionAsyncStreamReader<TRequest, TRawRequest>(request, method.FromRawRequest),
                new MagicOnionServerStreamWriter<TResponse, TRawResponse>(response, method.ToRawResponse),
                context
            ));
    }

    public void BindDuplexStreaming(ServiceBinderBase binder, Func<IAsyncStreamReader<TRequest>, IServerStreamWriter<TResponse>, ServerCallContext, Task> serverMethod, MethodHandler methodHandler, IMagicOnionSerializer messageSerializer)
    {
        var method = GrpcMethodHelper.CreateMethod<TRequest, TResponse, TRawRequest, TRawResponse>(MethodType.DuplexStreaming, methodHandler.ServiceName, methodHandler.MethodName, methodHandler.MethodInfo, messageSerializer);
        binder.AddMethod(new MagicOnionServerMethod<TRawRequest, TRawResponse>(method.Method, methodHandler),
            async (request, response, context) => await serverMethod(
                new MagicOnionAsyncStreamReader<TRequest, TRawRequest>(request, method.FromRawRequest),
                new MagicOnionServerStreamWriter<TResponse, TRawResponse>(response, method.ToRawResponse),
                context
            ));
    }

    public void BindServerStreaming(ServiceBinderBase binder, Func<TRequest, IServerStreamWriter<TResponse>, ServerCallContext, Task> serverMethod, MethodHandler methodHandler, IMagicOnionSerializer messageSerializer)
    {
        var method = GrpcMethodHelper.CreateMethod<TRequest, TResponse, TRawRequest, TRawResponse>(MethodType.ServerStreaming, methodHandler.ServiceName, methodHandler.MethodName, methodHandler.MethodInfo, messageSerializer);
        binder.AddMethod(new MagicOnionServerMethod<TRawRequest, TRawResponse>(method.Method, methodHandler),
            async (request, response, context) => await serverMethod(
                method.FromRawRequest(request),
                new MagicOnionServerStreamWriter<TResponse, TRawResponse>(response, method.ToRawResponse),
                context
            ));
    }

    public void BindClientStreaming(ServiceBinderBase binder, Func<IAsyncStreamReader<TRequest>, ServerCallContext, Task<TResponse?>> serverMethod, MethodHandler methodHandler, IMagicOnionSerializer messageSerializer)
    {
        var method = GrpcMethodHelper.CreateMethod<TRequest, TResponse, TRawRequest, TRawResponse>(MethodType.ClientStreaming, methodHandler.ServiceName, methodHandler.MethodName, methodHandler.MethodInfo, messageSerializer);
        binder.AddMethod(new MagicOnionServerMethod<TRawRequest, TRawResponse>(method.Method, methodHandler),
            async (request, context) => method.ToRawResponse(await serverMethod(
                new MagicOnionAsyncStreamReader<TRequest, TRawRequest>(request, method.FromRawRequest),
                context
            )));
    }
}
#pragma warning restore CS8604
