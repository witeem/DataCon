using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataConApplication;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DataConApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WxUserController : ControllerBase
    {
        private readonly IWxUserAppService _wxUserAppService;
        public WxUserController(IWxUserAppService wxUserAppService)
        {
            _wxUserAppService = wxUserAppService;
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserList")]
        public async Task<IActionResult> GetUserListAsync()
        {
            var list = await _wxUserAppService.AppServiceTest();
            return Ok(list);
        }
    }
}

