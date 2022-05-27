using manilahub.core.Services.IServices;
using manilahub.data.Entity;
using manilahub.data.Enum;
using manilahub.data.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manilahub.core.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IActionContextAccessor _actionContext;
        private readonly ITransactionRepository _transactionRepository;

        public PlayerService(
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor,
            IActionContextAccessor actionContextAccessor,
            ITransactionRepository transactionRepository)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _actionContext = actionContextAccessor;
            _transactionRepository = transactionRepository;
        }

        public async Task<Player> Get(string username)
        {
            return await _userRepository.Get(username);
        }

        public async Task<IEnumerable<Transaction>> GetAllByReferralCode()
        {
            var getUserInfo = await _userRepository.Get(_httpContextAccessor.HttpContext.User.Identity.Name);
            var getAgentInfo = await _userRepository.GetAgentInfo(getUserInfo.AgentId);
            var getAllPlayer = await _transactionRepository.GetAllByReferralCode(getAgentInfo.ReferralCode);

            var response = new List<Transaction>();

            foreach (var item in getAllPlayer)
            {
                if (item.Role is RoleEnum.PLAYER)
                {
                    item.Commission = 0;
                    item.Percentage = 0;
                }
                else
                {
                    var agentInfo = await _userRepository.GetAgentInfo(item.AgentId);

                    item.Commission = agentInfo.Commission;
                    item.Percentage = agentInfo.Percentage;
                }

                var trans = new Transaction
                {
                    UserId = item.UserId,
                    Username = item.Username,
                    Balance = item.Balance,
                    ContactNumber = item.ContactNumber,
                    Status = item.Status,
                    Role = item.Role,
                    ReferralCode = item.ReferralCode,
                    AgentId = item.AgentId,
                    Commission = item.Commission,
                    Percentage = item.Percentage
                };

                response.Add(trans);
            }

            return response ?? null;
        }

        public async Task<IEnumerable<Player>> GetPendingStatus()
        {
            var getAllUser = await _userRepository.GetAll();
            var getCurrentInfo = await _userRepository.Get(_httpContextAccessor.HttpContext.User.Identity.Name);
            var getAgentInfo = await _userRepository.GetAgentInfo(getCurrentInfo.AgentId);

            if (getCurrentInfo is null)
            {
                return null;
            }
            var response = getAllUser.Where(j => j.ReferralCode.Equals(getAgentInfo.ReferralCode) && j.Status is StatusEnum.Pending);
            return response;
        }

        public async Task<Player> UpdateStatus(Player model)
        {
            var userInfo = await _userRepository.GetById(model.UserId);
            var getCurrentUser = await _userRepository.Get(_httpContextAccessor.HttpContext.User.Identity.Name);
            var getAgentInfo = await _userRepository.GetAgentInfo(getCurrentUser.AgentId);

            if (!userInfo.ReferralCode.Equals(getAgentInfo.ReferralCode))
            {
                _actionContext.ActionContext.ModelState.AddModelError("error", "Unable to approve.");
                return null;
            }

            var player = new Player
            {
                UserId = userInfo.UserId,
                Status = StatusEnum.Approved,
                Role = RoleEnum.PLAYER
            };

            return await _userRepository.UpdatePlayerStatus(player);
        }
    }
}
