using Dapper;
using manilahub.data.Entity;
using manilahub.data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace manilahub.data.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IDbConnection _dbConnection;

        public TransactionRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Transaction>> GetAllByReferralCode(string referralCode)
        {
            var sql = @"select a.* from users a 
                        where a.Referral_Code = @Referral_Code and a.Status != 1";

            return await _dbConnection.QueryAsync<Transaction>(sql, new { Referral_Code = referralCode });
        }

        public async Task<Transaction> GetByReferralCode(string referralCode)
        {
            var sql = @"select a.UserId, a.Username, a.Referral_Code, a.balance, b.agentid, b.commission, b.referral_code
                        from users a inner join agents b 
                        on a.Agent_Id = b.AgentId
                        where b.Referral_Code = @Referral_Code and a.Status != 1";
            var returnVal = await _dbConnection.QueryFirstOrDefaultAsync<Transaction>(sql, new { Referral_Code = referralCode }) ?? null;

            return returnVal;
        }

        public async Task<bool> UpdateBalance(Transaction entity)
        {
            var sql = @"update users 
                        set 
                            balance = @balance 
                        where 
                            userid = @userid";

            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(entity);

            int rowsAffected = await _dbConnection.ExecuteAsync(sql, parameters);

            return (rowsAffected > 0) ? true : false;
        }

        public async Task<bool> Insert(TransactionR entity)
        {
            var sql = @"insert into transactions 
                                    (userid, agentid, type, amount, remarks) 
                                values 
                                    (@userid, @agentid, @type, @amount, @remarks)";

            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(entity);

            int rowsAffected = await _dbConnection.ExecuteAsync(sql, parameters);

            return (rowsAffected > 0) ? true : false;
        }
    }
}
