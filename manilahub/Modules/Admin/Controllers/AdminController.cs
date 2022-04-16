using AutoMapper;
using manilahub.core.Services.IServices;
using manilahub.data.Entity;
using manilahub.Modules.Admin.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace manilahub.Modules.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
