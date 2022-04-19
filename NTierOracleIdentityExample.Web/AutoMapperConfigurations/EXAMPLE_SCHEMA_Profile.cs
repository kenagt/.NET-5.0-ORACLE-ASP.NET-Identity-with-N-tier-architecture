using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NTierOracleIdentityExample.Dll.Entities;
using NTierOracleIdentityExample.Web.Models;

namespace NTierOracleIdentityExample.Web.AutoMapperConfigurations
{
    public class EXAMPLE_SCHEMA_Profile : Profile
    {
        public EXAMPLE_SCHEMA_Profile()
        {
            CreateMap<Log, LogViewModel>().ReverseMap();
            CreateMap<ExampleTable, ExampleTableViewModel>().ReverseMap();
            CreateMap<IdentityRole, RoleViewModel>().ReverseMap();
            CreateMap<ApplicationUser, ApplicationUserViewModel>().ReverseMap();
        }
    }
}
