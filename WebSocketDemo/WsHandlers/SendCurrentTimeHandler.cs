using System.Net.WebSockets;
using System.Text;

namespace WebSocketDemo.WsHandlers;

public class SendCurrentTimeHandler
{
    public async Task Handle(WebSocket webSocket, CancellationToken cancellationToken)
    {
        var buffer = new byte[1024 * 4];
        while (webSocket.State == WebSocketState.Open)
        {
            var currentTime = DateTime.Now.ToString("HH:mm:ss");
            var bytes = Encoding.UTF8.GetBytes(currentTime);
            var segment = new ArraySegment<byte>(bytes);

            await webSocket.SendAsync(segment, WebSocketMessageType.Text, endOfMessage: true, CancellationToken.None);
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}
