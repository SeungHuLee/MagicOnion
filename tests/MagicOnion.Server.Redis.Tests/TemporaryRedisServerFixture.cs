using System.Net.Sockets;
using System.Net;
using Testcontainers.Redis;

namespace MagicOnion.Server.Redis.Tests;

public sealed class TemporaryRedisServerFixture : IAsyncLifetime
{
    const ushort redisPort = 6379;

    readonly RedisContainer container = new RedisBuilder().Build();

    public string GetConnectionString()
    {
        return $"{container.Hostname}:{container.GetMappedPublicPort(redisPort)}";
    }

    Task IAsyncLifetime.InitializeAsync()
    {
        return container.StartAsync();
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        return container.DisposeAsync().AsTask();
    }

    static int GetAvailableListenerPort()
    {
        // WORKAROUND: `assignRandomHostPort` on Windows, a port within the range of `excludedport` setting may be selected.
        //             We get an available port from the operating system.
        using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        {
            socket.Bind(new IPEndPoint(IPAddress.Loopback, 0));
            return ((IPEndPoint)socket.LocalEndPoint!).Port;
        }
    }
}
