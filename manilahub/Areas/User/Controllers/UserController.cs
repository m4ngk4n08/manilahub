using AutoMapper;
using manilahub.core.Services.IServices;
using manilahub.data.Entity;
using manilahub.data.Enum;
using manilahub.Modules.User.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace manilahub.Modules
{
    [Authorize("auth")]
    [Route("[controller]")]
    [Area("user")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class UserController : Controller
    {
        private readonly IPlayerService _playerService;
        private readonly IAdminServices _adminServices;
        private readonly IBetServices _betServices;
        private readonly IMapper _mapper;

        public UserController(
            IPlayerService playerService,
            IAdminServices adminServices,
            IBetServices betServices,
            IMapper mapper)
        {
            _playerService = playerService;
            _adminServices = adminServices;
            _betServices = betServices;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var getUserList = await _playerService.GetPendingStatus();
            var fightList = new List<FightModel>();
            foreach (var item in getUserList)
            {
                var fight = new FightModel
                {
                    UserId = item.UserId,
                    Username = item.Username,
                    Status = item.Status,
                    ContactNumber = item.ContactNumber
                };

                fightList.Add(fight);
            }

            return View(fightList);
        }

        [HttpGet("approved")]
        public IActionResult Approved()
        {
            return View();
        }

        [HttpGet("get-approved")]
        public async Task<IActionResult> GetApprovedPlayer()
        {
            var returnVal = await _playerService.GetAllByReferralCode();
            returnVal.Where(j => j.Role.Equals(RoleEnum.PLAYER)).ToList().ForEach(j =>
            {
                j.Commission = 0;
            });
            return Json(returnVal);
        }

        [HttpGet("userlist")]
        public async Task<IActionResult> UserList()
        {
            var getUserList = await _playerService.GetPendingStatus();

            return Json(getUserList);
        }

        [HttpPost("{id}/{action}")]
        public async Task<IActionResult> approve(int id)
        {
            // accept username
            if (ModelState.IsValid)
            {
                var getUserList = await _playerService.GetPendingStatus();
                var fightList = new List<FightModel>();
                foreach (var item in getUserList)
                {
                    var fight = new FightModel
                    {
                        UserId = item.UserId,
                        Username = item.Username,
                        Status = item.Status,
                        ContactNumber = item.ContactNumber
                    };

                    fightList.Add(fight);
                }

                var player = new Player
                {
                    UserId = id
                };
                var map = _mapper.Map<Player>(player);

                if (await _playerService.UpdateStatus(map) != null)
                {
                    ViewBag.Success = true;
                    return View("Index", fightList);
                }

                if (!ModelState.IsValid)
                {
                    var test = ModelState.Values.Select(j => j.Errors).FirstOrDefault();
                    ViewBag.ErrorMessage = test.Select(j => j.ErrorMessage).First();
                    ViewBag.Success = false;
                    return View("Index", fightList);
                }
            }

            return BadRequest();
        }


        [HttpPost("promote")]
        public async Task<IActionResult> PromotePlayer([FromBody]PlayerModel model)
        {
            if (ModelState.IsValid)
            {
                var map = _mapper.Map<Player>(model);

                //accept username and role
                if (await _adminServices.Promote(map))
                {
                }
            }
            if (!ModelState.IsValid)
            {
                var test = ModelState.Values.Select(j => j.Errors).FirstOrDefault();
                return BadRequest(test.Select(j => j.ErrorMessage).First());
            }
            return Ok();
        }

        [HttpPost("Promote-Agent")]
        public async Task<IActionResult> PromoteAgent([FromBody]PlayerModel model)
        {
            //accept userid and role
            if (ModelState.IsValid)
            {
                var map = _mapper.Map<Player>(model);

                if (await _adminServices.UpdateRole(map))
                {
                    return Ok();
                }
            }

            return BadRequest();
        }
    }
}
