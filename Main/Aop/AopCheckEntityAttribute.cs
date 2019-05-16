namespace Aop
{
    //
    using Microsoft.AspNetCore.Mvc.Filters;
    using Common;
    using DbFrame;
    using DbFrame.SqlServerContext;
    using Logic.Class;
    using Entitys.Class;

    /// <summary>
    /// 实体验证 特性
    /// </summary>
    public class AopCheckEntityAttribute : ActionFilterAttribute
    {
        private DbContextSqlServer db = new DbContextSqlServer();
        private CheckModel<BaseClass> _CheckModel = new CheckModel<BaseClass>();
        public string[] ParamName { get; set; }

        public AopCheckEntityAttribute()
        {
            this.ParamName = new string[] { "model" };
        }

        public AopCheckEntityAttribute(string[] _ParamName)
        {
            this.ParamName = _ParamName;
        }

        /// <summary>
        /// 每次请求Action之前发生，，在行为方法执行前执行
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (this.ParamName != null)
            {
                foreach (var item in this.ParamName)
                {
                    var _Value = (Entitys.Class.BaseClass)filterContext.ActionArguments[item];
                    if (_Value != null)
                    {
                        if (!_CheckModel.Check(_Value))
                        {
                            throw new MessageBox(_CheckModel.ErrorMessage);
                        }
                    }
                }
            }

        }

    }
}
