using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace manilahub.Modules.Dashboard.Controllers
{
    [Authorize("auth")]
    [Route("[controller]")]
    [Area("dashboard")]
    public class DashboardController : Controller
    {
        public DashboardController()
        {

        }
        public IActionResult Index()
        {
            return View();
        }

    }
}
