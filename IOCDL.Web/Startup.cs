using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Autofac.Features.AttributeFilters;
using IOCDL.Framework;
using IOCDL.Interface;
using IOCDL.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IOCDL.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            //指定服务的注册
            //containerBuilder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope().AsImplementedInterfaces();
            //containerBuilder.RegisterType<ProductService>().As<IProductService>().InstancePerLifetimeScope().AsImplementedInterfaces();
            //var container = containerBuilder.Build();
            //IUserService userService = container.Resolve<IUserService>();
            //IProductService productService = container.Resolve<IProductService>();
            //userService.Show();
            //productService.Show();            
            containerBuilder.RegisterType<IOCInterceptor>();
            containerBuilder.RegisterType<TestServiceA>().As<ITestServiceA>().InstancePerDependency()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(IOCInterceptor));
            //var basePath = System.AppDomain.CurrentDomain.BaseDirectory;
            //var servicesDllFile = Path.Combine(basePath, "Blog.Core.Services.dll"); 
            //var assemblysServices = Assembly.LoadFrom(servicesDllFile);
            //containerBuilder.RegisterAssemblyTypes(assemblysServices)
            //    .InstancePerLifetimeScope()
            //    .AsImplementedInterfaces().e
        }
    }
}
