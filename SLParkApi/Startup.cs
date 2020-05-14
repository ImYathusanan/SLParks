using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SLParkApi.Data;
using SLParkApi.ParkMapping;
using SLParkApi.Repository;
using SLParkApi.Repository.IRepository;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace SLParkApi
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
            services.AddDbContext<ApplicationDbContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<INationalParkRepository, NationalParkRepository>();

            services.AddScoped<ITrailRepository, TrailRepository>();

            services.AddAutoMapper(typeof(ParkMappings));

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();

            //services.AddSwaggerGen(options => {
            //    options.SwaggerDoc("ParkOpenApiSpec",
            //        new Microsoft.OpenApi.Models.OpenApiInfo()
            //        {
            //            Title = "Park APi",
            //            Version = "1",
            //            Description = "My api",
            //            Contact = new Microsoft.OpenApi.Models.OpenApiContact()
            //            {
            //                Email = "something@gmail.com",
            //                Name = "Yathusanan Yogarajah",
            //                Url = new Uri("https://wwww.yathusanan.com")
            //            },
            //            License = new Microsoft.OpenApi.Models.OpenApiLicense()
            //            {
            //                Name = "MIT License",
            //                Url = new Uri("https://www.google.com")
            //            }
            //        });

            //    //options.SwaggerDoc("ParkOpenApiSpecTrial",
            //    //    new Microsoft.OpenApi.Models.OpenApiInfo()
            //    //    {
            //    //        Title = "Park API (Trial)",
            //    //        Version = "1",
            //    //        Description = "My api Trial",
            //    //        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
            //    //        {
            //    //            Email = "something@gmail.com",
            //    //            Name = "Yathusanan Yogarajah",
            //    //            Url = new Uri("https://wwww.yathusanan.com")
            //    //        },
            //    //        License = new Microsoft.OpenApi.Models.OpenApiLicense()
            //    //        {
            //    //            Name = "MIT License",
            //    //            Url = new Uri("https://www.google.com")
            //    //        }
            //    //    });

            //    var xmlCommentfile = $"{Assembly.GetExecutingAssembly().GetName().Name}";
            //    var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentfile);
            //    options.IncludeXmlComments(cmlCommentsFullPath);
            //});

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(options => 
                {
                    options.SwaggerEndpoint("/swagger/ParkOpenApiSpec/swagger.json", "Park API");
                    
                });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
