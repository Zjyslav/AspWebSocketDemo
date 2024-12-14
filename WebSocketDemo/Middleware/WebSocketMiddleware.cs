using WebSocketDemo.WsHandlers;
using static System.Net.Mime.MediaTypeNames;

namespace WebSocketDemo.Middleware;

public class WebSocketMiddleware : IMiddleware
{
    private readonly IWebSocketHandler _handler;

    public WebSocketMiddleware(IWebSocketHandler handler)
    {
        _handler = handler;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Path == "/ws")
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                await _handler.Handle(webSocket, context.RequestAborted);
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        }
        else
        {
            await next(context);
        }
    }
}
