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
    public class AdminRepository : IAdminRepository
    {
        private readonly IDbConnection _dbConnection;

        public AdminRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> Insert(Agent entity)
        {
            var sql = @"insert into [dbo].agents 
                            (Referral_Code,
                            Percentage) 
                        output Inserted.AgentId
                        values 
                            (@ReferralCode,
                            @Percentage)";
            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(entity);
            parameters.Output(entity, j => j.AgentId);

            var returnVal = await _dbConnection.ExecuteScalarAsync<int>(sql, parameters);

            return returnVal;
        }
    }
}
