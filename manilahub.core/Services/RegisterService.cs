using manilahub.core.Services.IServices;
using manilahub.data.Entity;
using manilahub.data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace manilahub.core.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IRegisterRepository _registerRepository;
        private readonly ICryptographyService _cryptographyService;
        private readonly IUserRepository _playerRepository;

        public RegisterService(
            IRegisterRepository registerRepository,
            ICryptographyService cryptographyService,
            IUserRepository playerRepository)
        {
            _registerRepository = registerRepository;
            _cryptographyService = cryptographyService;
            _playerRepository = playerRepository;
        }

        public bool Register(Player model)
        {
            try
            {
                var user = _playerRepository.Get(model.Username);

                if (user is null)
                {
                    var newModel = new Player
                    {
                        Username = model.Username,
                        Password = _cryptographyService.SHA512(model.Password),
                        ContactNumber = model.ContactNumber,
                        ReferralCode = model.ReferralCode
                    };

                    return _registerRepository.Insert(newModel);

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
