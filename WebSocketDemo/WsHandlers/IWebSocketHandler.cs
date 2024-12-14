using System.Net.WebSockets;

namespace WebSocketDemo.WsHandlers;
public interface IWebSocketHandler
{
    Task Handle(WebSocket webSocket, CancellationToken cancellationToken);
}