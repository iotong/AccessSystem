using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HzyAdmin.Areas.Admin.Controllers.Base
{
    //
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Aop;
    using DbFrame;
    using Common;
    using Entitys;
    using System.Collections;
    using System.Text;
    using Logic.SysClass;
    using Entitys.SysClass;
    using Logic;

    /// <summary>
    /// 会员管理
    /// </summary>
    public class MemberController : BaseController
    {
        private IHostingEnvironment _IHostingEnvironment = null;
        private string _WebRootPath = string.Empty;
        public MemberController(IHostingEnvironment IHostingEnvironment)
        {
            this._IHostingEnvironment = IHostingEnvironment;
            _WebRootPath = this._IHostingEnvironment.WebRootPath;
        }

        protected override void Init()
        {
            this.MenuID = "A-100";
            this.PrintTitle = "我是一个 打印标题！";
        }

        MemberLogic _Logic = new MemberLogic();

        #region  查询数据列表
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
            query.Add("Member_Sex", "131231");
            return _Logic.GetDataSource(query, page, rows);
        }
        #endregion  查询数据列表

        #region  基本操作，增删改查
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        [HttpPost, AopCheckEntity]
        public IActionResult Save(Member model, IFormFile Member_Photo_Files, IFormFile Member_FilePath_Fiels, string UserIDList)
        {
            //接收图片
            if (Member_Photo_Files != null)
            {
                model.Member_Photo = this.HandleUpFile(Member_Photo_Files, new string[] { ".jpg", ".gif", ".png" }, _WebRootPath);
            }

            //接收文件
            if (Member_FilePath_Fiels != null)
            {
                model.Member_FilePath = this.HandleUpFile(Member_FilePath_Fiels, null, _WebRootPath);
            }

            //判断是否有文件上传上来
            //if (Member_Photo_Files.Count > 0)
            //{
            //    foreach (var item in Member_Photo_Files)
            //    {
            //        this.HandleUpFile(item, new string[] { ".jpg", ".gif", ".png" }, _WebRootPath, null, (_Path) =>
            //         {
            //             model.Member_Photo = _Path;
            //         });
            //    }
            //}

            this.KeyID = _Logic.Save(model);
            return this.Success(new { status = 1, ID = this.KeyID });
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