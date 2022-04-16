using manilahub.data.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace manilahub.data.Repository.IRepository
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetAllByReferralCode(string referralCode);

        Task<Transaction> GetByReferralCode(string referralCode);

        Task<bool> UpdateBalance(Transaction entity);
        Task<bool> Insert(TransactionR entity);
    }
}
