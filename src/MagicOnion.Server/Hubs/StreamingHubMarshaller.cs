using Grpc.Core;
using MagicOnion.Serialization;

namespace MagicOnion.Server.Hubs;

internal class StreamingHubMarshaller
{
    public static Marshaller<byte[]> CreateForRequest(MethodHandler methodHandler, IMagicOnionSerializer messageSerializer)
        => new Marshaller<byte[]>((data, ctx) =>
        {
            var writer = ctx.GetBufferWriter();
            var buffer = writer.GetSpan(data.Length);
            data.CopyTo(buffer);
            writer.Advance(data.Length);
            ctx.Complete();
        }, (ctx) => ctx.PayloadAsNewBuffer());

    public static Marshaller<byte[]> CreateForResponse(MethodHandler methodHandler, IMagicOnionSerializer messageSerializer)
        => new Marshaller<byte[]>((data, ctx) =>
        {
            var writer = ctx.GetBufferWriter();
            var buffer = writer.GetSpan(data.Length);
            data.CopyTo(buffer);
            writer.Advance(data.Length);
            ctx.Complete();
        }, (ctx) => ctx.PayloadAsNewBuffer());
}
