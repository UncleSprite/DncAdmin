using DncAdmin.Api.Dtos.Rbac;
using DncAdmin.Api.Infrastructure;
using DncAdmin.Api.Models.Rbac;
using DncAdmin.Api.ViewModels;
using DncAdmin.Api.ViewModels.Rbac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DncAdmin.Api.Controllers.RBAC
{
    /// <summary>
    /// 角色
    /// </summary>
    [Route("api/rbac/[controller]")]
    [Authorize]
    public class RoleController : BaseController
    {
        private readonly DncAdminContext _context;
        public RoleController(DncAdminContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 角色列表
        /// </summary>
        /// <param name="keyword">搜索关键字</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        [HttpGet]
        [Route("list")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<RoleViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> List(string keyword, [FromQuery]int pageIndex = 1, [FromQuery]int pageSize = 20)
        {
            var root = _context.Roles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
                root = root.Where(t => t.Name.Contains(keyword));

            var count = await root.CountAsync();
            var data = await root
                .Select(r => new RoleViewModel
                {
                    Id = r.Id,
                    Description = r.Description,
                    CreateOn = r.CreateOn,
                    IsBuiltin = r.IsBuiltin,
                    IsSuperAdministrator = r.IsSuperAdministrator,
                    Name = r.Name,
                    status = r.status
                })
                .OrderByDescending(t => t.CreateOn).Skip((pageIndex - 1) * pageSize).Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return Ok(new PaginatedItemsViewModel<RoleViewModel>(pageIndex, pageSize, count, data));
        }

        /// <summary>
        /// 角色详情
        /// </summary>
        /// <param name="id">角色关键字</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RoleViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
                return NotFound();

            var data = new RoleViewModel
            {
                Id = role.Id,
                Description = role.Description,
                CreateOn = role.CreateOn,
                IsBuiltin = role.IsBuiltin,
                IsSuperAdministrator = role.IsSuperAdministrator,
                Name = role.Name,
                status = role.status
            };
            return Ok(data);
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Post([FromBody]CreateRoleInputModel inputModel)
        {
            if (await _context.Roles.AnyAsync(t => t.Name == inputModel.Name))
                return BadRequest("账号已存在");

            var role = new DncRole
            {
                Name = inputModel.Name,
                Description = inputModel.Description,
                status = inputModel.status,
            };

            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Put([FromBody]EditRoleInputModel inputModel)
        {
            var tempRole = await _context.Roles.FindAsync(inputModel.Id);
            if (tempRole == null)
                return BadRequest("角色不存在");

            tempRole.Name = inputModel.Name;
            tempRole.Description = inputModel.Description;
            tempRole.status = inputModel.status;

            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var tempRole = await _context.Roles.FindAsync(id);
            if (tempRole == null)
                return BadRequest("角色不存在");

            if (tempRole.IsBuiltin)
                return BadRequest("内置角色不能删除");

            _context.Roles.Remove(tempRole);
            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// 获取用户授权信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Route("user_role_assign")]
        [ProducesResponseType(typeof(UserRoleAssignViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UserRoleAssign(Guid id)
        {
            var user = await _context.Users.Include(t => t.UserRoles).SingleOrDefaultAsync(t => t.Id == id);
            if (user == null)
                return NotFound();

            var roles = await _context.Roles.Where(t => t.status == 1)
                .Select(t => new RoleBaseViewModel
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .AsNoTracking()
                .ToListAsync();

            var data = new UserRoleAssignViewModel
            {
                AssignedRoles = user.UserRoles.Select(t => t.RoleId).ToList(),
                Roles = roles
            };

            return Ok(data);
        }
    }
}
