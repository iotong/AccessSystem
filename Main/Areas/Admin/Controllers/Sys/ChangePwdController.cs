using Microsoft.AspNetCore.Mvc;

namespace HzyAdmin.Areas.Admin.Controllers.Sys
{
    //
    using Aop;
    using Common;
    using Entitys.SysClass;
    using Logic.SysClass;
    using System.Collections;

    /// <summary>
    /// 修改密码
    /// </summary>
    public class ChangePwdController : BaseController
    {
        public AccountLogic _Logic = new AccountLogic();

        protected override void Init()
        {
            this.MenuID = "Z-150";
        }

        public override IActionResult Index()
        {
            ViewData["userName"] = _Logic.Get(_Account.UserID).User_Name;
            return View();
        }

        [HttpPost]
        public IActionResult ChangePwd(string oldpwd, string newpwd, string newlypwd)
        {
            _Logic.ChangePwd(oldpwd, newpwd, newlypwd);
            return this.Success();
        }












    }
}