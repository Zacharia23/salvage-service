using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Identity;
using SalvageCore.Models;
using Serilog;

namespace SalvageCore.Data;

public class SeedData
{
    public static async Task EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await EnsureSeedData(context, roleManager);
        }
    }

    private static async Task EnsureSeedData(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
    {
        await SeedRoles(roleManager);
        await SeedIdentities(context);
        await SeedRegions(context);
        await SeedCurrencies(context);
    }

    private static async Task SeedCurrencies(ApplicationDbContext context)
    {
        try
        {
            List<Currency> currencies =
            [
                new()
                {
                    Id = Guid.CreateVersion7(),
                    Name = "Tanzanian Shillings",
                    Symbol = "/=",
                    Code = "TZS",
                    CreatedDate = DateTime.UtcNow
                },
                new()
                {
                    Id = Guid.CreateVersion7(),
                    Name = "US Dollar",
                    Symbol = "$",
                    Code = "USD",
                    CreatedDate = DateTime.UtcNow
                }
            ];

            // Use BulkInsertOrUpdate to insert new or update existing based on Code
            await context.BulkInsertOrUpdateAsync(currencies, options =>
            {
                options.UpdateByProperties = new List<string> { "Code" };
                options.PropertiesToExcludeOnUpdate = new List<string>
                {
                    nameof(Currency.Id),
                    nameof(Currency.CreatedDate)
                };
                options.SetOutputIdentity = false;
                options.BatchSize = 100;
            });
        }
        catch (Exception exception)
        {
            Log.Information("Failed to seed currencies: {@message}", exception.Message);
            throw new Exception("Failed to seed currencies", exception);
        }
    }

    private static async Task SeedRegions(ApplicationDbContext context)
    {
        try
        {
            List<Region> regions =
            [
                new()
                {
                    Id = Guid.CreateVersion7(),
                    RegionIso = "ARS01",
                    RegionName = "Arusha"
                },

                new()
                {
                    Id = Guid.CreateVersion7(),
                    RegionIso = "DSM02",
                    RegionName = "Dar es Salaam"
                },

                new()
                {
                    Id = Guid.CreateVersion7(),
                    RegionIso = "DOM03",
                    RegionName = "Dodoma"
                },

                new()
                {
                    Id = Guid.CreateVersion7(),
                    RegionIso = "GTA27",
                    RegionName = "Geita"
                },

                new()
                {
                    Id = Guid.CreateVersion7(),
                    RegionIso = "IRA27",
                    RegionName = "Iringa"
                },

                new()
                {
                    Id = Guid.CreateVersion7(),
                    RegionIso = "KGR27",
                    RegionName = "Kagera"
                },

                new()
                {
                    Id = Guid.CreateVersion7(),
                    RegionIso = "KTV28",
                    RegionName = "Katavi"
                },

                new()
                {
                    Id = Guid.CreateVersion7(),
                    RegionIso = "KGM08",
                    RegionName = "Kigoma"
                },

                new()
                {
                    Id = Guid.CreateVersion7(),
                    RegionIso = "KLM09",
                    RegionName = "Kilimanjaro"
                },

                new()
                {
                    Id = Guid.CreateVersion7(),
                    RegionIso = "LND12",
                    RegionName = "Lindi"
                },

                new()
                {
                    Id = Guid.CreateVersion7(),
                    RegionIso = "MYR26",
                    RegionName = "Manyara"
                },

                new()
                {
                    Id = Guid.CreateVersion7(),
                    RegionIso = "MAR13",
                    RegionName = "Mara"
                },

                new()
                {
                    Id = Guid.CreateVersion7(),
                    RegionIso = "MBY14",
                    RegionName = "Mbeya"
                },

                new()
                {
                    Id = Guid.CreateVersion7(),
                    RegionIso = "MOR16",
                    RegionName = "Morogoro"
                },

                new()
                {
                    Id = Guid.CreateVersion7(),
                    RegionIso = "MTR17",
                    RegionName = "Mtwara"
                },

                new()
                {
                    Id = Guid.CreateVersion7(),
                    RegionIso = "MZA18",
                    RegionName = "Mwanza"
                },

                new()
                {
                    Id = Guid.CreateVersion7(),
                    RegionIso = "NJ29",
                    RegionName = "Njombe"
                },

                new()
                {
                    Id = Guid.CreateVersion7(),
                    RegionIso = "PN09",
                    RegionName = "Pemba North"
                },

                new()
                {
                    Id = Guid.CreateVersion7(),
                    RegionIso = "PS10",
                    RegionName = "Pemba South"
                },

                new()
                {
                    Id = Guid.CreateVersion7(),
                    RegionIso = "PWN19",
                    RegionName = "Pwani"
                },

                new()
                {
                    Id = Guid.CreateVersion7(),
                    RegionIso = "RK20",
                    RegionName = "Rukwa"
                },

                new()
                {
                    Id = Guid.CreateVersion7(),
                    RegionIso = "RV21",
                    RegionName = "Ruvuma"
                },

                new()
                {
                    Id = Guid.CreateVersion7(),
                    RegionIso = "SHY22",
                    RegionName = "Shinyanga"
                },

                new()
                {
                    Id = Guid.CreateVersion7(),
                    RegionIso = "SMY30",
                    RegionName = "Simiyu"
                },

                new()
                {
                    Id = Guid.CreateVersion7(),
                    RegionIso = "SNG23",
                    RegionName = "Singida"
                },

                new()
                {
                    Id = Guid.CreateVersion7(),
                    RegionIso = "TBR24",
                    RegionName = "Tabora"
                },

                new()
                {
                    Id = Guid.CreateVersion7(),
                    RegionIso = "TNG25",
                    RegionName = "Tanga"
                },

                new()
                {
                    Id = Guid.CreateVersion7(),
                    RegionIso = "ZN07",
                    RegionName = "Zanzibar North"
                },

                new()
                {
                    Id = Guid.CreateVersion7(),
                    RegionIso = "ZSC11",
                    RegionName = "Zanzibar South and Central"
                },

                new()
                {
                    Id = Guid.CreateVersion7(),
                    RegionIso = "ZW15",
                    RegionName = "Zanzibar West"
                }
            ];
            await context.BulkInsertOrUpdateAsync(regions, options =>
            {
                options.UpdateByProperties = new List<string> { "RegionIso" };
                options.PropertiesToExcludeOnUpdate = new List<string>
                {
                    nameof(Region.Id),
                    nameof(Region.CreatedDate)
                };
                options.SetOutputIdentity = false;
                options.BatchSize = 100;
            });
        }
        catch (Exception exception)
        {
            Log.Information("Failed to seed regions: {@message}", exception.Message);
            throw new Exception("Failed to seed regions", exception);
        }
    }

    private static async Task SeedIdentities(ApplicationDbContext context)
    {
        try
        {
            List<IdentityType> identities =
            [
                new()
                {
                    Name = "NIN",
                    Description = "National Identification Number"
                },
                new()
                {
                    Name = "Passport Number",
                    Description = "Passport Number"
                },
                new()
                {
                    Name = "Drivers Licence",
                    Description = "Drivers Licence"
                }
            ];

            await context.BulkInsertOrUpdateAsync(identities, options =>
            {
                options.UpdateByProperties = new List<string> { "Name" };
                options.PropertiesToExcludeOnUpdate = new List<string>
                {
                    nameof(IdentityType.Id),
                    nameof(IdentityType.CreatedDate)
                };
                options.SetOutputIdentity = false;
                options.BatchSize = 100;
            });
        }
        catch (Exception exception)
        {
            Log.Information("Failed to seed identities: {@message}", exception.Message);
            throw new Exception("Failed to seed identities", exception);
        }
    }

    private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        try
        {
            string[] roles = ["Administrator", "Customer", "Manager"];

            foreach (var role in roles)
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole { Name = role });
        }
        catch (Exception exception)
        {
            Log.Information("Failed to seed roles: {@message}", exception.Message);
            throw new Exception("Failed to seed roles", exception);
        }
    }
}
