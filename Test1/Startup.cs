using KnifeUI.Swagger.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Test1
{
    public class Startup
    {
        private IWebHostEnvironment webHostEnvironment;
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("test", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "MyAPI", Version = "test" });
                options.CustomOperationIds(apiDesc =>
                {
                    var controllerAction = apiDesc.ActionDescriptor as ControllerActionDescriptor;
                    return controllerAction.ControllerName + "-" + controllerAction.ActionName;
                });
                //记得设置工程属性:生成xml文档
                var xmlPath = Path.Combine(webHostEnvironment.ContentRootPath, Assembly.GetExecutingAssembly().GetName().Name + ".xml");
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath, true);
                };
                //options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "CoreApi.xml"), true);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "Swagger文档";
                //c.RoutePrefix = "";
                c.SwaggerEndpoint("/swagger/test/swagger.json", "MyAPI");

            }
                );
            app.UseKnife4UI(c =>
            {
                c.DocumentTitle = "KnifeUI.Swagger.Net 实例文档";
                c.RoutePrefix = "";
                c.SwaggerEndpoint($"test/api-docs", $"MyAPI");
            });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapSwagger("{documentName}/api-docs");
                endpoints.MapControllers();
            });
        }
    }
}
