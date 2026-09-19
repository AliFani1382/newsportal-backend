using NewsPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Application.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {

        Task<Category?> GetBySlugAsync(
       string slug);

        Task<bool> ExistsBySlugAsync(
            string slug,
            int? excludeId = null);

    }
}
