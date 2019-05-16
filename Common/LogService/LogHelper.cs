﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.LogService
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Logging;
    using NLog;
    using NLog.Extensions.Logging;
    using NLog.Web;
    using System.IO;
    /// <summary>
    /// 参考地址：  
    ///             https://www.cnblogs.com/muyeh/p/9788311.html 
    ///             https://www.cnblogs.com/qulianqing/p/7222177.html
    /// </summary>
    public class LogHelper
    {
        public static Logger _Logger { get; set; }

        public LogHelper() { }

        public static void Init(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);//这是为了防止中文乱码
            loggerFactory.AddNLog();//添加NLog
            env.ConfigureNLog(env.ContentRootPath + "\\NLog\\nlog.config");//读取Nlog配置文件

            _Logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="Text"></param>
        public void WriteLog(string Text)
        {
            _Logger.Info(Text);
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="_Exception"></param>
        public void WriteLog(string Text, Exception _Exception)
        {
            if (_Logger.IsErrorEnabled)
            {
                _Logger.Error(_Exception, Text);
            }
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="_Exception"></param>
        /// <param name="UserHostAddress"></param>
        /// <param name="CallBack"></param>
        public void WriteLog(Exception _Exception, string UserHostAddress, Action<StringBuilder> CallBack = null)
        {
            var sb = new StringBuilder();
            var _Message = "异常信息: " + _Exception.Message;
            var _Source = "错误源:" + _Exception.Source;
            var _StackTrace = "堆栈信息:" + _Exception.StackTrace;

            sb.Append("\r\n" + UserHostAddress + "\r\n" + _Message + "\r\n" + _Source + "\r\n" + _StackTrace + "\r\n");

            if (CallBack != null)
            {
                CallBack(sb);
            }

            this.WriteLog(sb.ToString(), _Exception);
        }

    }
}
