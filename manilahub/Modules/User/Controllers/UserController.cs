using AutoMapper;
using manilahub.core.Services.IServices;
using manilahub.data.Entity;
using manilahub.Middleware;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace manilahub.Modules
{
    [ApiController]
    [HubMiddleware]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IPlayerService _playerService;
        private readonly IAdminServices _adminServices;
        private readonly IMapper _mapper;

        public UserController(
            IPlayerService playerService,
            IAdminServices adminServices,
            IMapper mapper)
        {
            _playerService = playerService;
            _adminServices = adminServices;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return new JsonResult(_playerService.GetPendingStatus());
        }

        [HttpPost("Approve")]
        public async Task<IActionResult> UpdateStatus(PlayerModel model)
        {
            // accept username
            if (ModelState.IsValid)
            {
                var map = _mapper.Map<Player>(model);

                if (await _playerService.UpdateStatus(map) != null)
                {
                    return new JsonResult("Status Succesfuly updated");
                }
            }

            return BadRequest();
        }

        [HttpPost("Promote-Player")]
        public async Task<IActionResult> PromotePlayer([FromBody] PlayerModel model)
        {
            if (ModelState.IsValid)
            {
                var map = _mapper.Map<Player>(model);

                //accept username and role
                if (await _adminServices.Insert(map))
                {
                    return Ok();
                }
            }

            return BadRequest();
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
