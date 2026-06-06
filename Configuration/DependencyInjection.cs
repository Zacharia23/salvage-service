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
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 4;
            options.Password.RequiredUniqueChars = 1;
            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        }).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

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
            options.MapInboundClaims = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = configuration["JWT:Issuer"],
                ValidateAudience = true,
                ValidAudience = configuration["JWT:Audience"],
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1),
                NameClaimType = "sub",
                RoleClaimType = "role",
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
        services.AddScoped<IPasswordHasher<TempCustomer>, PasswordHasher<TempCustomer>>();

        // services.AddTransient<IApiKeyValidation, ApiKeyValidation>();
        // services.AddScoped<ApiKeyAuthFilter>();

        return services;
    }
}
