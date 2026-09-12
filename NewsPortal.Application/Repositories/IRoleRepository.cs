using NewsPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Application.Repositories
{
    public interface IRoleRepository
    {
        Task<Role?> GetNameAsync(
            string name,
            CancellationToken cancellationToken = default);
    }
}
