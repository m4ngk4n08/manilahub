using manilahub.core.Services.IServices;
using manilahub.data.Entity;
using manilahub.data.Enum;
using manilahub.data.Repository.IRepository;
using Microsoft.AspNetCore.Http;
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

        public PlayerService(
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Player> Get(string username)
        {
            return await _userRepository.Get(username);
        }

        public IEnumerable<Player> GetPendingStatus()
        {
            return _userRepository.GetAll().Where(j => j.Status is StatusEnum.Pending);
        }

        public async Task<Player> UpdateStatus(Player model)
        {
            var userInfo = await _userRepository.Get(model.Username);
            var getCurrentUser = await _userRepository.Get(_httpContextAccessor.HttpContext.User.Identity.Name);

            if (getCurrentUser.ReferralCode.Equals(userInfo.ReferralCode))
            {

                var player = new Player
                {
                    UserId = userInfo.UserId,
                    Status = StatusEnum.Approved,
                    Role = RoleEnum.PLAYER
                };

                return await _userRepository.UpdatePlayerStatus(player);
            }

            return null;
        }
    }
}
