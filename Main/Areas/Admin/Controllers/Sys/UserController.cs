using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HzyAdmin.Areas.Admin.Controllers.Sys
{
    //
    using Aop;
    using Common;
    using Entitys.SysClass;
    using Logic.SysClass;
    using DbFrame;
    using System.Collections;

    /// <summary>
    /// 用户管理
    /// </summary>
    public class UserController : BaseController
    {
        public Sys_UserLogic _Logic = new Sys_UserLogic();

        protected override void Init()
        {
            this.MenuID = "Z-100";
        }

        #region  基本操作，增删改查

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [NonAction]
        public override PagingEntity GetPagingEntity(Hashtable query, int page = 1, int rows = 20)
        {
            //获取列表
            return _Logic.GetDataSource(query, page, rows);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        [HttpPost, AopCheckEntity]
        public IActionResult Save(Sys_User model, string Role_ID)
        {
            this.KeyID = _Logic.Save(model, new Sys_UserRole() { UserRole_RoleID = Role_ID.ToGuid() });
            return this.Success(new
            {
                status = 1,
                ID = this.KeyID
            });
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Delete(string Ids)
        {
            _Logic.Delete(Ids);
            return this.Success();
        }

        /// <summary>
        /// 查询根据ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult LoadForm(string ID)
        {
            return this.Success(_Logic.LoadForm(ID.ToGuid()));
        }
        #endregion  基本操作，增删改查




    }
}