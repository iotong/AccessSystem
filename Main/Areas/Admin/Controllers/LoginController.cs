using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace HzyAdmin.Areas.Admin.Controllers
{
    //
    using Aop;
    using Entitys;
    using Entitys.SysClass;
    using DbFrame;
    using Common;
    using Logic.SysClass;
    using Common.VerificationCode;

    [AopActionFilter(false)]
    public class LoginController : BaseController
    {
        protected override void Init()
        {
            this.IsExecutePowerLogic = false;
        }

        AccountLogic _AccountLogic = new AccountLogic();

        [Route(""), Route("/Admin"), Route("/Admin/Index"), Route("/Admin/Login")]
        public override IActionResult Index()
        {
            Tools.SetSession("Account", new Account());
            return View();
        }

        /// <summary>
        /// 检查 登录 信息
        /// </summary>
        /// <param name="uName"></param>
        /// <param name="uPwd"></param>
        /// <param name="loginCode"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Checked(string uName, string uPwd, string loginCode)
        {
            _AccountLogic.Checked(uName, uPwd, loginCode);
            return this.Success(new
            {
                status = 1,
                jumpurl = AppConfig.HomePageUrl + "#!%u9996%u9875#!/Admin/Home/Main/"
            });
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        public IActionResult GetYZM()
        {
            var _Helper = new Helper();
            Tools.SetCookie("loginCode", _Helper.Text, 2);
            return File(_Helper.GetBytes(), Tools.GetFileContentType[".bmp"]);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public IActionResult Out()
        {
            return RedirectToAction("Index");
        }





    }
}