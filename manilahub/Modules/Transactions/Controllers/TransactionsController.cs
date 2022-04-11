using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace manilahub.Modules.Transactions.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Withdrawal()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult Deposit()
        {
            throw new NotImplementedException();
        }
    }

    public class TransactionModel
    {
        public int TransactionId { get; set; }
        public int AgentId { get; set; }
        public int UserId { get; set; }
        public string Amount { get; set; }


    }
}
