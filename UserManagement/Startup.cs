using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UserManagement.Service.DAL;
using Swashbuckle.AspNetCore.Swagger;
using UserManagement.Infrastructure;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNet.OData.Builder;
using UserManagement.Service.Models;

namespace UserManagement
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
            services.AddDbContext<UserContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("UserManagement")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<FileUploadOperation>();
                c.DescribeAllEnumsAsStrings();
                c.CustomSchemaIds(type => type.FullName);
                c.IgnoreObsoleteActions();

                c.SwaggerDoc("v1", new Info { Title = $"{AppDomain.CurrentDomain.FriendlyName}", Version = "v1" });
            }
            );
            services.AddOData();
            services.AddMvcCore(options =>
            {
                foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
                foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
            }
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var builder = new ODataConventionModelBuilder(app.ApplicationServices);
            builder.EntitySet<User>("Users");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "User management"); });
            app.UseStaticFiles();
            app.UseMvc(routeBuilder=> {
                routeBuilder.EnableDependencyInjection();
                routeBuilder.Expand().Select().OrderBy().Filter().MaxTop(null).Count(Microsoft.AspNet.OData.Query.QueryOptionSetting.Allowed);
            });
        }
    }
}
