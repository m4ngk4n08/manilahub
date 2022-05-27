using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace manilahub.Areas.User.Controllers
{
    [Authorize("auth")]
    [Route("[controller]")]
    [Area("user")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class TransactionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("Withdrawal")]
        public IActionResult Withdrawal()
        {
            return View();
        }

        [Route("Deposit")]
        public IActionResult Deposit()
        {
            return View();
        }
    }
}
