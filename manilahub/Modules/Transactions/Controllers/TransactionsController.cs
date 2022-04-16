using AutoMapper;
using manilahub.core.Services.IServices;
using manilahub.data.Entity;
using manilahub.Middleware;
using manilahub.Modules.Transactions.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace manilahub.Modules.Transactions.Controllers
{
    [ApiController]
    [HubMiddleware]
    [Route("[controller]")]
    public class TransactionsController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;

        public TransactionsController(
            ITransactionService transactionService,
            IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Withdrawal")]
        public async Task<IActionResult> Withdrawal(TransactionModel model)
        {
            if (ModelState.IsValid)
            {
                var transMap = _mapper.Map<TransactionR>(model);

                var trans = await _transactionService.Withdrawal(transMap);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.Values.Select(j => j.Errors));
                }

                return Ok();
            }

            return BadRequest(ModelState.Values.Select(j => j.Errors));
        }

        [HttpPost("Deposit")]
        public async Task<IActionResult> Deposit(TransactionModel model)
        {
            //id type amount remarks
            if (ModelState.IsValid)
            {
                var transMap = _mapper.Map<TransactionR>(model);

                var trans = await _transactionService.Deposit(transMap);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.Values.Select(j => j.Errors));
                }

                return Ok();
            }

            return BadRequest(ModelState.Values.Select(j => j.Errors));
        }
    }
}
