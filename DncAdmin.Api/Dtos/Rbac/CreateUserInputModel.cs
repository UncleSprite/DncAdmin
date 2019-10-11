using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DncAdmin.Api.Dtos.Rbac
{
    /// <summary>
    /// 添加用户
    /// </summary>
    public class CreateUserInputModel
    {
        /// <summary>
        /// 账户
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NiName { get; set; }     

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }       
    }
}
