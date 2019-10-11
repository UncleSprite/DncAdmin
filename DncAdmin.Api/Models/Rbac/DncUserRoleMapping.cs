using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DncAdmin.Api.Models.Rbac
{
    /// <summary>
    /// 用户-角色映射实体
    /// </summary>
    public class DncUserRoleMapping
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public DncUser DncUser { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public DncRole DncRole { get; set; }
    }
}
