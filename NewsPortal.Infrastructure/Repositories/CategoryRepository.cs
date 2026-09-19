using Microsoft.EntityFrameworkCore;
using NewsPortal.Application.Repositories;
using NewsPortal.Domain.Entities;
using NewsPortal.Infrastructure.Persistence;

namespace NewsPortal.Infrastructure.Repositories
{
    public class CategoryRepository
     : Repository<Category>, ICategoryRepository
    {

        public CategoryRepository(
            ApplicationDBContext db)
            : base(db)
        {
        }

        public async Task<bool> ExistsBySlugAsync(
            string slug,
            int? excludeId = null)
        {
            return await dbSet.AnyAsync(
                x =>
                x.Slug == slug &&
                (!excludeId.HasValue ||
                 x.Id != excludeId.Value));
        }

        public async Task<Category?> GetBySlugAsync(
            string slug)
        {
            return await dbSet
                .FirstOrDefaultAsync(
                    x => x.Slug == slug);
        }

    }

}

