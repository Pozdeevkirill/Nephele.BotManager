using Microsoft.AspNetCore.Mvc;
using Nephele.BotManager.Models.HttpModels.Requests;
using Nephele.BotManager.Services.Interfaces;

namespace Nephele.BotManager.API.Controllers;

[ApiController]
[Route("api/bot/")]
public class BotController : ControllerBase
{
    private readonly IBotService _botService;

    public BotController(IBotService botService)
    {
        _botService = botService;
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartBot([FromBody] StartBotRequest request)
    {
        try
        {
            var botInstance = await _botService.StartBotAsync(request.Token, request.BotId);
            
            return Ok(new
            {
                BotId = botInstance.BotId,
                Username = botInstance.Username,
                Status = botInstance.Status,
                StartedAt = botInstance.StartedAt,
                Message = $"Бот @{botInstance.Username} запущен"
            });
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("stop/{botId}")]
    public async Task<IActionResult> StopBot(string botId)
    {
        var result = await _botService.StopBotAsync(botId);
        
        if (!result)
        {
            return NotFound(new { Error = $"Бот с ID {botId} не найден" });
        }
        
        return Ok(new { Message = $"Бот {botId} остановлен" });
    }

    [HttpPost("stop-all")]
    public async Task<IActionResult> StopAllBots()
    {
        var result = await _botService.StopAllBotsAsync();
        
        return Ok(new 
        { 
            Message = "Все боты остановлены",
            Success = result
        });
    }

    [HttpGet("status/{botId}")]
    public IActionResult GetBotStatus(string botId)
    {
        var botInstance = _botService.GetBot(botId);
        
        if (botInstance == null)
        {
            return NotFound(new { Error = $"Бот с ID {botId} не найден" });
        }
        
        return Ok(botInstance);
    }

    [HttpGet("list")]
    public IActionResult GetAllBots()
    {
        var bots = _botService.GetAllBots();
        
        return Ok(new
        {
            TotalCount = bots.Count(),
            Bots = bots.Select(b => new
            {
                b.BotId,
                b.Username,
                b.Status,
                b.StartedAt,
                RunningFor = b.StartedAt.HasValue 
                    ? DateTime.UtcNow - b.StartedAt.Value 
                    : TimeSpan.Zero
            })
        });
    }

    [HttpGet("statuses")]
    public IActionResult GetStatuses()
    {
        var statuses = _botService.GetBotStatuses();
        
        return Ok(new
        {
            ActiveBots = _botService.GetAllBots().Count(b => b.Status == "Running"),
            TotalBots = statuses.Count,
            Statuses = statuses
        });
    }
}