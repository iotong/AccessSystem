using System;
using Microsoft.AspNetCore.Mvc;

namespace Aop
{
    //
    using Microsoft.AspNetCore.Mvc.Filters;
    using Common;
    using Entitys;
    using DbFrame;
    using Entitys.SysClass;

    public class AopActionFilterAttribute : ActionFilterAttribute
    {

        private bool _IsExecute { get; set; }

        public AopActionFilterAttribute(bool IsExecute = true)
        {
            this._IsExecute = IsExecute;
        }

        /// <summary>
        /// 每次请求Action之前发生，，在行为方法执行前执行
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            if (this._IsExecute)
            {
                //登陆超时验证
                this.CheckedLoginAccount(context);
            }

            base.OnActionExecuting(context);
        }

        /// <summary>
        /// 检查登录帐户
        /// </summary>
        private void CheckedLoginAccount(ActionExecutingContext context)
        {
            var accountM = Tools.GetSession<Account>("Account");

            if (accountM == null || accountM.UserID.ToGuid() == Guid.Empty)
            {
                if (Tools.IsAjaxRequest)
                {
                    context.Result = new JsonResult(new ErrorModel(AppConfig.LoginPageUrl, EMsgStatus.登录超时20));
                }
                else
                {
                    context.Result = new ContentResult()
                    {
                        Content = @"<script type='text/javascript'>
                                        alert('登录超时！系统将退出重新登录！');
                                        top.window.location='" + AppConfig.LoginPageUrl + @"';
                                    </script>",
                        ContentType = "text/html;charset=utf-8;"
                    };
                }
                return;
            }
        }


















    }
}
