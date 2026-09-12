using Microsoft.EntityFrameworkCore;
using NewsPortal.Domain.Entities;

namespace NewsPortal.Infrastructure.Persistence.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDBContext context)
    {
        await SeedRolesAsync(context);
        await SeedCategoriesAsync(context);
        await SeedCitiesAsync(context);

        await context.SaveChangesAsync();
    }

    private static async Task SeedRolesAsync(ApplicationDBContext context)
    {
        var roles = new[]
        {
            new Role
            {
                Name = "User"
            },
            new Role
            {
                Name = "Admin"
            }
        };

        foreach (var role in roles)
        {
            var exists = await context.Roles
                .AnyAsync(r => r.Name == role.Name);

            if (!exists)
            {
                await context.Roles.AddAsync(role);
            }
        }
    }

    private static async Task SeedCategoriesAsync(ApplicationDBContext context)
    {
        var categories = new[]
        {
            new Category
            {
                Name = "فناوری",
                Slug = "technology"
            },
            new Category
            {
                Name = "ورزش",
                Slug = "sport"
            },
            new Category
            {
                Name = "سیاسی",
                Slug = "politics"
            },
            new Category
            {
                Name = "اقتصاد",
                Slug = "economy"
            }
        };

        foreach (var category in categories)
        {
            var exists = await context.Categories
                .AnyAsync(c => c.Slug == category.Slug);

            if (!exists)
            {
                await context.Categories.AddAsync(category);
            }
        }
    }

    private static async Task SeedCitiesAsync(ApplicationDBContext context)
    {
        var cities = new[]
        {
            new City
            {
                Name = "تهران",
                Slug = "tehran"
            },
            new City
            {
                Name = "اهواز",
                Slug = "ahvaz"
            },
            new City
            {
                Name = "تبریز",
                Slug = "tabriz"
            },
            new City
            {
                Name = "شیراز",
                Slug = "shiraz"
            }
        };

        foreach (var city in cities)
        {
            var exists = await context.Cities
                .AnyAsync(c => c.Slug == city.Slug);

            if (!exists)
            {
                await context.Cities.AddAsync(city);
            }
        }
    }
}