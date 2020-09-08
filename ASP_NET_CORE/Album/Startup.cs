using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Album.Data;
using Album.Mail;
using Album.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Album {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {

            services.AddOptions ();                                        // Kích hoạt Options
            var mailsettings = Configuration.GetSection ("MailSettings");  // đọc config
            services.Configure<MailSettings> (mailsettings);               // đăng ký để Inject

            services.AddTransient<IEmailSender, SendMailService>();        // Đăng ký dịch vụ Mail


            // Đăng ký AppDbContext
            services.AddDbContext<AppDbContext> (options => {
                // Đọc chuỗi kết nối
                string connectstring = Configuration.GetConnectionString ("AppDbContext");
                // Sử dụng MS SQL Server
                options.UseSqlServer (connectstring);
            });

            // Đăng ký Identity
            services.AddIdentity<AppUser, IdentityRole> ()
                .AddEntityFrameworkStores<AppDbContext> ()
                .AddDefaultTokenProviders ();

            // Truy cập IdentityOptions
            services.Configure<IdentityOptions> (options => {
                // Thiết lập về Password
                options.Password.RequireDigit = false; // Không bắt phải có số
                options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
                options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
                options.Password.RequireUppercase = false; // Không bắt buộc chữ in
                options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
                options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

                // Cấu hình Lockout - khóa user
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes (5); // Khóa 5 phút
                options.Lockout.MaxFailedAccessAttempts = 50; // Thất bại 5 lầ thì khóa
                options.Lockout.AllowedForNewUsers = true;

                // Cấu hình về User.
                options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true; // Email là duy nhất

                // Cấu hình đăng nhập.
                options.SignIn.RequireConfirmedEmail = true; // Cấu hình xác thực địa chỉ email (email phải tồn tại)
                options.SignIn.RequireConfirmedPhoneNumber = false; // Xác thực số điện thoại

            });
            
            services.Configure<RouteOptions> (options => {
                options.LowercaseUrls = true;                   // url chữ thường
                options.LowercaseQueryStrings = false;          // không bắt query trong url phải in thường
            });

            services.AddRazorPages ();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            } else {
                app.UseExceptionHandler ("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts ();
            }

            app.UseHttpsRedirection ();
            app.UseStaticFiles ();

            app.UseRouting ();

            app.UseAuthentication (); // Phục hồi thông tin đăng nhập (xác thực)
            app.UseAuthorization (); // Phục hồi thông tinn về quyền của User

            app.UseEndpoints (endpoints => {
                endpoints.MapRazorPages ();
            });
        }
    }
}