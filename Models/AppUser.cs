using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task_API.Models
{
    public class AppUser :IdentityUser
    {
        public string Gender { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

    }
}
