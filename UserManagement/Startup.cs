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
using Microsoft.OData.Edm;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using System.IO;
using UserManagement.Service.Infrastructure;
using ILogger = UserManagement.Service.Infrastructure.ILogger;
using Microsoft.AspNetCore.Diagnostics;
using System.Text;
using Newtonsoft.Json;

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
            services.AddLocalization(o => { o.ResourcesPath = "Resources"; });

            var logger = new Serilogger(
                Path.Combine(
                    Directory.CreateDirectory(
                        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"logs")).FullName, $"usermanagement_{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}.txt"));
            services.AddSingleton<ILogger>(logger);
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                options.Filters.Add(typeof(ValidateModelStateAttribute));
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddDataAnnotationsLocalization(o =>
            {
                o.DataAnnotationLocalizerProvider = (type, factory) =>
                {
                    return factory.Create(typeof(SharedResource));
                };
            });
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
            IList<CultureInfo> supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en-Us"),
                new CultureInfo("ka-GE")
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("ka-GE"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";
                    var exception = context.Features.Get<IExceptionHandlerFeature>();
                    if(exception != null)
                    {
                        var ex = exception.Error;
                        var logger = context.RequestServices.GetService<ILogger>();
                        logger.LogException(ex);
                        byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(exception.Error.Message));
                        await context.Response.Body.WriteAsync(data, 0, data.Length);
                    }
                });
            });
            app.UseMvc(routeBuilder =>
            {

                routeBuilder.Select().Expand().Filter().OrderBy().MaxTop(100).Count();
                routeBuilder.MapODataServiceRoute("odataroute", "api", builder.GetEdmModel());
                routeBuilder.EnableDependencyInjection();
            });
        }
    }
}
