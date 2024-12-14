using Microsoft.AspNetCore.WebSockets;
using System.Net.WebSockets;
using System.Text;
using WebSocketDemo.WsHandlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<SendCurrentTimeHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseWebSockets();
app.UseMiddleware<WebSocketMiddleware>();

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/ws")
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            await SendCurrentTime(webSocket);
        }
        else
        {
            context.Response.StatusCode = 400;
        }
    }
    else
    {
        await next();
    }
});

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();

static async Task SendCurrentTime(WebSocket webSocket)
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