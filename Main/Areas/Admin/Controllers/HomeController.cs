using Microsoft.AspNetCore.Mvc;

namespace HzyAdmin.Areas.Admin.Controllers
{
    using Logic;
    using Logic.SysClass;
    using System;
    using System.Text;

    public class HomeController : BaseController
    {

        Sys_MenuLogic _Sys_MenuLogic = new Sys_MenuLogic();

        protected override void Init()
        {
            base.Init();
            this.IsExecutePowerLogic = false;
        }

        public override IActionResult Index()
        {
            StringBuilder _StringBuilder = new StringBuilder();
            _Sys_MenuLogic.CreateMenus(Guid.Empty, _StringBuilder);
            ViewData["MenuHtml"] = _StringBuilder.ToString();
            return View(this._Account);
        }

        public IActionResult Main()
        {
            return View();
        }



    }
}