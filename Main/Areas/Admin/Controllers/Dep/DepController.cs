using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Main.Areas.Access.Controllers //请在此设置您得命名空间
{
    //
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Aop;
    using DbFrame;
    using DbFrame.Class;
    using Common;
    using Entitys;
    using System.Collections;
    using System.Text;
    using Logic.SysClass;
    using Entitys.SysClass;
    using Logic;
    using HzyAdmin.Areas.Admin.Controllers;

    /// <summary>
    /// Dep管理
    /// </summary>
    public class DepController : BaseController
    {

        public DepLogic _Logic = new DepLogic();

        protected override void Init()
        {
            this.MenuID = "A-200";
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
        /// 新增
        /// </summary>
        /// <returns></returns>
        [HttpPost, AopCheckEntity]
        public IActionResult Save(Dep model)
        {
            model.Oper = _Account.UserLoginName;
            
            this.KeyID = _Logic.Save(model);
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
        public IActionResult LoadForm(Guid? ID)
        {
            return this.Success(_Logic.LoadForm(ID.ToGuid()));
        }

        #endregion  基本操作，增删改查



    }
}