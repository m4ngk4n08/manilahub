using manilahub.core.Services.IServices;
using manilahub.data.Entity;
using manilahub.data.Enum;
using manilahub.Modules.Betting.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace manilahub.Modules.Betting.Controllers
{
    [Route("[controller]")]
    [Authorize("auth")]
    [Area("bet")]
    public class BetController : ControllerBase
    {
        private readonly IBetServices _betServices;

        public BetController(IBetServices betServices)
        {
            _betServices = betServices;
        }

        public IActionResult Index()
        {
            return Ok();
        }

        [HttpPost("Play")]
        public async Task<IActionResult> Betting(BettingModel model)
        {
            //userid bet*meron/wala* amount

            if (ModelState.IsValid)
            {
                var fight = new Fight
                {
                    Result = model.Bet,
                    Amount = model.Amount
                };

                await _betServices.PlayerBet(fight);
                
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.Values.Select(j => j.Errors));
                }

                return Ok();
            }
            return BadRequest(ModelState.Values.Select(j => j.Errors));
        }

        [HttpPost("Start")]
        public async Task<IActionResult> Start()
        {
            if (ModelState.IsValid)
            {
                await _betServices.StartGame();
            }

            return Ok();
        }

        [HttpPost("End")]
        public async Task<IActionResult> End()
        {
            if (ModelState.IsValid)
            {
                await _betServices.EndGame();
            }

            return Ok();
        }

        [HttpPost("winner")]
        public async Task<IActionResult> Declare(BettingModel model)
        {
            if (ModelState.IsValid)
            {
                var fight = new Fight
                {
                    FightId = model.FightId,
                    Result = model.Bet
                };

                await _betServices.DeclareWinner(fight);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.Values.Select(j => j.Errors));
                }

                return Ok();
            }
            return BadRequest();
        }
    }
}
