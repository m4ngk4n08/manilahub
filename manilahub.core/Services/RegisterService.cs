using manilahub.core.Services.IServices;
using manilahub.data.Entity;
using manilahub.data.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace manilahub.core.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IRegisterRepository _registerRepository;
        private readonly ICryptographyService _cryptographyService;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IActionContextAccessor _actionContext;

        public RegisterService(
            IRegisterRepository registerRepository,
            ICryptographyService cryptographyService,
            IUserRepository userRepository,
            IHttpContextAccessor httpContext,
            IActionContextAccessor actionContext)
        {
            _registerRepository = registerRepository;
            _cryptographyService = cryptographyService;
            _userRepository = userRepository;
            _httpContext = httpContext;
            _actionContext = actionContext;
        }

        public async Task<bool> Register(Player model)
        {
            try
            {
                var user = await _userRepository.Get(model.Username);
                var getReferralCode = await _userRepository.Get(_httpContext.HttpContext.User.Identity.Name);
                var getAgentInfo = await _userRepository.GetAgentInfo(getReferralCode.AgentId);
                var getAllUser = await _userRepository.GetAll();

                if (getAllUser.ToList().Select(j => j.Username).Contains(model.Username))
                {
                    _actionContext.ActionContext.ModelState.AddModelError("error", "username already exist");
                    return false;
                }

                //if (getAllUser.ToList().Select(j => j.ContactNumber).Contains(model.ContactNumber))
                //{
                //    _actionContext.ActionContext.ModelState.AddModelError("error", "contact number already exist");
                //    return false;
                //}

                if (user is null || getReferralCode is null)
                {
                    var newModel = new Player
                    {
                        Username = model.Username,
                        Password = _cryptographyService.SHA512(model.Password),
                        ContactNumber = model.ContactNumber,
                        ReferralCode = getAgentInfo.ReferralCode
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
