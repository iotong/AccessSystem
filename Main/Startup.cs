using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HzyAdmin
{
    using System.IO;
    using UEditor.Core;
    using Common;
    using Common.LogService;

    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            _IHostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment _IHostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container. 
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;//关闭GDPR规范
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //session 注册
            services.AddSession(item =>
            {
                item.IdleTimeout = TimeSpan.FromMinutes(60 * 2);
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //自定义 视图 
            services.Configure<Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions>(item =>
            {
                item.AreaViewLocationFormats.Clear();
                item.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");

                item.AreaViewLocationFormats.Add("/Areas/{2}/Views/{1}/{0}.cshtml");
                item.AreaViewLocationFormats.Add("/Areas/{2}/Views/Shared/{0}.cshtml");
                item.AreaViewLocationFormats.Add("/Areas/{2}/Views/Sys/{1}/{0}.cshtml");//系统管理
                item.AreaViewLocationFormats.Add("/Areas/{2}/Views/Base/{1}/{0}.cshtml");//基础信息管理
                item.AreaViewLocationFormats.Add("/Areas/{2}/Views/Operate/{1}/{0}.cshtml");//运营管理
                item.AreaViewLocationFormats.Add("/Areas/{2}/Views/Statistics/{1}/{0}.cshtml");//统计管理
            });

            //注入链接字符串
            DbFrame.SqlServerContext.DbContextSqlServer.SetDefaultConnectionString(Configuration.GetSection("AppConfig:SqlServerConnStr").Value);

            //Ueditor  编辑器 服务端 注入  configFileRelativePath: "wwwroot/Admin/libs/nUeditor/net/config.json", isCacheConfig: false, basePath: "C:/basepath"
            services.AddUEditorService(
                    configFileRelativePath: _IHostingEnvironment.WebRootPath + "\\Admin\\libs\\neditor\\net\\config.json",
                    isCacheConfig: false,
                    basePath: _IHostingEnvironment.WebRootPath + "\\Admin\\libs\\neditor\\net\\"
                );

            //配置跨域处理
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("any", builder =>
            //    {
            //        builder.AllowAnyOrigin() //允许任何来源的主机访问
            //        .AllowAnyMethod()
            //        .AllowAnyHeader()
            //        .AllowCredentials();//指定处理cookie
            //    });
            //});

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            //全局配置Json序列化处理
            .AddJsonOptions(options =>
            {
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                //不使用驼峰样式的key
                options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                //设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime applicationLifetime)
        {
            //添加控制台输出
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();

            //初始化NLog
            LogHelper.Init(app, env, loggerFactory);

            //程序启动
            applicationLifetime.ApplicationStarted.Register(() =>
            {
                Tools.Log.WriteLog("App启动");
            });
            //程序正在结束中
            applicationLifetime.ApplicationStopping.Register(() =>
            {
                Tools.Log.WriteLog("App结束中...");
            });
            //程序已结束
            applicationLifetime.ApplicationStopped.Register(() =>
            {
                Tools.Log.WriteLog("App已结束");
            });
            //applicationLifetime.StopApplication();//停止程序

            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            //session 注册
            app.UseSession();
            //将 对象 IHttpContextAccessor 注入 HttpContextHelper 静态对象中
            Common.HttpContextService.HttpContextHelper.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            
            app.Use(async (context, next) =>
            {
                //如果 404 默认跳转首页
                if (context.Response.StatusCode == 404)
                    context.Response.Redirect("/404");
                else
                    await next.Invoke();
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


               routes.MapRoute(
               name: "areas",
               template: "{area:exists}/{controller=Access}/{action=Index}/{id?}");


                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


        }
    }
}
