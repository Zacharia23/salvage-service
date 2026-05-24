using System.Text;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SalvageCore.Data;
using SalvageCore.Interface;
using SalvageCore.Models;
using SalvageCore.Repository;
using SalvageCore.Service;

namespace SalvageCore.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
        }).AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme =
                options.DefaultChallengeScheme =
                    options.DefaultForbidScheme =
                        options.DefaultScheme =
                            options.DefaultSignInScheme =
                                options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = configuration["JWT:Issuer"],
                ValidateAudience = true,
                ValidAudience = configuration["JWT:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["JWT:SigninKey"] ?? string.Empty)
                )
            };
        });

        services.AddOptions();

        // Hangfire Client
        services.AddHangfire(config =>
        {
            config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180);
            config.UseSimpleAssemblyNameTypeSerializer();
            config.UseRecommendedSerializerSettings();
            config.UsePostgreSqlStorage(configuration.GetConnectionString("HangfireMain"));
        });

        // Hangfire Server
        services.AddHangfireServer();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IMakeRepository, MakeRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IOfferRepository, OfferRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IUploadService, FileUploadService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<ITemplateRepository, TemplateRepository>();
        services.AddScoped<IRedisCacheService, RedisCacheService>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<IVerifyCodeService, VerifyCodeService>();
        services.AddScoped<IPdfService, PdfService>();

        // services.AddTransient<IApiKeyValidation, ApiKeyValidation>();
        // services.AddScoped<ApiKeyAuthFilter>();

        return services;
    }
}