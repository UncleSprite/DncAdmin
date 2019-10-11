using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DncAdmin.Api.Models.Rbac
{
    /// <summary>
    /// 菜单实体
    /// </summary>
    public class DncMenu
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 页面地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 页面别名
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 父Id
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 前端组件（.vuew）
        /// </summary>
        public string Component { get; set; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        public int IsHide { get; set; }

        /// <summary>
        /// 是否缓存
        /// </summary>
        public int IsCache { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateOn { get; set; }

        /// <summary>
        /// 上级菜单
        /// </summary>
        public DncMenu Parent { get; set; }

        /// <summary>
        /// 子菜单
        /// </summary>
        public ICollection<DncMenu> SubMenus { get; set; }
    }
}
