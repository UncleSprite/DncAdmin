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
    /// 用户
    /// </summary>
    [Route("api/rbac/[controller]")]
    [Authorize]
    public class UserController : BaseController
    {
        private readonly DncAdminContext _context;
        public UserController(DncAdminContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="keyword">搜索关键字</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        [HttpGet]
        [Route("list")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<UserViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Lists(string keyword, [FromQuery]int pageIndex = 1, [FromQuery]int pageSize = 20)
        {
            var root = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
                root = root.Where(t => t.Account.Contains(keyword) || t.NiName.Contains(keyword));

            var count = await root.CountAsync();
            var data = await root
                .Select(u => new UserViewModel
                {
                    Account = u.Account,
                    Avatar = u.Avatar,
                    CreateOn = u.CreateOn,
                    Id = u.Id,
                    NiName = u.NiName,
                    Remark = u.Remark,
                    Status = u.Status
                })
                .OrderByDescending(t => t.CreateOn).Skip((pageIndex - 1) * pageSize).Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return Ok(new PaginatedItemsViewModel<UserViewModel>(pageIndex, pageSize, count, data));
        }

        /// <summary>
        /// 用户
        /// </summary>
        /// <param name="id">用户关键字</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            var data = new UserViewModel
            {
                Id = user.Id,
                Account = user.Account,
                Avatar = user.Avatar,
                CreateOn = user.CreateOn,
                NiName = user.NiName,
                Remark = user.Remark,
                Status = user.Status
            };
            return Ok(data);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Post([FromBody]CreateUserInputModel inputModel)
        {
            if (await _context.Users.AnyAsync(t => t.Account == inputModel.Account))
                return BadRequest("账号已存在");

            var dncUser = new DncUser
            {
                Account = inputModel.Account,
                Password = inputModel.Password,
                NiName = inputModel.NiName,
                Status = inputModel.Status,
                Remark = inputModel.Remark
            };

            await _context.Users.AddAsync(dncUser);
            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Put([FromBody]EditUserInputModel inputModel)
        {
            var tempUser = await _context.Users.FindAsync(inputModel.Id);
            if (tempUser == null)
                return BadRequest("账户不存在");

            tempUser.NiName = inputModel.NiName;
            tempUser.Status = inputModel.Status;
            tempUser.Remark = inputModel.Remark;

            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// 用户授予角色
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("assign")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AssignRoles([FromBody]AssignRoleInputModel inputModel)
        {
            var tempUser = await _context.Users.Include(t => t.UserRoles).SingleOrDefaultAsync(u => u.Id == inputModel.UserId);
            if (tempUser == null)
                return BadRequest("账户不存在");



            return Ok();
        }
    }
}
