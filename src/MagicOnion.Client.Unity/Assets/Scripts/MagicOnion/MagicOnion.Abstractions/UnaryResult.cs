using Grpc.Core;
using MagicOnion.Client;
using MagicOnion.CompilerServices; // require this using in AsyncMethodBuilder
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MessagePack;

namespace MagicOnion
{
    /// <summary>
    /// Represents the result of a Unary call that wraps AsyncUnaryCall as Task-like.
    /// </summary>
    [AsyncMethodBuilder(typeof(AsyncUnaryResultMethodBuilder))]
    public readonly struct UnaryResult
    {
        internal readonly bool hasRawValue;
        internal readonly Task rawTaskValue;
        internal readonly Task<IResponseContext<Nil>> response;

        public UnaryResult(Nil nil)
        {
            this.hasRawValue = true;
            this.rawTaskValue = default;
            this.response = null;
        }

        public UnaryResult(Task<Nil> rawTaskValue)
        {
            this.hasRawValue = true;
            this.rawTaskValue = rawTaskValue ?? throw new ArgumentNullException(nameof(rawTaskValue));
            this.response = null;
        }

        public UnaryResult(Task<IResponseContext<Nil>> response)
        {
            this.hasRawValue = false;
            this.rawTaskValue = default;
            this.response = response ?? throw new ArgumentNullException(nameof(response));
        }

        /// <summary>
        /// Asynchronous call result.
        /// </summary>
        public Task ResponseAsync
        {
            get
            {
                if (hasRawValue)
                {
                    if (rawTaskValue != null)
                    {
                        return rawTaskValue;
                    }
                }
                else
                {
                    // If the UnaryResult has no raw-value and no response, it is the default value of UnaryResult.
                    // So, we will return the default value of TResponse as Task.
                    if (response is null)
                    {
                        return Task.CompletedTask;
                    }

                    return UnwrapResponse();
                }

                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// Asynchronous access to response headers.
        /// </summary>
        public Task<Metadata> ResponseHeadersAsync => UnwrapResponseHeaders();

        async Task UnwrapResponse()
        {
            var ctx = await response.ConfigureAwait(false);
            await ctx.ResponseAsync.ConfigureAwait(false);
        }

        async Task<Metadata> UnwrapResponseHeaders()
        {
            var ctx = await response.ConfigureAwait(false);
            return await ctx.ResponseHeadersAsync.ConfigureAwait(false);
        }

        IResponseContext TryUnwrap()
        {
            if (!response.IsCompleted)
            {
                throw new InvalidOperationException("UnaryResult request is not yet completed, please await before call this.");
            }

            return response.Result;
        }

        /// <summary>
        /// Allows awaiting this object directly.
        /// </summary>
        public TaskAwaiter GetAwaiter()
            => ResponseAsync.GetAwaiter();

        /// <summary>
        /// Gets the call status if the call has already finished.
        /// Throws InvalidOperationException otherwise.
        /// </summary>
        public Status GetStatus()
            => TryUnwrap().GetStatus();

        /// <summary>
        /// Gets the call trailing metadata if the call has already finished.
        /// Throws InvalidOperationException otherwise.
        /// </summary>
        public Metadata GetTrailers()
            => TryUnwrap().GetTrailers();

        /// <summary>
        /// Provides means to cleanup after the call.
        /// If the call has already finished normally (request stream has been completed and call result has been received), doesn't do anything.
        /// Otherwise, requests cancellation of the call which should terminate all pending async operations associated with the call.
        /// As a result, all resources being used by the call should be released eventually.
        /// </summary>
        /// <remarks>
        /// Normally, there is no need for you to dispose the call unless you want to utilize the
        /// "Cancel" semantics of invoking <c>Dispose</c>.
        /// </remarks>
        public void Dispose()
        {
            if (!response.IsCompleted)
            {
                UnwrapDispose();
            }
            else
            {
                response.Result.Dispose();
            }
        }
        
        async void UnwrapDispose()
        {
            try
            {
                var ctx = await response.ConfigureAwait(false);
                ctx.Dispose();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Gets a completed <see cref="T:MagicOnion.UnaryResult" /> with the void result.
        /// </summary>
        public static UnaryResult CompletedResult => default;

        /// <summary>
        /// Creates a <see cref="T:MagicOnion.UnaryResult`1" /> with the specified result.
        /// </summary>
        public static UnaryResult<T> FromResult<T>(T value)
            => new UnaryResult<T>(value);

        /// <summary>
        /// Creates a <see cref="T:MagicOnion.UnaryResult`1" /> with the specified result task.
        /// </summary>
        public static UnaryResult<T> FromResult<T>(Task<T> task)
            => new UnaryResult<T>(task);
    }

    /// <summary>
    /// Represents the result of a Unary call that wraps AsyncUnaryCall as Task-like.
    /// </summary>
    [AsyncMethodBuilder(typeof(AsyncUnaryResultMethodBuilder<>))]
    public readonly struct UnaryResult<TResponse>
    {
        internal readonly bool hasRawValue; // internal
        internal readonly TResponse rawValue; // internal
        internal readonly Task<TResponse> rawTaskValue; // internal

        readonly Task<IResponseContext<TResponse>> response;

        public UnaryResult(TResponse rawValue)
        {
            this.hasRawValue = true;
            this.rawValue = rawValue;
            this.rawTaskValue = null;
            this.response = null;
        }

        public UnaryResult(Task<TResponse> rawTaskValue)
        {
            this.hasRawValue = true;
            this.rawValue = default;
            this.rawTaskValue = rawTaskValue ?? throw new ArgumentNullException(nameof(rawTaskValue));
            this.response = null;
        }

        public UnaryResult(Task<IResponseContext<TResponse>> response)
        {
            this.hasRawValue = false;
            this.rawValue = default;
            this.rawTaskValue = null;
            this.response = response ?? throw new ArgumentNullException(nameof(response));
        }

        /// <summary>
        /// Asynchronous call result.
        /// </summary>
        public Task<TResponse> ResponseAsync
        {
            get
            {
                if (!hasRawValue)
                {
                    // If the UnaryResult has no raw-value and no response, it is the default value of UnaryResult<TResponse>.
                    // So, we will return the default value of TResponse as Task.
                    if (response is null)
                    {
                        return Task.FromResult(default(TResponse));
                    }

                    return UnwrapResponse();
                }
                else if (rawTaskValue != null)
                {
                    return rawTaskValue;
                }
                else
                {
                    return Task.FromResult(rawValue);
                }
            }
        }

        /// <summary>
        /// Asynchronous access to response headers.
        /// </summary>
        public Task<Metadata> ResponseHeadersAsync => UnwrapResponseHeaders();

        async Task<TResponse> UnwrapResponse()
        {
            var ctx = await response.ConfigureAwait(false);
            return await ctx.ResponseAsync.ConfigureAwait(false);
        }

        async Task<Metadata> UnwrapResponseHeaders()
        {
            var ctx = await response.ConfigureAwait(false);
            return await ctx.ResponseHeadersAsync.ConfigureAwait(false);
        }

        async void UnwrapDispose()
        {
            try
            {
                var ctx = await response.ConfigureAwait(false);
                ctx.Dispose();
            }
            catch
            {
            }
        }

        IResponseContext<TResponse> TryUnwrap()
        {
            if (!response.IsCompleted)
            {
                throw new InvalidOperationException("UnaryResult request is not yet completed, please await before call this.");
            }

            return response.Result;
        }

        /// <summary>
        /// Allows awaiting this object directly.
        /// </summary>
        public TaskAwaiter<TResponse> GetAwaiter()
            => ResponseAsync.GetAwaiter();

        /// <summary>
        /// Gets the call status if the call has already finished.
        /// Throws InvalidOperationException otherwise.
        /// </summary>
        public Status GetStatus()
            => TryUnwrap().GetStatus();

        /// <summary>
        /// Gets the call trailing metadata if the call has already finished.
        /// Throws InvalidOperationException otherwise.
        /// </summary>
        public Metadata GetTrailers()
            => TryUnwrap().GetTrailers();

        /// <summary>
        /// Provides means to cleanup after the call.
        /// If the call has already finished normally (request stream has been completed and call result has been received), doesn't do anything.
        /// Otherwise, requests cancellation of the call which should terminate all pending async operations associated with the call.
        /// As a result, all resources being used by the call should be released eventually.
        /// </summary>
        /// <remarks>
        /// Normally, there is no need for you to dispose the call unless you want to utilize the
        /// "Cancel" semantics of invoking <c>Dispose</c>.
        /// </remarks>
        public void Dispose()
        {
            if (!response.IsCompleted)
            {
                UnwrapDispose();
            }
            else
            {
                response.Result.Dispose();
            }
        }
    }
}
