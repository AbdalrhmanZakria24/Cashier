
using Fixawy.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Fixawy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // App Configration

            builder.Services.RegisterConfig();

            //Data Base Contex
            builder.Services.AddDbContext<ApplicationDBContext>(option =>
            {
                option.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                    );
            });

            // Identity

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option =>
            {
                option.User.RequireUniqueEmail = true;
                option.Password.RequiredLength = 8;
                option.Password.RequireDigit = true;
                option.Password.RequireUppercase = true;
                option.Password.RequireLowercase = true;
                option.Password.RequireNonAlphanumeric = false;
                option.SignIn.RequireConfirmedEmail = true;
                option.SignIn.RequireConfirmedPhoneNumber = false;
                option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                option.Lockout.MaxFailedAccessAttempts = 5;
            })
                .AddEntityFrameworkStores<ApplicationDBContext>()
                .AddDefaultTokenProviders();
            
                                       
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
