using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DncAdmin.Api.Dtos.Rbac
{
    /// <summary>
    /// 用户授予角色模型
    /// </summary>
    public class AssignRoleInputModel
    {
        public Guid UserId { get; set; }
    }
}
