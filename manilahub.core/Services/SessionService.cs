using manilahub.core.Services.IServices;
using manilahub.data.Entity;
using manilahub.data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace manilahub.core.Services
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionrepository;
        private readonly IRegisterRepository _registerRepository;
        private readonly IPlayerRepository _playerRepository;

        public SessionService(
            ISessionRepository sessionrepository,
            IRegisterRepository registerRepository,
            IPlayerRepository playerRepository)
        {
            _sessionrepository = sessionrepository;
            _registerRepository = registerRepository;
            _playerRepository = playerRepository;
        }

        public Session GetUserSession(string userId)
        {
            var returnInfo = _sessionrepository.GetAllUserSession(userId).OrderByDescending(j => j.Expiration).FirstOrDefault();
            return returnInfo;
        }

        public bool Insert(Session entity)
        {
            return _sessionrepository.Insert(entity);
        }

        public bool Logout(string username)
        {
            var userDetails = _playerRepository.Get(username);
            var sessionDeatils = _sessionrepository.GetAllUserSession(userDetails.UserId.ToString()).OrderByDescending(j => j.Expiration).FirstOrDefault();

            if (sessionDeatils != null)
            {
                var sessionDTO = new Session
                {
                    SessionId = sessionDeatils.SessionId
                };

                return _sessionrepository.Logout(sessionDTO);
            }

            return false;
        }
    }
}
