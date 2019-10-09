using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DncAdmin.Api.Auth
{
    /// <summary>
    /// Jwt 配置项
    /// </summary>
    public class JwtAuthenticationSettings
    {
        /// <summary>
        /// Issuer
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Audience
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// 过期分钟数
        /// </summary>
        public int ExpMinutes { get; set; }
    }
}
