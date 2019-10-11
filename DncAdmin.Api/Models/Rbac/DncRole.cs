using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DncAdmin.Api.Models.Rbac
{
    /// <summary>
    /// 角色实体
    /// </summary>
    public class DncRole
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public bool IsSuperAdministrator { get; set; }

        /// <summary>
        /// 是否系统内置角色
        /// </summary>
        public bool IsBuiltin { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateOn { get; set; }

        /// <summary>
        /// 用户角色映射
        /// </summary>
        public ICollection<DncUserRoleMapping> UserRoles { get; set; }
    }
}
