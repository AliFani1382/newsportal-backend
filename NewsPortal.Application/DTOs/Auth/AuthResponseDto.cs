using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Application.DTOs.Auth
{
    public sealed class AuthResponseDto
    {
        public int UserId { get; set; }
        public required string Username { get; set; }
        public required string Token { get; set; }
        public bool IsEmailVerified { get; set; }

    }
}
