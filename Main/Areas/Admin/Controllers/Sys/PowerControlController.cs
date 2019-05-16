using Microsoft.AspNetCore.Mvc;

namespace HzyAdmin.Areas.Admin.Controllers.Sys
{
    public class PowerControlController : Controller
    {
        //权限控制
        // GET: /ManageSys/PowerControl/

        public IActionResult Index()
        {
            return PartialView();
        }
    }
}