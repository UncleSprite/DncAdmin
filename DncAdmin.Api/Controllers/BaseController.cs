using DncAdmin.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DncAdmin.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        public UserIdentity UserIdentity
        {
            get
            {
                var identity = new UserIdentity();
                identity.UserId = Guid.Parse(HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Jti) ?? Guid.Empty.ToString());
                
                return identity;
            }
        }
    }
}
