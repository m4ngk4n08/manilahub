using AutoMapper;
using manilahub.core.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace manilahub.Modules.Admin.Controllers
{
    [Authorize("auth")]
    [Route("[controller]")]
    [Area("admin")]
    public class AdminController : Controller
    {
        private readonly IAdminServices _adminServices;
        private readonly IMapper _mapper;

        public AdminController(
            IAdminServices adminServices,
            IMapper mapper)
        {
            _adminServices = adminServices;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
