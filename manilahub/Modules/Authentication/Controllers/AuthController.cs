using AutoMapper;
using manilahub.Authentication.Model;
using manilahub.core.Services.IServices;
using manilahub.data.Entity;
using manilahub.Middleware;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace manilahub.Authentication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILoginService _loginService;
        private readonly ISessionService _sessionService;
        private readonly IPlayerService _playerService;
        private readonly IMapper _mapper;

        public AuthController(
            IWebHostEnvironment hostingEnvironment,
            ILoginService loginService,
            ISessionService sessionService,
            IPlayerService playerService,
            IMapper mapper)
        {
            _hostingEnvironment = hostingEnvironment;
            _loginService = loginService;
            _sessionService = sessionService;
            _playerService = playerService;
            _mapper = mapper;
        }


        [HubMiddleware]
        public IActionResult Index()
        {
            return Content("hi im login");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginModel model)
        {
            if (model is null)
            {
                return BadRequest("Invalid client request.");

            }
            var map = _mapper.Map<Login>(model);


            if (await _loginService.Login(map))
            {
                var serverName = string.Empty;
                if (_hostingEnvironment.IsDevelopment())
                {
                    serverName = "https://localhost:5001";
                }
                else
                {
                    serverName = "http://www.manilahub.somee.com";
                }

                var userInfo = await _playerService.Get(model.Username);
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKey@368123"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: serverName,
                    audience: serverName,
                    claims: new List<Claim>() 
                    { 
                        new Claim(ClaimTypes.Name, model.Username)
                    },
                    expires: DateTime.Now.AddMinutes(20),
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                var sessionEntity = new Session
                {
                    UserId = userInfo.UserId,
                    BearerToken = tokenString,
                    Expiration = DateTime.Now.AddMinutes(20)
                };
                await _sessionService.Insert(sessionEntity);

                return Ok(new { Token = tokenString });
            }

            return Unauthorized();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(string username)
        {
            await _sessionService.Logout(username);

            return Ok();
        }
    }
}
