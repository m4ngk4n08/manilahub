
using AutoMapper;
using manilahub.Authentication.Model;
using manilahub.data.Entity;

namespace manilahub.Modules.Authentication.Profiles
{
    public class RegisterProfile : Profile
    {
        public RegisterProfile()
        {
            CreateMap<RegisterModel, Register>();
        }
    }
}
