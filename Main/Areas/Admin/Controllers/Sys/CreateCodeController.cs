using Microsoft.AspNetCore.Mvc;

namespace HzyAdmin.Areas.Admin.Controllers.Sys
{
    //
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Hosting;
    using Aop;
    using Common;
    using Entitys.SysClass;
    using Logic.SysClass;
    using DbFrame;
    using System.Collections;


    /// <summary>
    /// 创建代码
    /// </summary>
    public class CreateCodeController : BaseController
    {
        private IHostingEnvironment _IHostingEnvironment = null;
        private string _WebRootPath = string.Empty;
        public CreateCodeController(IHostingEnvironment IHostingEnvironment)
        {
            this._IHostingEnvironment = IHostingEnvironment;
            _WebRootPath = this._IHostingEnvironment.WebRootPath;
        }

        public Sys_CreateCodeLogic _Logic = new Sys_CreateCodeLogic();

        protected override void Init()
        {
            this.MenuID = "Z-160";
        }

        public override IActionResult Index()
        {
            ViewData["DbSetCode"] = _Logic.CreateDbSetCode();
            ViewData["Path"] = (_WebRootPath + "\\Content\\CreateFile\\").Replace("\\", "\\\\");
            return base.Index();
        }

        /// <summary>
        /// 获取数据库中所有的表和字段
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetZTreeAllTable()
        {
            return this.Success(new { status = 1, value = _Logic.GetZTreeAllTable() });
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Save(IFormCollection fc)
        {
            var Type = fc["ClassType"];
            var Url = (fc["Url"].ToStr() == null ? _WebRootPath + "\\Content\\CreateFile\\" : fc["Url"].ToStr());
            var Str = fc["Str"];
            var Table = fc["Table"];
            var isall = fc["isall"].ToBool();
            var Template = _WebRootPath + "\\Content\\Template\\";

            _Logic.Save(Type, Url, Template, Str, isall, Table);
            return this.Success();
        }

        


    }
}