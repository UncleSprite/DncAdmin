using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DncAdmin.Api.ViewModels.Rbac
{
    /// <summary>
    /// 用户角色授权信息
    /// </summary>
    public class UserRoleAssignViewModel
    {
        /// <summary>
        /// 已授权的角色
        /// </summary>
        public IEnumerable<Guid> AssignedRoles { get; set; }

        /// <summary>
        /// 所有角色
        /// </summary>
        public IEnumerable<RoleBaseViewModel> Roles { get; set; }
    }
}
