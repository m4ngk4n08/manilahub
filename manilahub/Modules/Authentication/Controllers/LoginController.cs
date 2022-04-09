using AutoMapper;
using manilahub.Authentication.Model;
using manilahub.core.Services.IServices;
using manilahub.data.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace manilahub.Authentication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly IMapper _mapper;

        public LoginController(
            ILoginService loginService,
            IMapper mapper)
        {
            _loginService = loginService;
            _mapper = mapper;
        }
        [Authorize]
        public IActionResult Index()
        {
            return Content("hi im login");
        }

        [HttpPost]
        public IActionResult Login([FromBody]LoginModel model)
        {
            if (model is null)
            {
                return BadRequest("Invalid client request.");

            }
            var map = _mapper.Map<Login>(model);


            if (_loginService.Login(map))
            {
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKey@368123"));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var tokeOptions = new JwtSecurityToken(
                        issuer: "https://localhost:5001",
                        audience: "https://localhost:5001",
                        claims: new List<Claim>(),
                        expires: DateTime.Now.AddMinutes(5),
                        signingCredentials: signinCredentials
                    );
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                    return Ok(new { Token = tokenString });
            }

            return Unauthorized();
        }
    }
}
