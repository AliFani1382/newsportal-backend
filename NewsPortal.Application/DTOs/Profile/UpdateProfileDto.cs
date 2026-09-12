using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Application.DTOs.Profile
{
    public class UpdateProfileDto
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }

    }
}
