using AutoMapper;
using manilahub.Authentication.Model;
using manilahub.data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace manilahub.Modules.Authentication.Profiles
{
    public class LoginProfile : Profile
    {
        public LoginProfile()
        {
            CreateMap<LoginModel, Login>();
        }
    }
}
