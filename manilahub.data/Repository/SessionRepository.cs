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
    public class SessionRepository : ISessionRepository
    {
        private readonly IDbConnection _dbConnection;

        public SessionRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Session>> GetAllUserSession(string userId)
        {
            var sql = @"select * from [dbo].Session where UserId = @userId and IsActive = 1";

            return await _dbConnection.QueryAsync<Session>(sql, new { UserId = userId });
        }

        public async Task<bool> Insert(Session entity)
        {
            var sql = @"insert into [dbo].session 
                                (userid, bearer_token, expiration) 
                        values 
                                (@userid, @bearertoken, @expiration)";

            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(entity);
            parameters.Output(entity, j => j.UserId);

            var returnVal = await _dbConnection.ExecuteAsync(sql, parameters);

            return returnVal is 0 ? false : true;
        }

        public async Task<bool> Logout(Session entity)
        {
            var sql = @"update [dbo].session set IsActive = 0 where SessionId = @SessionId";

            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(entity);

            int rowsAffected = await _dbConnection.ExecuteAsync(sql, parameters);

            return (rowsAffected > 0) ? true : false;
        }

        public async Task<bool> UpdateSession(Session entity)
        {
            var sql = @"update [dbo].session set expiration = cast(@expiration as datetime) where SessionId = @SessionId";

            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(entity);

            int rowsAffected = await _dbConnection.ExecuteAsync(sql, parameters);

            return (rowsAffected > 0) ? true : false;
        }
    }
}
