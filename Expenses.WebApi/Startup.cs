
using Expenses.Core;
using Expenses.DB;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;

namespace Expenses.WebApi
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

            //AppDB Context
            services.AddDbContext<AppDbContext>();

            //Expenses Services
            services.AddTransient<IExpensesServices, ExpensesServices>();

            //Users Services
            services.AddTransient<IUserService, UserService>();

            //Users Services
            services.AddTransient<IPasswordHasher, PasswordHasher>();

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            //Swagger
            services.AddSwaggerDocument(settings =>
            {
                settings.Title = "Expenses";
            });

            //CORS
            services.AddCors(options =>
            {
                options.AddPolicy("ExpensesPolicy",
                 builder =>
                  {
                    builder.WithOrigins("*")
                      .AllowAnyHeader()
                      .AllowAnyMethod();
                  });
            });

            var secret = Environment.GetEnvironmentVariable("JWT_SECRET");
            var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
            services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(opts =>
                {
                    opts.TokenValidationParameters = new TokenValidationParameters
                    {
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = issuer,
                      ValidateAudience = false,
                      IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(secret))
                    };
                });
                
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

            app.UseCors("ExpensesPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseOpenApi();

            app.UseSwaggerUi3();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
