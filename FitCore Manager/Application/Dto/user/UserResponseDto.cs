using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.user
{
    public class UserResponseDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string? Token { get; set; }
        public string Email { get; set; }
        public string? Role { get; set; }
        public string? Error { get; set; }
    }
}
