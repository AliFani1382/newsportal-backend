using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Application.Interfaces
{
    public interface ISlugService
    {
        string Generate(string text);
        Task<string> GenerateUniqueAsync(string slug);
    }
}
