using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using UEditor.Core;

namespace HzyAdmin.Areas.Admin.Controllers
{
    //[EnableCors("any")]//跨域
    [Aop.AopActionFilter(false)]
    public class UEditorController : BaseController
    {
        protected override void Init()
        {
            base.Init();
            this.IsExecutePowerLogic = false;
        }

        private readonly UEditorService _ueditorService;
        public UEditorController(UEditorService ueditorService)
        {
            this._ueditorService = ueditorService;
        }

        //如果是API，可以按MVC的方式特别指定一下API的URI
        [HttpGet, HttpPost]
        public ContentResult Upload()
        {
            var response = _ueditorService.UploadAndGetResponse(HttpContext);
            return Content(response.Result, response.ContentType);
        }

    }
}