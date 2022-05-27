using manilahub.core.Services.IServices;
using manilahub.data.Entity;
using manilahub.data.Enum;
using manilahub.data.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace manilahub.core.Services
{
    public class AdminServices : IAdminServices
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IUserRepository _userRepository;
        private readonly IActionContextAccessor _actionContext;
        private readonly IHttpContextAccessor _httpContext;
        private static Random random = new Random();

        public AdminServices(
            IAdminRepository adminRepository,
            IUserRepository userRepository,
            IActionContextAccessor actionContext,
            IHttpContextAccessor httpContext)
             
        {
            _adminRepository = adminRepository;
            _userRepository = userRepository;
            _actionContext = actionContext;
            _httpContext = httpContext;
        }

        public async Task<bool> UpdateRole(Player model)
        {
            var userInfo = await _userRepository.GetById(model.UserId);
            var agentInfo = await _userRepository.GetAgentInfo(userInfo.AgentId);

            var player = new Player
            {
                UserId = model.UserId,
                AgentId = agentInfo.AgentId,
                Role = model.Role,
                Status = StatusEnum.Approved
            };

            await _userRepository.UpdatePlayerStatus(player);

            return true;
        }

        public async Task<bool> Promote(Player model)
        {
            try
            {
                var userInfo = await  _userRepository.Get(model.Username);
                var currentUserInfo = await _userRepository.Get(_httpContext.HttpContext.User.Identity.Name);
                switch (currentUserInfo.Role)
                {
                    case RoleEnum.GOLD:
                        if (userInfo.Role is RoleEnum.GOLD || userInfo.Role is RoleEnum.OPERATOR || userInfo.Role is RoleEnum.MASTER)
                        {
                            _actionContext.ActionContext.ModelState.AddModelError("error", "Unable to promote");
                            return false;
                        }
                        break;
                    case RoleEnum.MASTER:
                        if (userInfo.Role is RoleEnum.GOLD || userInfo.Role is RoleEnum.OPERATOR ||  userInfo.Role is RoleEnum.MASTER)
                        {
                            _actionContext.ActionContext.ModelState.AddModelError("error", "Unable to promote");
                            return false;
                        }
                        break;
                    case RoleEnum.OPERATOR:
                        if (userInfo.Role is RoleEnum.MASTER || userInfo.Role is RoleEnum.OPERATOR)
                        {
                            _actionContext.ActionContext.ModelState.AddModelError("error", "Unable to promote");
                            return false;
                        }
                        break;
                    case RoleEnum.ADMIN:
                        break;
                    case RoleEnum.PLAYER:
                        return false;
                    default:
                        break;
                }

                if (userInfo.Role is RoleEnum.PLAYER)
                {
                    var agent = new Agent
                    {
                        ReferralCode = ReferralCode(),
                        Percentage = Commission(model.Role)
                    };

                    var agentId = await _adminRepository.Insert(agent);

                    var updateStatus = new Player
                    {
                        UserId = userInfo.UserId,
                        AgentId = agentId,
                        Role = model.Role,
                        Status = StatusEnum.Approved
                    };

                    await _userRepository.UpdatePlayerStatus(updateStatus);
                }
                else
                {
                    int role = 0;
                    switch (userInfo.Role)
                    {
                        case RoleEnum.GOLD:
                            role = (int)RoleEnum.MASTER;
                            break;
                        case RoleEnum.MASTER:
                            role = (int)RoleEnum.OPERATOR;
                            break;
                        default:
                            return false;
                    }

                    var updateStatus = new Player
                    {
                        UserId = userInfo.UserId,
                        AgentId = userInfo.AgentId,
                        Role = (RoleEnum)role,
                        Status = StatusEnum.Approved
                    };

                    await _userRepository.UpdatePlayerStatus(updateStatus);
                }

                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        string ReferralCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 5)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        double Commission(RoleEnum role)
        {
            switch (role)
            {
                case RoleEnum.GOLD:
                    return 0.2;
                case RoleEnum.MASTER:
                    return 0.3;
                case RoleEnum.OPERATOR:
                    return 0.4;
                default:
                    return 0;
            }
        }
    }
}
