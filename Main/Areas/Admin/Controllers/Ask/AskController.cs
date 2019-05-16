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
    using Microsoft.AspNetCore.Mvc.Razor.Internal;
    using System.Data;

    /// <summary>
    /// Ask管理
    /// </summary>
    public class AskController : BaseController
    {

        public AskLogic _Logic = new AskLogic();
        public EmpLogic _empLogic = new EmpLogic();
        public Emp emp = new Emp();

        protected override void Init()
        {
            this.MenuID = "C-200";
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
            
            query.Add("Oper",this.LoginName);
          
              
            return _Logic.GetDataSource(query, page, rows);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        [HttpPost, AopCheckEntity]
        public IActionResult Save(Ask model)
        {
            DataTable dt = _empLogic.getempInfo(this.LoginName);
            if (dt.Rows.Count > 0)
            {
                model.Name = dt.Rows[0]["name"].ToStr();
                model.Depart = dt.Rows[0]["department"].ToStr();
                model.MangeDep = dt.Rows[0]["managementdept"].ToStr();

            }
           
            model.Oper = this.LoginName;
            model.OperTime = DateTime.Now;
            

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