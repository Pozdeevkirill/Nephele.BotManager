using Microsoft.AspNetCore.Mvc;
using Nephele.BotManager.Models.HttpModels.Requests;

namespace Nephele.BotManager.API.Controllers;

[ApiController]
[Route("api/bot-info/")]
public class BotInfoController : ControllerBase
{
    [HttpPost]
    [Route("create")]
    public ActionResult CreateBot(CreateBotRequest request)
    {
        return Ok();
    }

    [HttpDelete]
    [Route("delete")]
    public ActionResult DeleteBot([FromQuery]string botName)
    {
        return Ok();
    }

    [HttpDelete]
    [Route("delete")]
    public ActionResult DeleteBot([FromQuery]Guid id)
    {
        return Ok();
    }

    [HttpGet]
    [Route("get")]
    public ActionResult GetBot([FromQuery]string botName)
    {
        return Ok();
    }

    [HttpGet]
    [Route("get")]
    public ActionResult GetBot([FromQuery]Guid id)
    {
        return Ok();
    }
}