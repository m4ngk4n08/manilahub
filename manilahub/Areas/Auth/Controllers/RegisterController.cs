using AutoMapper;
using manilahub.Authentication.Model;
using manilahub.core.Services.IServices;
using manilahub.data.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace manilahub.Authentication.Controllers
{
    [Route("[controller]")]
    [Area("auth")]
    public class RegisterController : Controller
    {
        private readonly IRegisterService _registerService;
        private readonly IPlayerService _playerService;
        private readonly IMapper _mapper;

        public RegisterController(
            IRegisterService registerService,
            IPlayerService playerService,
            IMapper mapper)
        {
            _registerService = registerService;
            _playerService = playerService;
            _mapper = mapper;
        }

        [Route("add")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("add")]
        public async Task<IActionResult> Register(RegisterModel model)
        {

            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            var map = _mapper.Map<Player>(model);

            if (await _registerService.Register(map))
            {
                ViewBag.Success = true;
                return View("Index");
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    var test = ModelState.Values.Select(j => j.Errors).FirstOrDefault();
                    ViewBag.ErrorMessage = test.Select(j => j.ErrorMessage).First();
                    ViewBag.Success = false;
                    return View("Index");
                }
            }

            return View("Index");
        }

    }
}
