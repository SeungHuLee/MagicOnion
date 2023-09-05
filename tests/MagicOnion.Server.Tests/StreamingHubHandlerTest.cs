using System.Buffers;
using Grpc.Core;
using MagicOnion.Serialization;
using MagicOnion.Server.Hubs;
using MessagePack;
using Microsoft.Extensions.DependencyInjection;

namespace MagicOnion.Server.Tests;

public class StreamingHubHandlerTest
{
    [Fact]
    public async Task Parameterless_Returns_Task()
    {
        // Arrange
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();
        var hubType = typeof(StreamingHubHandlerTestHub);
        var hubMethod = hubType.GetMethod(nameof(StreamingHubHandlerTestHub.Method_Parameterless_Returns_Task))!;
        var hubInstance = new StreamingHubHandlerTestHub();
        var fakeStreamingHubContext = new FakeStreamingServiceContext<byte[], byte[]>(hubType, hubMethod, MessagePackMagicOnionSerializerProvider.Default.Create(MethodType.DuplexStreaming, null), serviceProvider);

        // Act
        var handler = new StreamingHubHandler(hubType, hubMethod, new StreamingHubHandlerOptions(new MagicOnionOptions()), serviceProvider);
        var ctx = new StreamingHubContext()
        {
            HubInstance = hubInstance,
            ServiceContext = fakeStreamingHubContext,
            Request = MessagePackSerializer.Serialize<Nil>(Nil.Default),
        };
        await handler.MethodBody.Invoke(ctx);

        // Assert
        hubInstance.Results.Should().Contain(nameof(StreamingHubHandlerTestHub.Method_Parameterless_Returns_Task) + " called.");
        byte[] BuildMessage()
        {
            // [MessageId, MethodId, Nil]
            var buffer = new ArrayBufferWriter<byte>();
            var writer = new MessagePackWriter(buffer);
            writer.WriteArrayHeader(3);
            writer.Write(ctx.MessageId);
            writer.Write(ctx.MethodId);
            writer.WriteNil();
            writer.Flush();

            return buffer.WrittenMemory.ToArray();
        }
        fakeStreamingHubContext.Responses[0].Should().Equal(BuildMessage());
    }

    [Fact]
    public async Task Parameterless_Returns_TaskOfInt32()
    {
        // Arrange
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();
        var hubType = typeof(StreamingHubHandlerTestHub);
        var hubMethod = hubType.GetMethod(nameof(StreamingHubHandlerTestHub.Method_Parameterless_Returns_TaskOfInt32))!;
        var hubInstance = new StreamingHubHandlerTestHub();
        var fakeStreamingHubContext = new FakeStreamingServiceContext<byte[], byte[]>(hubType, hubMethod, MessagePackMagicOnionSerializerProvider.Default.Create(MethodType.DuplexStreaming, null), serviceProvider);

        // Act
        var handler = new StreamingHubHandler(hubType, hubMethod, new StreamingHubHandlerOptions(new MagicOnionOptions()), serviceProvider);
        var ctx = new StreamingHubContext()
        {
            HubInstance = hubInstance,
            ServiceContext = fakeStreamingHubContext,
            Request = MessagePackSerializer.Serialize<Nil>(Nil.Default),
        };
        await handler.MethodBody.Invoke(ctx);

        // Assert
        hubInstance.Results.Should().Contain(nameof(StreamingHubHandlerTestHub.Method_Parameterless_Returns_TaskOfInt32) + " called.");
        byte[] BuildMessage()
        {
            // [MessageId, MethodId, {Int32:12345}]
            var buffer = new ArrayBufferWriter<byte>();
            var writer = new MessagePackWriter(buffer);
            writer.WriteArrayHeader(3);
            writer.Write(ctx.MessageId);
            writer.Write(ctx.MethodId);
            MessagePackSerializer.Serialize(ref writer, 12345);
            writer.Flush();

            return buffer.WrittenMemory.ToArray();
        }
        fakeStreamingHubContext.Responses[0].Should().Equal(BuildMessage());
    }
        [Fact]
    public async Task Parameterless_Returns_ValueTask()
    {
        // Arrange
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();
        var hubType = typeof(StreamingHubHandlerTestHub);
        var hubMethod = hubType.GetMethod(nameof(StreamingHubHandlerTestHub.Method_Parameterless_Returns_ValueTask))!;
        var hubInstance = new StreamingHubHandlerTestHub();
        var fakeStreamingHubContext = new FakeStreamingServiceContext<byte[], byte[]>(hubType, hubMethod, MessagePackMagicOnionSerializerProvider.Default.Create(MethodType.DuplexStreaming, null), serviceProvider);

        // Act
        var handler = new StreamingHubHandler(hubType, hubMethod, new StreamingHubHandlerOptions(new MagicOnionOptions()), serviceProvider);
        var ctx = new StreamingHubContext()
        {
            HubInstance = hubInstance,
            ServiceContext = fakeStreamingHubContext,
            Request = MessagePackSerializer.Serialize<Nil>(Nil.Default),
        };
        await handler.MethodBody.Invoke(ctx);

        // Assert
        hubInstance.Results.Should().Contain(nameof(StreamingHubHandlerTestHub.Method_Parameterless_Returns_ValueTask) + " called.");
        byte[] BuildMessage()
        {
            // [MessageId, MethodId, Nil]
            var buffer = new ArrayBufferWriter<byte>();
            var writer = new MessagePackWriter(buffer);
            writer.WriteArrayHeader(3);
            writer.Write(ctx.MessageId);
            writer.Write(ctx.MethodId);
            writer.WriteNil();
            writer.Flush();

            return buffer.WrittenMemory.ToArray();
        }
        fakeStreamingHubContext.Responses[0].Should().Equal(BuildMessage());
    }

    [Fact]
    public async Task Parameterless_Returns_ValueTaskOfInt32()
    {
        // Arrange
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();
        var hubType = typeof(StreamingHubHandlerTestHub);
        var hubMethod = hubType.GetMethod(nameof(StreamingHubHandlerTestHub.Method_Parameterless_Returns_ValueTaskOfInt32))!;
        var hubInstance = new StreamingHubHandlerTestHub();
        var fakeStreamingHubContext = new FakeStreamingServiceContext<byte[], byte[]>(hubType, hubMethod, MessagePackMagicOnionSerializerProvider.Default.Create(MethodType.DuplexStreaming, null), serviceProvider);

        // Act
        var handler = new StreamingHubHandler(hubType, hubMethod, new StreamingHubHandlerOptions(new MagicOnionOptions()), serviceProvider);
        var ctx = new StreamingHubContext()
        {
            HubInstance = hubInstance,
            ServiceContext = fakeStreamingHubContext,
            Request = MessagePackSerializer.Serialize<Nil>(Nil.Default),
        };
        await handler.MethodBody.Invoke(ctx);

        // Assert
        hubInstance.Results.Should().Contain(nameof(StreamingHubHandlerTestHub.Method_Parameterless_Returns_ValueTaskOfInt32) + " called.");
        byte[] BuildMessage()
        {
            // [MessageId, MethodId, {Int32:12345}]
            var buffer = new ArrayBufferWriter<byte>();
            var writer = new MessagePackWriter(buffer);
            writer.WriteArrayHeader(3);
            writer.Write(ctx.MessageId);
            writer.Write(ctx.MethodId);
            MessagePackSerializer.Serialize(ref writer, 12345);
            writer.Flush();

            return buffer.WrittenMemory.ToArray();
        }
        fakeStreamingHubContext.Responses[0].Should().Equal(BuildMessage());
    }

    [Fact]
    public async Task Parameter_Single_Returns_Task()
    {
        // Arrange
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();
        var hubType = typeof(StreamingHubHandlerTestHub);
        var hubMethod = hubType.GetMethod(nameof(StreamingHubHandlerTestHub.Method_Parameter_Single_Returns_Task))!;
        var hubInstance = new StreamingHubHandlerTestHub();
        var fakeStreamingHubContext = new FakeStreamingServiceContext<byte[], byte[]>(hubType, hubMethod, MessagePackMagicOnionSerializerProvider.Default.Create(MethodType.DuplexStreaming, null), serviceProvider);

        // Act
        var handler = new StreamingHubHandler(hubType, hubMethod, new StreamingHubHandlerOptions(new MagicOnionOptions()), serviceProvider);
        var ctx = new StreamingHubContext()
        {
            HubInstance = hubInstance,
            ServiceContext = fakeStreamingHubContext,
            Request = MessagePackSerializer.Serialize(12345),
        };
        await handler.MethodBody.Invoke(ctx);

        // Assert
        hubInstance.Results.Should().Contain(nameof(StreamingHubHandlerTestHub.Method_Parameter_Single_Returns_Task) + "(12345) called.");
        byte[] BuildMessage()
        {
            // [MessageId, MethodId, Nil]
            var buffer = new ArrayBufferWriter<byte>();
            var writer = new MessagePackWriter(buffer);
            writer.WriteArrayHeader(3);
            writer.Write(ctx.MessageId);
            writer.Write(ctx.MethodId);
            writer.WriteNil();
            writer.Flush();

            return buffer.WrittenMemory.ToArray();
        }
        fakeStreamingHubContext.Responses[0].Should().Equal(BuildMessage());
    }

    [Fact]
    public async Task Parameter_Multiple_Returns_Task()
    {
        // Arrange
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();
        var hubType = typeof(StreamingHubHandlerTestHub);
        var hubMethod = hubType.GetMethod(nameof(StreamingHubHandlerTestHub.Method_Parameter_Multiple_Returns_Task))!;
        var hubInstance = new StreamingHubHandlerTestHub();
        var fakeStreamingHubContext = new FakeStreamingServiceContext<byte[], byte[]>(hubType, hubMethod, MessagePackMagicOnionSerializerProvider.Default.Create(MethodType.DuplexStreaming, null), serviceProvider);

        // Act
        var handler = new StreamingHubHandler(hubType, hubMethod, new StreamingHubHandlerOptions(new MagicOnionOptions()), serviceProvider);
        var ctx = new StreamingHubContext()
        {
            HubInstance = hubInstance,
            ServiceContext = fakeStreamingHubContext,
            Request = MessagePackSerializer.Serialize(new DynamicArgumentTuple<int, string, bool>(12345, "テスト", true)),
        };
        await handler.MethodBody.Invoke(ctx);

        // Assert
        hubInstance.Results.Should().Contain(nameof(StreamingHubHandlerTestHub.Method_Parameter_Multiple_Returns_Task) + "(12345,テスト,True) called.");
        byte[] BuildMessage()
        {
            // [MessageId, MethodId, Nil]
            var buffer = new ArrayBufferWriter<byte>();
            var writer = new MessagePackWriter(buffer);
            writer.WriteArrayHeader(3);
            writer.Write(ctx.MessageId);
            writer.Write(ctx.MethodId);
            writer.WriteNil();
            writer.Flush();
            return buffer.WrittenMemory.ToArray();
        }
        fakeStreamingHubContext.Responses[0].Should().Equal(BuildMessage());
    }

    [Fact]
    public async Task Parameter_Multiple_Returns_TaskOfInt32()
    {
        // Arrange
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();
        var hubType = typeof(StreamingHubHandlerTestHub);
        var hubMethod = hubType.GetMethod(nameof(StreamingHubHandlerTestHub.Method_Parameter_Multiple_Returns_TaskOfInt32))!;
        var hubInstance = new StreamingHubHandlerTestHub();
        var fakeStreamingHubContext = new FakeStreamingServiceContext<byte[], byte[]>(hubType, hubMethod, MessagePackMagicOnionSerializerProvider.Default.Create(MethodType.DuplexStreaming, null), serviceProvider);

        // Act
        var handler = new StreamingHubHandler(hubType, hubMethod, new StreamingHubHandlerOptions(new MagicOnionOptions()), serviceProvider);
        var ctx = new StreamingHubContext()
        {
            HubInstance = hubInstance,
            ServiceContext = fakeStreamingHubContext,
            Request = MessagePackSerializer.Serialize(new DynamicArgumentTuple<int, string, bool>(12345, "テスト", true)),
        };
        await handler.MethodBody.Invoke(ctx);

        // Assert
        hubInstance.Results.Should().Contain(nameof(StreamingHubHandlerTestHub.Method_Parameter_Multiple_Returns_TaskOfInt32) + "(12345,テスト,True) called.");
        byte[] BuildMessage()
        {
            // [MessageId, MethodId, {Int32:12345}]
            var buffer = new ArrayBufferWriter<byte>();
            var writer = new MessagePackWriter(buffer);
            writer.WriteArrayHeader(3);
            writer.Write(ctx.MessageId);
            writer.Write(ctx.MethodId);
            MessagePackSerializer.Serialize(ref writer, 12345);
            writer.Flush();
            return buffer.WrittenMemory.ToArray();
        }
        fakeStreamingHubContext.Responses[0].Should().Equal(BuildMessage());
    }

    [Fact]
    public async Task CallRepeated_Parameter_Multiple_Returns_TaskOfInt32()
    {
        // Arrange
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();
        var hubType = typeof(StreamingHubHandlerTestHub);
        var hubMethod = hubType.GetMethod(nameof(StreamingHubHandlerTestHub.Method_Parameter_Multiple_Returns_TaskOfInt32))!;
        var hubInstance = new StreamingHubHandlerTestHub();
        var fakeStreamingHubContext = new FakeStreamingServiceContext<byte[], byte[]>(hubType, hubMethod, MessagePackMagicOnionSerializerProvider.Default.Create(MethodType.DuplexStreaming, null), serviceProvider);

        // Act
        var handler = new StreamingHubHandler(hubType, hubMethod, new StreamingHubHandlerOptions(new MagicOnionOptions()), serviceProvider);
        for (var i = 0; i < 3; i++)
        {
            var ctx = new StreamingHubContext()
            {
                MessageId = i * 1000,
                HubInstance = hubInstance,
                ServiceContext = fakeStreamingHubContext,
                Request = MessagePackSerializer.Serialize(new DynamicArgumentTuple<int, string, bool>(i, $"テスト{i}", i % 2 == 0)),
            };
            await handler.MethodBody.Invoke(ctx);
        }

        // Assert
        hubInstance.Results.Should().Contain(nameof(StreamingHubHandlerTestHub.Method_Parameter_Multiple_Returns_TaskOfInt32) + "(0,テスト0,True) called.");
        hubInstance.Results.Should().Contain(nameof(StreamingHubHandlerTestHub.Method_Parameter_Multiple_Returns_TaskOfInt32) + "(1,テスト1,False) called.");
        hubInstance.Results.Should().Contain(nameof(StreamingHubHandlerTestHub.Method_Parameter_Multiple_Returns_TaskOfInt32) + "(2,テスト2,True) called.");

        byte[] BuildMessage(int messageId, int retVal)
        {
            // [MessageId, MethodId, {Int32:RetVal}]
            var buffer = new ArrayBufferWriter<byte>();
            var writer = new MessagePackWriter(buffer);
            writer.WriteArrayHeader(3);
            writer.Write(messageId);
            writer.Write(0 /* MethodId - Fixed */);
            MessagePackSerializer.Serialize(ref writer, retVal);
            writer.Flush();
            return buffer.WrittenMemory.ToArray();
        }
        fakeStreamingHubContext.Responses[0].Should().Equal(BuildMessage(0, 0));
        fakeStreamingHubContext.Responses[1].Should().Equal(BuildMessage(1000, 1));
        fakeStreamingHubContext.Responses[2].Should().Equal(BuildMessage(2000, 2));
    }

    [Fact]
    public async Task UseCustomMessageSerializer()
    {
        // Arrange
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();
        var hubType = typeof(StreamingHubHandlerTestHub);
        var hubMethod = hubType.GetMethod(nameof(StreamingHubHandlerTestHub.Method_Parameter_Multiple_Returns_TaskOfInt32))!;
        var hubInstance = new StreamingHubHandlerTestHub();
        var fakeStreamingHubContext = new FakeStreamingServiceContext<byte[], byte[]>(hubType, hubMethod, XorMessagePackMagicOnionSerializerProvider.Instance.Create(MethodType.DuplexStreaming, null), serviceProvider);
        var bufferWriter = new ArrayBufferWriter<byte>();
        var serializer = XorMessagePackMagicOnionSerializerProvider.Instance.Create(MethodType.DuplexStreaming, null);
        serializer.Serialize(bufferWriter, new DynamicArgumentTuple<int, string, bool>(12345, "テスト", true));

        // Act
        var handler = new StreamingHubHandler(hubType, hubMethod, new StreamingHubHandlerOptions(new MagicOnionOptions()
        {
            MessageSerializer = XorMessagePackMagicOnionSerializerProvider.Instance,
        }), serviceProvider);
        var ctx = new StreamingHubContext()
        {
            HubInstance = hubInstance,
            ServiceContext = fakeStreamingHubContext,
            Request = bufferWriter.WrittenMemory.ToArray(),
        };
        await handler.MethodBody.Invoke(ctx);

        // Assert
        hubInstance.Results.Should().Contain(nameof(StreamingHubHandlerTestHub.Method_Parameter_Multiple_Returns_TaskOfInt32) + "(12345,テスト,True) called.");
        byte[] BuildMessage()
        {
            // [MessageId, MethodId, {Xor:Int32:12345}]
            var buffer = new ArrayBufferWriter<byte>();
            var writer = new MessagePackWriter(buffer);
            writer.WriteArrayHeader(3);
            writer.Write(ctx.MessageId);
            writer.Write(ctx.MethodId);
            writer.Flush();
            serializer.Serialize(buffer, 12345);
            return buffer.WrittenMemory.ToArray();
        }
        fakeStreamingHubContext.Responses[0].Should().Equal(BuildMessage());
    }


    interface IStreamingHubHandlerTestHubReceiver
    {
    }

    interface IStreamingHubHandlerTestHub : IStreamingHub<IStreamingHubHandlerTestHub, IStreamingHubHandlerTestHubReceiver>
    {
        Task Method_Parameterless_Returns_Task();
        Task<int> Method_Parameterless_Returns_TaskOfInt32();

        Task Method_Parameter_Single_Returns_Task(int arg0);
        Task Method_Parameter_Multiple_Returns_Task(int arg0, string arg1, bool arg2);
        Task<int> Method_Parameter_Multiple_Returns_TaskOfInt32(int arg0, string arg1, bool arg2);

        ValueTask Method_Parameterless_Returns_ValueTask();
        ValueTask<int> Method_Parameterless_Returns_ValueTaskOfInt32();

    }
    class StreamingHubHandlerTestHub : IStreamingHubHandlerTestHub
    {
        public List<string> Results { get; } = new List<string>();

        public IStreamingHubHandlerTestHub FireAndForget() => throw new NotImplementedException();
        public Task DisposeAsync() => throw new NotImplementedException();
        public Task WaitForDisconnect() => throw new NotImplementedException();

        public Task Method_Parameterless_Returns_Task()
        {
            Results.Add(nameof(Method_Parameterless_Returns_Task) + " called.");
            return Task.CompletedTask;
        }

        public Task<int> Method_Parameterless_Returns_TaskOfInt32()
        {
            Results.Add(nameof(Method_Parameterless_Returns_TaskOfInt32) + " called.");
            return Task.FromResult(12345);
        }

        public Task Method_Parameter_Single_Returns_Task(int arg0)
        {
            Results.Add(nameof(Method_Parameter_Single_Returns_Task) + $"({arg0}) called.");
            return Task.CompletedTask;
        }

        public Task Method_Parameter_Multiple_Returns_Task(int arg0, string arg1, bool arg2)
        {
            Results.Add(nameof(Method_Parameter_Multiple_Returns_Task) + $"({arg0},{arg1},{arg2}) called.");
            return Task.CompletedTask;
        }

        public Task<int> Method_Parameter_Multiple_Returns_TaskOfInt32(int arg0, string arg1, bool arg2)
        {
            Results.Add(nameof(Method_Parameter_Multiple_Returns_TaskOfInt32) + $"({arg0},{arg1},{arg2}) called.");
            return Task.FromResult(arg0);
        }

        public ValueTask Method_Parameterless_Returns_ValueTask()
        {
            Results.Add(nameof(Method_Parameterless_Returns_ValueTask) + " called.");
            return ValueTask.CompletedTask;
        }

        public ValueTask<int> Method_Parameterless_Returns_ValueTaskOfInt32()
        {
            Results.Add(nameof(Method_Parameterless_Returns_ValueTaskOfInt32) + " called.");
            return ValueTask.FromResult(12345);
        }

    }
}
