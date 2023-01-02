using LC_Backend.Context;
using LC_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static LC_Backend.websocketservice;

namespace LC_Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WebSocketController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    public WebSocketController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    public async Task<int> CreateChatEndpoint()
    {
        Dialog dialog = new Dialog();
        WebSocketService.Dialogs.Add(dialog);

        WebSocketService.Wssv.AddWebSocketService<RecieveChat1>($"/Customer-{WebSocketService.Dialogs.Count}");
        WebSocketService.Wssv.AddWebSocketService<RecieveChat2>($"/Worker-{WebSocketService.Dialogs.Count}");
        return WebSocketService.Dialogs.Count;
    }
}