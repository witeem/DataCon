using DataCon.IApplication;
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

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        [HttpGet("MyTest")]
        public async Task<IActionResult> MyTestAsync()
        {
            var result = await _wxUserAppService.MyTestAsync();
            return Ok(result);
        }

        /// <summary>
        /// MQ生产者
        /// </summary>
        /// <returns></returns>
        [HttpGet("SendMq")]
        public async Task<IActionResult> SendMqAsync()
        {
            var result = _wxUserAppService.SendMqAsync();
            await Task.CompletedTask;
            return Ok(result);
        }

        /// <summary>
        /// MQ 消费者
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExpendMq")]
        public async Task<IActionResult> ExpendMqAsync()
        {
            var result = _wxUserAppService.ExpendMqAsync();
            await Task.CompletedTask;
            return Ok(result);
        }

        /// <summary>
        /// MQ 消费者
        /// </summary>
        /// <returns></returns>
        [HttpGet("CloseExpendMq")]
        public async Task<IActionResult> CloseExpendMqAsync()
        {
            var result = _wxUserAppService.CloseExpendMqAsync();
            await Task.CompletedTask;
            return Ok(result);
        }
    }
}

