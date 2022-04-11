using AutoMapper;
using manilahub.core.Services.IServices;
using manilahub.data.Entity;
using Microsoft.AspNetCore.Mvc;

namespace manilahub.Modules
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : Controller
    {
        private readonly IPlayerService _playerService;
        private readonly IMapper _mapper;

        public StatusController(
            IPlayerService playerService,
            IMapper mapper)
        {
            _playerService = playerService;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return new JsonResult(_playerService.GetPendingStatus());
        }

        [HttpPost]
        public IActionResult UpdateStatus(PlayerModel model)
        {
            var map = _mapper.Map<Player>(model);

            _playerService.UpdateStatus(map);
            return new JsonResult("Status Succesfuly updated");
        }
    }
}
