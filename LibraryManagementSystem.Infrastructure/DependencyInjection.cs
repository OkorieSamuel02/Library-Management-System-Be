using LibraryManagementSystem.Application.Authentication.Interface;
using LibraryManagementSystem.Application.Automapper;
using LibraryManagementSystem.Application.Book.Interface;
using LibraryManagementSystem.Application.Borrowing.Interface;
using LibraryManagementSystem.Application.ConfigSetting.Interface;
using LibraryManagementSystem.Application.Membership.Interface;
using LibraryManagementSystem.Domain.Entity;
using LibraryManagementSystem.Infrastructure.Data;
using LibraryManagementSystem.Infrastructure.Helper;
using LibraryManagementSystem.Infrastructure.Repository.Authentication;
using LibraryManagementSystem.Infrastructure.Repository.BookCatalog;
using LibraryManagementSystem.Infrastructure.Repository.Borrowing;
using LibraryManagementSystem.Infrastructure.Repository.ConfigSettings;
using LibraryManagementSystem.Infrastructure.Repository.MemberShip;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection InfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IBorrowService, BorrowService>();
            services.AddScoped<ISettingsService, SettingsService>();

            services.AddScoped<AuthHelper>();

            services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            services.AddIdentity<User, IdentityRole>(option =>
            {
                option.Password.RequiredLength = 8;
                option.Password.RequireUppercase = true;
                option.Password.RequireLowercase = true;
                option.Password.RequireDigit = true;
               option.User.RequireUniqueEmail = true;
                
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
              {
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = configuration["JwtSettings:Issuer"],
                      ValidAudience = configuration["JwtSettings:Audience"],
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]!)),
                      ClockSkew = TimeSpan.Zero
                  };
              });
            return services;
        }
    }
}
