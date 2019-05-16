using System;
using System.Collections.Generic;
using System.Linq;
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
    /// 菜单功能
    /// </summary>
    public class MenuFunctionController : BaseController
    {

        public Sys_MenuLogic _Logic = new Sys_MenuLogic();

        protected override void Init()
        {
            this.MenuID = "Z-130";
        }

        public override IActionResult Info()
        {
            var _List = db
                .Query<Sys_Function>()
                .OrderBy(item => new { item.Function_Num })
                .ToList<Sys_Function>();
            return View(_List);
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
        public IActionResult Save(Sys_Menu model, string _Sys_Function_List)
        {
            string Function_List;
            if (_Sys_Function_List == null)
            {
                Function_List = "[{Function_ID:'B6FD5425-504A-46A9-993B-2F8DC9158EB8'},{Function_ID:'C9518758-B2E1-4F51-B517-5282E273889C'},{Function_ID:'F27ECB0A-197D-47B1-B243-59A8C71302BF'},{Function_ID:'383E7EE2-7690-46AC-9230-65155C84AA30'},{Function_ID:'9C388461-2704-4C5E-A729-72C17E9018E1'},{Function_ID:'BFFEFB1C-8988-4DDF-B4AC-81C2B30E80CD'},{Function_ID:'2401F4D0-60B0-4E2E-903F-84C603373572'},{Function_ID:'E7EF2A05-8317-41C3-B588-99519FE88BF9'}]";
            }
            else
            {
                Function_List = _Sys_Function_List;


            }


            this.KeyID = _Logic.Save(model, Function_List);
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

        /// <summary>
        /// 获取菜单和功能树
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetMenuAndFunctionTree()
        {
            return Success(new
            {
                status = 1,
                value = _Logic.GetMenuZTree()
            });
        }


    }
}