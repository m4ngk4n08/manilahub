
using AutoMapper;
using manilahub.Authentication.Model;
using manilahub.data.Entity;
using manilahub.Modules.Admin.Model;
using manilahub.Modules.Transactions.Model;

namespace manilahub.Modules.Authentication.Profiles
{
    public class RegisterProfile : Profile
    {
        public RegisterProfile()
        {
            CreateMap<RegisterModel, Player>();
            CreateMap<LoginModel, Login>();
            CreateMap<PlayerModel, Player>();
            CreateMap<AgentModel, Agent>();
            CreateMap<TransactionModel, TransactionR>();
        }
    }
}
