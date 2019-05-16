using System;
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
    /// 角色功能
    /// </summary>
    public class RoleFunctionController : BaseController
    {

        public Sys_RoleMenuFunctionLogic _Logic = new Sys_RoleMenuFunctionLogic();

        protected override void Init()
        {
            this.MenuID = "Z-140";
        }

        public override IActionResult Index()
        {
            var _List = db
                .Query<Sys_Role>()
                .OrderBy(item => new { item.Role_Num })
                .ToList<Sys_Role>();
            return View(_List);
        }
        #region  基本操作，增删改查

        /// <summary>
        /// 获取角色菜单功能
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetRoleMenuFunctionTree(string RoleId)
        {
            return this.Success(new
            {
                status = 1,
                value = _Logic.GetRoleMenuFunctionZTree(RoleId.ToGuid())
            });
        }

        /// <summary>
        /// 保存角色功能
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Save(string Sys_RoleMenuFunction_List, string RoleId)
        {
            _Logic.SaveFunction(Sys_RoleMenuFunction_List, RoleId.ToGuid());
            return this.Success();
        }
        #endregion 基本操作，增删改查


    }
}