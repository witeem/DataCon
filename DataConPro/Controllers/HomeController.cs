using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DataConPro.Models;
using DataConApplication;
using DataConPro.ViewModels;
using DataConCore;
using DataConCore.Handels;
using Newtonsoft.Json;

namespace DataConPro.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IWxUserAppService _userAppService;
    private readonly IConfiguration _configuration;
    private static int serverIndex = 0;

    public HomeController(ILogger<HomeController> logger, IWxUserAppService userAppService, IConfiguration configuration)
    {
        _logger = logger;
        _userAppService = userAppService;
        _configuration = configuration;
    }

    public async Task<IActionResult> Index()
    {

        /*
         * 直接通过业务层获取数据
         * var result = await _userAppService.AppServiceTest();
        List<UserViewModel> models = new List<UserViewModel>();
        foreach (var item in result)
        {
            models.Add(new UserViewModel().MapFrom(item));
        }*/


        #region Nginx

        /*
         * 通过请求Nginx， Nginx反向代理转发请求 DataConApi服务获取数据
        var result = ApiHandel.InvokeApi("http://localhost:8088/api/WxUser/Getuserlist", HttpMethod.Get);
        */
        #endregion

        #region Consul 服务注册与发现
        var urls = ConsulHandel.GetConsulServers("DataConApi", "api/WxUser/Getuserlist");

        /*
         * 轮询策略
        var result = ApiHandel.InvokeApi(urls[serverIndex++ % urls.Count], HttpMethod.Get);*/

        
        // 平均策略
        Random random = new Random(serverIndex++);
        var result = ApiHandel.InvokeApi(urls[random.Next(urls.Count)], HttpMethod.Get);
        

        /*
         * 
         
         */

        
        #endregion

        List<UserViewModel> models = JsonConvert.DeserializeObject<List<UserViewModel>>(result);
        ViewData["DataList"] = models;
        ViewData["Da"] = models.FirstOrDefault();

        // 建表
        // var sqlDb = SqlSugarHandel.GetMySqlDb();
        // sqlDb.CodeFirst.InitTables<UserEntity>(); 
        return View();
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
