using Authors.Helpers;
using AutoMapper;
using DataTransferObject.Mapper;
using DbLayer;
using DtoLayer.Dto;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Repository.Implementation;
using Repository.Interface;
using Services.Implementation;
using Services.Interface;
using System;
using System.Diagnostics;

namespace Authors
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
            
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //Cachle iþlemleri yapabilmek için eklendi
            services.AddMemoryCache();

            var connection = @"Server=localhost\SQLEXPRESS;Database=AuthorDb;Trusted_Connection=True;";

            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(connection);
            });

            //Repositories Injection
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            //Services Injection
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<ICategoryService, CategoryService>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDistributedMemoryCache();
            services.AddSession(options => {
                options.Cookie.Name = ".AuthorsWorks.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(15);
            });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseSession();
            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa => {
                spa.Options.SourcePath = "ClientApp";
                if (env.IsDevelopment())
                    spa.UseAngularCliServer(npmScript: "start");
            });

            app.Run(async (context) => {
                var user = context.Session.GetObject<AuthorDto>("CurrentUser");
                await context.Response.WriteAsync($"{user}");
            });
        }
        
    }
}
