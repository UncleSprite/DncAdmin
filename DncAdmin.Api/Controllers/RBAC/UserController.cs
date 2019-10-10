using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DncAdmin.Api.Controllers.RBAC
{
    /// <summary>
    /// 用户
    /// </summary>
    [Route("api/rabc/[controller]")]
    [Authorize]
    public class UserController : BaseController
    {
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> Lists()
        {
            return Ok(UserIdentity.UserId);
        }
    }
}
