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
        private readonly IRegisterRepository _registerRepository;
        private readonly ICryptographyService _cryptographyService;

        public LoginService(
            IRegisterRepository registerRepository,
            ICryptographyService cryptographyService)
        {
            _registerRepository = registerRepository;
            _cryptographyService = cryptographyService;
        }

        public bool Login(Login model)
        {
            var getDetails = _registerRepository.Get(model.Username);

            if (getDetails != null)
            {
                if (_cryptographyService.SHA512(model.Password).Equals(getDetails.Password))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
