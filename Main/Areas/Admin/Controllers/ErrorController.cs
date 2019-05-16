using Microsoft.AspNetCore.Mvc;
//
using Aop;
using Common;
using Main.Areas.Access.Controllers;

namespace HzyAdmin.Areas.Admin.Controllers
{
    [AopActionFilter(false)]
    public class ErrorController : BaseController
    {
        protected override void Init()
        {
            this.IsExecutePowerLogic = false;
        }

        public IActionResult Index(ErrorModel _ErrorModel)
        {
            return View(_ErrorModel);
        }

        [Route("/404")]
        public IActionResult Page404()
        {
            return View();
        }
    }
}