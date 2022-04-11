using manilahub.core.Services.IServices;
using manilahub.data.Entity;
using manilahub.data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace manilahub.core.Services
{
    public class LoginService : ILoginService
    {
        private readonly IPlayerService _playerService;
        private readonly ICryptographyService _cryptographyService;

        public LoginService(
            IPlayerService playerService,
            ICryptographyService cryptographyService)
        {
            _playerService = playerService;
            _cryptographyService = cryptographyService;
        }

        public bool Login(Login model)
        {
            try
            {
                var getDetails = _playerService.Get(model.Username);

                if (getDetails != null)
                {
                    if (_cryptographyService.SHA512(model.Password).Equals(getDetails.Password))
                    {
                        if (getDetails.Status != 1)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
