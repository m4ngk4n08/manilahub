using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace manilahub.Area.Tests
{
    [Area("test")]
    [Route("[controller]")]
    public class TestController : Controller
    {
        // GET: TestController
        public ActionResult Index()
        {
            return View();
        }

        // GET: TestController/Details/5
        [Route("{id}/approve")]
        public ActionResult approve(int id)
        {
            return View();
        }
    }
}
