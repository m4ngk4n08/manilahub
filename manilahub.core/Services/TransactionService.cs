using manilahub.core.Services.IServices;
using manilahub.data.Entity;
using manilahub.data.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace manilahub.core.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IActionContextAccessor _actionContext;
        private readonly IUserRepository _userRepository;

        public TransactionService(
            ITransactionRepository transactionRepository,
            IActionContextAccessor actionContext,
            IUserRepository userRepository)
        {
            _transactionRepository = transactionRepository;
            _actionContext = actionContext;
            _userRepository = userRepository;
        }


        public async Task<TransactionR> Withdrawal(TransactionR model)
        {
            var userInfo = await _userRepository.GetById(model.UserId.ToString());
            var agentInfo = await _transactionRepository.GetByReferralCode(userInfo.ReferralCode is null ? null : userInfo.ReferralCode);

            if (userInfo != null || agentInfo != null)
            {
                if (agentInfo.ReferralCode.Equals(userInfo.ReferralCode))
                {
                    if (userInfo.Balance >= model.Amount)
                    {
                        //update user balance
                        var userTrans = new Transaction
                        {
                            UserId = model.UserId,
                            Balance = userInfo.Balance - model.Amount
                        };
                        await _transactionRepository.UpdateBalance(userTrans);

                        //update agent balance
                        var agentTrans = new Transaction
                        {
                            UserId = agentInfo.UserId,
                            Balance = agentInfo.Balance + model.Amount
                        };
                        await _transactionRepository.UpdateBalance(agentTrans);

                        //insert transaction
                        var trans = new TransactionR
                        {
                            UserId = model.UserId,
                            AgentId = Convert.ToInt32(agentInfo.AgentId),
                            Amount = model.Amount,
                            Type = model.Type,
                            Remarks = model.Remarks
                        };

                        await _transactionRepository.Insert(trans);

                        return trans;
                    }
                    else
                    {
                        _actionContext.ActionContext.ModelState.AddModelError("error", "Not enough balance.");
                    }
                }
                else
                {
                    _actionContext.ActionContext.ModelState.AddModelError("error", "Referral code not match");
                }
            }

            return null;
        }

        public async Task<TransactionR> Deposit(TransactionR model)
        {
            var userInfo = await _userRepository.GetById(model.UserId.ToString());
            var agentInfo = await _transactionRepository.GetByReferralCode(userInfo.ReferralCode is null ? null : userInfo.ReferralCode);

            if (userInfo != null || agentInfo != null)
            {
                if (agentInfo.ReferralCode.Equals(userInfo.ReferralCode))
                {
                    if (agentInfo.Balance > model.Amount)
                    {
                        //update user balance
                        var userTrans = new Transaction
                        {
                            UserId = model.UserId,
                            Balance = userInfo.Balance + model.Amount
                        };
                        await _transactionRepository.UpdateBalance(userTrans);

                        //update agent balance
                        var agentTrans = new Transaction
                        {
                            UserId = agentInfo.UserId,
                            Balance = agentInfo.Balance - model.Amount
                        };
                        await _transactionRepository.UpdateBalance(agentTrans);

                        //insert transaction
                        var trans = new TransactionR
                        {
                            UserId = model.UserId,
                            AgentId = Convert.ToInt32(agentInfo.AgentId),
                            Amount = model.Amount,
                            Type = model.Type,
                            Remarks = model.Remarks
                        };

                        await _transactionRepository.Insert(trans);

                        return trans;
                    }
                    else
                    {
                        _actionContext.ActionContext.ModelState.AddModelError("error", "Not enough balance.");
                    }
                }
                else
                {
                    _actionContext.ActionContext.ModelState.AddModelError("error", "Referral code not match");
                }
            }

            return null;
        }
    }
}
