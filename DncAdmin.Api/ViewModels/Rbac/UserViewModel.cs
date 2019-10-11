using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DncAdmin.Api.ViewModels.Rbac
{
    /// <summary>
    /// 用户视图模型
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 账户
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NiName { get; set; }    

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateOn { get; set; }
    }
}
