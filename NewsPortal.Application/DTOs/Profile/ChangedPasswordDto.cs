using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Application.DTOs.Profile
{
    public class ChangedPasswordDto
    {
        public required string CurrentPassword { get; set; } 

        public required string NewPassword { get; set; }

        public required string ConfirmPassword { get; set; }




    }
}
