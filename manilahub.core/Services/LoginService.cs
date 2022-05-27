using manilahub.core.Services.IServices;
using manilahub.data.Entity;
using manilahub.data.Enum;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Core;
using Microsoft.AspNetCore.Http;

namespace manilahub.core.Services
{
    public class LoginService : ILoginService
    {
        private readonly IConfiguration _configuration;
        private readonly IPlayerService _playerService;
        private readonly ICryptographyService _cryptographyService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISessionService _sessionService;

        public LoginService(
            IConfiguration configuration,
            IPlayerService playerService,
            ICryptographyService cryptographyService,
            IHttpContextAccessor httpContextAccessor,
            ISessionService sessionService)
        {
            _configuration = configuration;
            _playerService = playerService;
            _cryptographyService = cryptographyService;
            _httpContextAccessor = httpContextAccessor;
            _sessionService = sessionService;
        }

        public async Task<bool> Login(Login model)
        {
            try
            {
                var getDetails = await _playerService.Get(model.Username);

                if (getDetails != null)
                {
                    if (_cryptographyService.SHA512(model.Password).Equals(getDetails.Password))
                    {
                        if (getDetails.Status != StatusEnum.Pending)
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

        public async Task<Player> GenerateJwt(Login model, string serverName)
        {
            var userInfo = await _playerService.Get(model.Username);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim("ConnectionString", _configuration.GetSection("ConnectionString:APIConnection").Value)
            };
            // Authenticate Cookie
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await _httpContextAccessor.HttpContext.SignInAsync(
                principal,
                 new AuthenticationProperties
                 {
                     ExpiresUtc = DateTime.Now.AddMinutes(20),
                     IsPersistent = true
                 }
            );
            var sessionEntity = new Session
            {
                UserId = userInfo.UserId,
                BearerToken = string.Empty,
                Expiration = DateTime.Now.AddMinutes(20)
            };
            await _sessionService.Insert(sessionEntity);

            return userInfo;
        }
    }
}
