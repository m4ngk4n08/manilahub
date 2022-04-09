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

        public RegisterService(
            IRegisterRepository registerRepository,
            ICryptographyService cryptographyService)
        {
            _registerRepository = registerRepository;
            _cryptographyService = cryptographyService;
        }

        public string Get()
        {
            return _registerRepository.Get();
        }

        public bool Register(Register model)
        {
            try
            {
                var user = _registerRepository.Get(model.Username);

                if (user is null)
                {

                    var newModel = new Register
                    {
                        Username = model.Username,
                        Password = _cryptographyService.SHA512(model.Password),
                        ContactNumber = model.ContactNumber
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
