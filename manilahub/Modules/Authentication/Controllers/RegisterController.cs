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
    [ApiController]
    [Route("[controller]")]
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
        public IActionResult Index()
        {
            _playerService.Get(string.Empty);
            return Content("");
        }

        [HttpPost]
        public IActionResult Register([FromBody]RegisterModel model,[FromQuery] string refId)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (refId == string.Empty)
            {
                return BadRequest("Referral code cannot be empty");
            }

            model.ReferralCode = refId;

            var map = _mapper.Map<Player>(model);

            if (_registerService.Register(map))
            {
                return new JsonResult("User Added Succesfuly");
            }

            return BadRequest();
        }

    }
}
