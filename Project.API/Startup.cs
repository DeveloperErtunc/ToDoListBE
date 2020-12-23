using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Project.BLL.Services;
using Project.DAL.Context;
using Project.DAL.Models;
using static Project.BLL.Services.IServicesAppUser;

namespace Project.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MyContext>();
            services.AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 0;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequiredUniqueChars = 0;
            }).AddEntityFrameworkStores<MyContext>().AddDefaultTokenProviders();
            services.AddAuthentication(at =>
            {
                at.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                        //Kim Tarafından Üretiliyor
                        ValidIssuer = "http://localhost",
                        //Kim Tarafından Kullanılıyor
                        ValidAudience = "http://localhost",
                        ///Key 
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ertunc123ertunc123ertunc123")),
                        //Key kontrol
                        ValidateIssuerSigningKey = true,
                        //Web Token Süre Kontrol.
                        ValidateLifetime = true,
                        //Her Hangi bir zaman farklılıgı.
                        ClockSkew = System.TimeSpan.Zero
                };
            });
            
            services.AddControllers();
            services.AddHttpClient();
            services.AddScoped<IServicesAppUser, ServicesAppUser>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
