using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DataConPro.Models;
using DataConApplication;
using DataConPro.ViewModels;
using DataConCore;

namespace DataConPro.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IWxUserAppService _userAppService;

    public HomeController(ILogger<HomeController> logger, IWxUserAppService userAppService)
    {
        _logger = logger;
        _userAppService = userAppService;
    }

    public async Task<IActionResult> Index()
    {
   
        var result = await _userAppService.AppServiceTest();
        UserViewModel entity = new UserViewModel(){ Name = result };
        // 建表
        // var sqlDb = SqlSugarHandel.GetMySqlDb();
        // sqlDb.CodeFirst.InitTables<UserEntity>(); 
        return View(entity);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
