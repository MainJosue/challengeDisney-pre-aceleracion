using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChallengeDisney.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ChallengeDisney.Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SendGrid.Extensions.DependencyInjection;
using ChallengeDisney.Services;

namespace ChallengeDisney
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
           

            services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ChallengeDisney", Version = "v1" });
            });

            services.AddIdentity<Usuario, IdentityRole>()
              .AddEntityFrameworkStores<UserContext>()
              .AddDefaultTokenProviders();

            services.AddAuthentication(options => 
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = "https://localhost:5001",
                    ValidIssuer = "https://localhost:5001",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["LlaveSecreta"]))
                };
            });

            services.AddDbContextPool<ChallengeDisneyContext>((services, options) =>
            {
                //options.UseInternalServiceProvider(services);
                options.UseSqlServer(Configuration.GetConnectionString("ChallengeConnectionString"));
            });

            services.AddDbContextPool<UserContext>((services, options) =>
            {
                //options.UseInternalServiceProvider(services);
                options.UseSqlServer(Configuration.GetConnectionString("UsuarioConnectionString"));
            });

            services.AddSendGrid(x => x.ApiKey = Configuration["LlaveEmail"]);
            services.AddTransient<IMailService, SendGridMailService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChallengeDisney v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
