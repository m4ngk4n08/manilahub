using Dapper;
using manilahub.data.Entity;
using manilahub.data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace manilahub.data.Repository
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly IDbConnection _dbConnection;

        public PlayerRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public Player Get(string username)
        {
            string sql = @"select * from [dbo].users where UPPER(TRIM(username)) = UPPER(TRIM(@Username))";

            return _dbConnection.QueryFirstOrDefault<Player>(sql, new { Username = username });
        }

        public IEnumerable<Player> GetAll()
        {
            var sql = @"select * from [dbo].users";

            var returnVal = _dbConnection.Query<Player>(sql);

            return returnVal ?? null;
        }

        public Player UpdatePlayerStatus(Player entity)
        {
            var sql = @"update [dbo].users
                        set
                            status = @status
                        where
                            UserId = @UserId";

            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(entity);

            int rowsAffected = _dbConnection.Execute(sql, parameters);

            return (rowsAffected > 0) ? entity : null;
        }
    }
}
