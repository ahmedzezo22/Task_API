using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task_API.DTO
{
    public class UserReturnData
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }
}
