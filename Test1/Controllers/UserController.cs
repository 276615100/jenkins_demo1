using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test1.Controllers
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// Get获取端口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Get()
        {

            string str = "Get:" + Request.HttpContext.Connection.LocalIpAddress.MapToIPv4().ToString() + ":" + Request.HttpContext.Connection.LocalPort;
            return str;
        }


        [HttpGet]
        public async Task<List<User>> GetUser()
        {
            var list = new List<User>() {
                new User("张三",10),
                new User("李四",100),
            };
            return list;
            //return await Task.FromResult(new List<User>() { });
        }
    }
}
