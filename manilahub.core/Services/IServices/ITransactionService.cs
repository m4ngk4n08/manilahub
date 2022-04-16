using manilahub.data.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace manilahub.core.Services.IServices
{
    public interface ITransactionService
    {
        Task<TransactionR> Deposit(TransactionR model);

        Task<TransactionR> Withdrawal(TransactionR model);
    }
}
