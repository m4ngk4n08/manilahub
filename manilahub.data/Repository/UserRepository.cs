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
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _dbConnection;

        public UserRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Player> Get(string username)
        {
            string sql = @"select * from [dbo].users where UPPER(TRIM(username)) = UPPER(TRIM(@Username))";

            var returnVal =  await _dbConnection.QueryFirstOrDefaultAsync<Player>(sql, new { Username = username });

            return returnVal ?? null;
        }

        public async Task<Player> GetById(int userid)
        {
            string sql = @"select * from [dbo].users where userid = @userid";

            var returnVal = await _dbConnection.QueryFirstOrDefaultAsync<Player>(sql, new { userid = userid });

            return returnVal ?? null;
        }

        public async Task<Agent> GetAgentInfo(int agentid)
        {
            string sql = @"select * from [dbo].agents where agentid = @agentid";

            var returnval = await _dbConnection.QueryFirstOrDefaultAsync<Agent>(sql, new { agentid = agentid });

            return returnval ?? null;
        }

        public async Task<IEnumerable<Player>> GetAll()
        {
            var sql = @"select * from [dbo].users";

            var returnVal = await _dbConnection.QueryAsync<Player>(sql);

            return returnVal ?? null;
        }

        public async Task<Player> UpdatePlayerStatus(Player entity)
        {
            var sql = @"update [dbo].users
                        set
                            agent_id = @agentid,
                            role = @role,
                            status = @status
                        where
                            UserId = @UserId";

            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(entity);

            int rowsAffected = await _dbConnection.ExecuteAsync(sql, parameters);

            return (rowsAffected > 0) ? entity : null;
        }

        public async Task<Agent> UpdateCommission(Agent entity)
        {
            var sql = @"update agents set commission = @commission where agentid = @agentid";

            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(entity);

            int rowsAffected = await _dbConnection.ExecuteAsync(sql, parameters);

            return rowsAffected > 0 ? entity : null;
        }
    }
}
