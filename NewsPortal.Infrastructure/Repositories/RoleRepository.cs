using Microsoft.EntityFrameworkCore;
using NewsPortal.Application.Repositories;
using NewsPortal.Domain.Entities;
using NewsPortal.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Infrastructure.Repositories
{
    public sealed class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDBContext _context;
        public RoleRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Role?> GetNameAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(
                x => x.Name == name,
                cancellationToken);
        }
    }
}
