using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DncAdmin.Api.Dtos.Rbac
{
    /// <summary>
    /// 编辑角色模型
    /// </summary>
    public class EditRoleInputModel
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
    }
}
