using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Tegetgram.Api.Filters;
using Tegetgram.Api.Models;
using Tegetgram.Data;
using Tegetgram.Data.Entities;
using Tegetgram.Services;
using Tegetgram.Services.Interfaces;

namespace Tegetgram.Api
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
            System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddDbContext<TegetgramDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApiUser, IdentityRole>().AddEntityFrameworkStores<TegetgramDbContext>();
            services.Configure<JWTSettings>(Configuration.GetSection("JWTSettings"));

            var jwtSettings = Configuration.GetSection("JWTSettings").Get<JWTSettings>();
            var secret = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);
            var issuer = jwtSettings.Issuer;
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secret),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = false,
                    NameClaimType = JwtRegisteredClaimNames.Sub
                };
            });

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IMessageService, MessageService>();
            services.AddTransient<IActivityLogger, ActivityLogger>();
            services.AddScoped<GlobalExceptionFilter>();
            services.AddScoped<LogAttribute>();
            services.AddAutoMapper(typeof(MappingProfile));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IMapper mapper)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();

            loggerFactory.AddFile("Logs/{Date}.txt");
        }
    }
}
