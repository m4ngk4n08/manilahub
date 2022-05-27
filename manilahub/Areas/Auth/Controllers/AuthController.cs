using AutoMapper;
using manilahub.Authentication.Model;
using manilahub.core.Services.IServices;
using manilahub.data.Entity;
using manilahub.data.Enum;
using manilahub.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
    [Route("[controller]")]
    [Area("auth")]
    public class AuthController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILoginService _loginService;
        private readonly ISessionService _sessionService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public AuthController(
            IWebHostEnvironment hostingEnvironment,
            ILoginService loginService,
            ISessionService sessionService,
            IHttpContextAccessor httpContext, 
            IMapper mapper)
        {
            _hostingEnvironment = hostingEnvironment;
            _loginService = loginService;
            _sessionService = sessionService;
            _httpContext = httpContext;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index(string returnUrl = null)
        {
            if (_httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            this.ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = "")
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);

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
                var validate = await _loginService.GenerateJwt(map, serverName);
                if (validate != null)
                {
                    if (validate.Role.Equals(RoleEnum.PLAYER))
                    {
                        return RedirectToAction("Index", "Player");
                    }

                    return RedirectToAction("Index", "Dashboard");
                }
            }

            return RedirectToAction("index", "auth");
        }

        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            if (_httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                await _sessionService.Logout(_httpContext.HttpContext.User.Identity.Name);
                await _httpContext.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                foreach (var cookie in _httpContext.HttpContext.Request.Cookies.Keys)
                {
                    _httpContext.HttpContext.Response.Cookies.Delete(cookie);
                }
            }

            return RedirectToAction("index", "auth");
        }

        [Route("accessdenied")]
        public async Task<IActionResult> AccessDenied()
        {
            if (_httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                await _sessionService.Logout(_httpContext.HttpContext.User.Identity.Name);
                await _httpContext.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                foreach (var cookie in _httpContext.HttpContext.Request.Cookies.Keys)
                {
                    _httpContext.HttpContext.Response.Cookies.Delete(cookie);
                }
            }

            return RedirectToAction("index", "auth");
        }
    }
}
