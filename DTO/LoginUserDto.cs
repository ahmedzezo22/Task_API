using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task_API.DTO
{
    public class LoginUserDto
    {
        public string  Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
