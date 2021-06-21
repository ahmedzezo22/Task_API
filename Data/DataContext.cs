using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_API.Models;

namespace Task_API.Data
{
    public class DataContext : IdentityDbContext<AppUser,AppRole,string>
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        { }
       
    }
}
