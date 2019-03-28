using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using BrightTest.Models;
using BrightTest.Utilities;
using Newtonsoft.Json;

namespace BrightTest
{
    public class Startup
    {
        private const string _corsName = "AllowAllOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(
                    _corsName,
                    policyBuilder =>
                    {
                        policyBuilder.AllowAnyOrigin();
                        policyBuilder.AllowAnyHeader();
                        policyBuilder.AllowAnyMethod();
                    });
            });

            services.AddDbContext<RequestContext>(optionsBuidler =>
            {
                optionsBuidler.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddSingleton<HttpClient>();

            AddMapping(services);

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(_corsName);

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private void AddMapping(IServiceCollection services)
        {
            var mapConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile<MappingProfile>();
            });
            var mapper = mapConfiguration.CreateMapper();

            services.AddSingleton<IMapper>(mapper);
        }
    }
}
