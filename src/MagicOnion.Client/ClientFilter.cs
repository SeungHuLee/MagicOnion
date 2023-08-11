using System;
using System.Threading.Tasks;

namespace MagicOnion.Client
{
    public interface IClientFilter
    {
        ValueTask<ResponseContext> SendAsync(RequestContext context, Func<RequestContext, ValueTask<ResponseContext>> next);
    }
}
