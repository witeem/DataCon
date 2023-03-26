namespace DataConMenu.Controllers;

using DataCon.IApplication;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class MenuController : ControllerBase
{
    private readonly IMenuAppService _menuAppService;
    public MenuController(IMenuAppService menuAppService)
    {
        _menuAppService = menuAppService;
    }

    /// <summary>
    /// MQ 消费者
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetMenus")]
    public async Task<IActionResult> GetMenusAsync()
    {
        var result = await _menuAppService.GetMenus(default);
        return Ok(result);
    }
}
