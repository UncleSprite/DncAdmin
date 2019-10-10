using DncAdmin.Api.Auth;
using DncAdmin.Api.DomainModels;
using DncAdmin.Api.Infrastructure;
using DncAdmin.Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DncAdmin.Api.Controllers
{
    /// <summary>
    /// 授权
    /// </summary>
    [Route("api/[controller]")]
    public class OauthController : ControllerBase
    {
        private readonly DncAdminContext _context;
        private readonly JwtAuthenticationSettings _settings;
        public OauthController(IOptions<JwtAuthenticationSettings> options, DncAdminContext context)
        {
            _settings = options.Value;
            _context = context;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <remarks>
        /// post /
        /// {
        ///   account: 'account',
        ///   password: 'password'
        /// }
        /// </remarks>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(LoginResultViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login([FromBody]LoginInfo loginInfo)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Account == loginInfo.Account.Trim());
            if (user == null)
                return BadRequest("账号不存在");

            if (user.Password != loginInfo.Password)
                return BadRequest("账号或密码错误");

            if (user.Status != 0)
                return BadRequest("账号状态异常");

            var data = new LoginResultViewModel
            {
                Avatar = user.Avatar ?? string.Empty,
                Id = user.Id,
                NiName = user.NiName ?? string.Empty,
            };

            var exp = $"{new DateTimeOffset(DateTime.Now.AddMinutes(_settings.ExpMinutes)).ToUnixTimeSeconds()}";
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                new Claim("niName", user.NiName?? string.Empty),
                new Claim("avatar", user.Avatar?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Iss, _settings.Issuer),
                new Claim(JwtRegisteredClaimNames.Aud, _settings.Audience),
                new Claim(JwtRegisteredClaimNames.Exp, exp),
            };

            // 生成token
            var token = JwtBearerAuthenticationExtension.GetJwtAccessToken(_settings, claims);

            data.Token = token;
            return Ok(data);
        }       
    }
}
