using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_API.DTO;
using Task_API.Models;

namespace Task_API.Helpers
{
    public class IMapperProfile :Profile
    {
        public IMapperProfile()
        {
            CreateMap<AppUser, UserReturnData>();
        }
    }
}
