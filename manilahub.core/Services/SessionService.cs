using manilahub.core.Services.IServices;
using manilahub.data.Entity;
using manilahub.data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manilahub.core.Services
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionrepository;
        private readonly IRegisterRepository _registerRepository;
        private readonly IUserRepository _playerRepository;

        public SessionService(
            ISessionRepository sessionrepository,
            IRegisterRepository registerRepository,
            IUserRepository playerRepository)
        {
            _sessionrepository = sessionrepository;
            _registerRepository = registerRepository;
            _playerRepository = playerRepository;
        }

        public async Task<Session> GetUserSession(string userId)
        {
            var returnInfo = await _sessionrepository.GetAllUserSession(userId);
                
            
            return returnInfo.ToList()
                .OrderByDescending(j => j.Expiration)
                .FirstOrDefault();
        }

        public async Task<bool> Insert(Session entity)
        {
            return await _sessionrepository.Insert(entity);
        }

        public async Task<bool> Logout(string username)
        {
            var userDetails = await _playerRepository.Get(username);
            var sessionDeatils = await _sessionrepository.GetAllUserSession(userDetails.UserId.ToString());                
            var session = sessionDeatils.OrderByDescending(j => j.Expiration).FirstOrDefault();
            if (session != null)
            {
                var sessionDTO = new Session
                {
                    SessionId = session.SessionId
                };

                return await _sessionrepository.Logout(sessionDTO);
            }

            return false;
        }
    }
}
