using manilahub.core.Services.IServices;
using manilahub.data.Entity;
using manilahub.data.Enum;
using manilahub.data.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace manilahub.core.Services
{
	public class BetServices : IBetServices
	{
		private readonly IBetRepository _betRepository;
		private readonly ITransactionRepository _transactionRepository;
		private readonly IActionContextAccessor _actionContext;
		private readonly IUserRepository _userRepository;

		public BetServices(
			IBetRepository betRepository,
			ITransactionRepository transactionRepository,
			IActionContextAccessor actionContext,
			IUserRepository userRepository)
		{
			_betRepository = betRepository;
			_transactionRepository = transactionRepository;
			_actionContext = actionContext;
			_userRepository = userRepository;
		}

		public async Task<IEnumerable<Fight>> GetAllPlayerBet()
        {
			var fight = new Fight
			{
				FightId = 1
			};
			return await _betRepository.GetAllPlayerBet(fight);
        }
		public async Task<int> StartGame()
		{
			// get latest fight if 0 count starts to 1
			// implement by 1
			var getAllFightResult = await _betRepository.GetLatestFight();
			var fightResult = getAllFightResult.ToList().FirstOrDefault();
			var fight = new FightResult();

			if (fightResult is null)
			{
				fight = new FightResult
				{
					FightId = 1
				};

				await _betRepository.InsertFightResult(fight);
			}
			else
			{
				fight = new FightResult
				{
					FightId = fightResult.FightId + 1
				};

				await _betRepository.InsertFightResult(fight);
			}

			return fight.FightId;
		}

		public async Task<bool> EndGame()
		{
			var getAllFightResult = await _betRepository.GetLatestFight();
			var fightResult = getAllFightResult.ToList().FirstOrDefault();
			// update fighresult

			var fight = new FightResult
			{
				FightId = fightResult.FightId,
				IsClosed = true
			};

			await _betRepository.UpdateFightResult(fight);

			return true;
		}

		public async Task<bool> CloseGame()
		{
			await _betRepository.CloseGame();

			return true;
		}

		public async Task<ResultEnum> DeclareWinner(Fight model)
		{
			try
			{
				// accept fight id and winner
				// get all users that has fight result
				// calculate base on payout
				// get amount sum of meron and wala
				var getAllUserBasedOnFightId = await _betRepository.GetAllPlayerBet(model);

				//accept fightid

				//calculate all betters based on payout
				// win / lose / draw / cancelled

				// dehado = mababapayout
				// llamado = pataas payout
				var meron = new Fight
				{
					FightId = model.FightId,
					Result = ResultEnum.Meron
				};

				var totalMeron = await _betRepository.GetPlayerBetSum(meron);

				var wala = new Fight
				{
					FightId = model.FightId,
					Result = ResultEnum.Wala
				};

				var totalWala = await _betRepository.GetPlayerBetSum(wala);

				var percentage = CalculatePayout(model, totalMeron, totalWala);

				foreach (var item in getAllUserBasedOnFightId)
				{                    
					// condition if winner else lose
					switch (item.Result)
					{
						case ResultEnum.Meron:
							await CalculatePlayerBet(model, item, percentage, totalMeron, totalWala);
							break;
						case ResultEnum.Wala:
							await CalculatePlayerBet(model, item, percentage, totalWala, totalMeron);
							break;
						case ResultEnum.Draw:
							break;
						case ResultEnum.Cancelled:
							break;
						default:
							break;
					}
				}

				// update fight result
				var fightResult = new FightResult
				{
					FightId = model.FightId,
					Result = model.Result,
					IsClosed = true
				};

				await _betRepository.UpdateFightResult(fightResult);
				// return winner
				return model.Result;
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}

		async Task CalculatePlayerBet(Fight model, Fight item, Tuple<double, double> percentage, double totalMeron, double totalWala)
		{
			var getUserInfo = await _userRepository.GetById(item.UserId);
			var updtBalance = new Transaction();
			var getBet = new Fight();
			double balance;

			if (item.Result.Equals(model.Result))
			{
				// Player winner
				var max = Max(totalMeron, totalWala);
				var min = Min(totalMeron, totalWala);
				switch (item.Result)
				{
					case ResultEnum.Meron:
						if (totalMeron > totalWala)
						{
							// dehado

							var dehado = percentage.Item2 - 100;

							balance = getUserInfo.Balance + (item.Amount * ((percentage.Item2 - 100) * 0.01));

							updtBalance = new Transaction
							{
								UserId = item.UserId,
								Balance = balance
							};
							//payout calculation
							// balance - getuserinfo.balance = result + amount = payout
                        }	
                        else
                        {
							// llamado
							var llamado = percentage.Item1 - 100;
							balance = getUserInfo.Balance + (item.Amount * ((percentage.Item1 - 100) * 0.01));

							updtBalance = new Transaction
							{
								UserId = item.UserId,
								Balance = balance
							};
						}
						break;
					case ResultEnum.Wala:
						if (totalWala > totalMeron)
						{
							// dehado

							var dehado = percentage.Item2 - 100;

							balance = getUserInfo.Balance + (item.Amount * ((percentage.Item2 - 100) * 0.01));

							updtBalance = new Transaction
							{
								UserId = item.UserId,
								Balance = balance
							};
						}
						else
						{
							// llamado
							var llamado = percentage.Item1 - 100;
							balance = getUserInfo.Balance + (item.Amount * ((percentage.Item1 - 100) * 0.01));

							updtBalance = new Transaction
							{
								UserId = item.UserId,
								Balance = balance
							};
						}
						break;
					case ResultEnum.Draw:
						break;
					case ResultEnum.Cancelled:
						break;
					default:
						break;
				}
				// update balance
				//balance and userid
				await _transactionRepository.UpdateBalance(updtBalance);
			}
			else
			{
				// lose player
			}

			// insert bethistory
			var betHistory = new BettingHistory
			{
				UserId = getUserInfo.UserId,
				FightId = model.FightId,
				Result = model.Result,
				Amount = item.Amount
			};

			await _betRepository.InsertBettingHistory(betHistory);
		}

		public async Task<bool> PlayerBet(Fight model)
		{
			//get userinfo
			//get agent info
			var userInfo = await _userRepository.Get(_actionContext.ActionContext.HttpContext.User.Identity.Name);

			if (userInfo is null)
			{
				_actionContext.ActionContext.ModelState.AddModelError("error", "undefined user");
				return false;
			}

			if (userInfo.Balance < model.Amount)
			{
				_actionContext.ActionContext.ModelState.AddModelError("error", "Not enough balance.");
				return false;
			}

			//get master and subop if upline agent is gold
			//split commission
			var agentInfo = await _transactionRepository.GetByReferralCode(userInfo.ReferralCode);

			var agentRole = await _userRepository.Get(agentInfo.Username);

			if (agentInfo is null)
			{
				_actionContext.ActionContext.ModelState.AddModelError("error", "undefined user");
				return false;
			}

			switch (agentRole.Role)
			{
				case RoleEnum.GOLD:
					// Master
					var masterInfo = await _transactionRepository.GetByReferralCode(agentRole.ReferralCode);
					var masterRole = await _userRepository.Get(masterInfo.Username);

					// SubOperator
					var subOpInfo = await _transactionRepository.GetByReferralCode(masterRole.ReferralCode);
					var subOpRole = await _userRepository.Get(subOpInfo.Username);

					// Admin
					var adminInfo = await _transactionRepository.GetByReferralCode(subOpRole.ReferralCode);

					// calculate
					var goldComm = model.Amount * 0.02;
					var masterComm = model.Amount * 0.01;
					var subOpComm = model.Amount * 0.01;
					var adminComm = model.Amount * 0.08;

					// update Commission
					await UpdateComm(agentInfo.AgentId, agentInfo.Commission + goldComm);
					await UpdateComm(masterInfo.AgentId, masterInfo.Commission + masterComm);
					await UpdateComm(subOpInfo.AgentId, subOpInfo.Commission + subOpComm);
					await UpdateComm(adminInfo.AgentId, adminInfo.Commission + adminComm);
					break;
				case RoleEnum.MASTER:
					var subOpM = await _transactionRepository.GetByReferralCode(agentRole.ReferralCode);
					var subOpRoleM = await _userRepository.Get(subOpM.Username);

					var adminInfoM = await _transactionRepository.GetByReferralCode(subOpRoleM.ReferralCode);

					var masterCommM = model.Amount * 0.03;
					var subOpCommM = model.Amount * 0.01;
					var adminCommM = model.Amount * 0.08;

					await UpdateComm(agentInfo.AgentId, agentInfo.Commission + masterCommM);
					await UpdateComm(subOpM.AgentId, subOpM.Commission + subOpCommM);
					await UpdateComm(adminInfoM.AgentId, adminInfoM.Commission + adminCommM);
					break;
				case RoleEnum.OPERATOR:
					var adminInfoO = await _transactionRepository.GetByReferralCode(agentRole.ReferralCode);

					var subCommO = model.Amount * 0.04;
					var adminCommO = model.Amount * 0.08;

					await UpdateComm(agentInfo.AgentId, agentInfo.Commission + subCommO);
					await UpdateComm(adminInfoO.AgentId, adminInfoO.Commission + adminCommO);
					break;
				case RoleEnum.ADMIN:
					var adminInfoA = await _transactionRepository.GetByReferralCode(agentInfo.ReferralCode);

					var adminCommA = model.Amount * 0.12;

					await UpdateComm(adminInfoA.AgentId, adminInfoA.Commission + adminCommA);
					break;
				case RoleEnum.PLAYER:
					break;
				default:
					break;
			}

			// get current fight result id
			var fightId = _betRepository.GetLatestFight().Result.ToList().FirstOrDefault();

			// insert fight
			var fight = new Fight
			{
				FightId = fightId.FightId,
				UserId = userInfo.UserId,
				Result = model.Result,
				Amount = model.Amount,
				EOD = false
			};

			await _betRepository.InsertPlayerBet(fight);

			// update player balance
			//balance and userid
			var updtBalance = new Transaction
			{
				UserId = userInfo.UserId,
				Balance = Max(model.Amount, userInfo.Balance) - Min(model.Amount, userInfo.Balance)
			};

			await _transactionRepository.UpdateBalance(updtBalance);

			// insert bettinghistory
			var bettingHistory = new BettingHistory
			{
				UserId = userInfo.UserId,
				FightId = fightId.FightId,
				Result = model.Result,
				Amount = model.Amount
			};

			await _betRepository.InsertBettingHistory(bettingHistory);

			return true;
		}

		async Task<Agent> UpdateComm(int agentId, double commission)
		{
			var agent = new Agent
			{
				AgentId = agentId,
				Commission = commission
			};

			return await _userRepository.UpdateCommission(agent);
		}

		Tuple<double, double> CalculatePayout(Fight model, double totalMeron, double totalWala)
		{
			// result and betid

			var alg1 = 0.2;
			var alg2 = 0.4;
			var alg3 = /*225.6;*/ 224;
			var alg4 = /*225.6;*/ 223;
			double llamado;
			double dehado;

			var meron1 = totalMeron - (totalMeron * alg1);
			var meron2 = totalWala - (totalWala * alg1);
			llamado = Max(meron1, meron2);
			dehado = Min(meron1, meron2);

			var total = meron1 + meron2 - ((meron1 + meron2) * alg2);

			var percent1 = (llamado / total) * alg3;
			var percent2 = (dehado / total) * alg4;
				   

			return new Tuple<double, double>(Max(percent1, percent2), Min(percent1, percent2));
		}

		double Max(params double[] input)
		{
			return input.Max();
		}
		double Min(params double[] input)
		{
			return input.Min();
		}
	}
}
