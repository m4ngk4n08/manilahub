using Dapper;
using manilahub.data.Entity;
using manilahub.data.Repository.IRepository;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace manilahub.data.Repository
{
    public class BetRepository : IBetRepository
    {
        private readonly IDbConnection _dbConnection;

        public BetRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> InsertPlayerBet(Fight entity)
        {
            var sql = @"insert into PlayerBet 
                            (fightid, userid, result, amount, eod) 
                            output inserted.fightid
                        values 
                            (@fightid, @userid, @result, @amount, @eod)";

            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(entity);
            parameters.Output(entity, j => j.FightId);

            var returnVal = await _dbConnection.ExecuteScalarAsync<int>(sql, parameters);
            return returnVal > 0 ? returnVal : 0;
        }

        public async Task<bool> UpdateEod(Fight entity)
        {
            var sql = @"update fight set eod = @eod where fightid = @fightid";

            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(entity);

            var rowsAffected = await _dbConnection.ExecuteAsync(sql, parameters);

            return rowsAffected > 0 ? true : false;
        }

        public async Task<int> InsertFightResult(FightResult entity)
        {
            var sql = @"insert into fightresult
                        (fightid, result, isClosed, eod) 
                            output inserted.fightid
                            values 
                        (@fightid, @result, @isclosed, @eod)";

            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(entity);
            parameters.Output(entity, j => j.FightResultId);

            var returnVal = await _dbConnection.ExecuteScalarAsync<int>(sql, parameters);
            return returnVal > 0 ? returnVal : 0;
        }

        public async Task<bool> UpdateFightResult(FightResult entity)
        {
            var sql = @"update 
                            fightresult 
                        set 
                            result = @result,
                            IsClosed = @isclosed,
                            EOD = @eod
                        where 
                            fightid = @fightid 
                        and 
                            eod = 0";

            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(entity);

            var returnval = await _dbConnection.ExecuteAsync(sql, parameters);

            return returnval > 0 ? true : false;

        }

        public async Task<IEnumerable<Fight>> GetLatestFight()
        {
            var sql = @"select top 3 * from fightresult where eod = 0 order by created desc";

            return await _dbConnection.QueryAsync<Fight>(sql) ?? null;
        }

        public async Task<int> CloseGame()
        {
            var sql = @"update fightresult set eod = 0";
            
            var returnVal = await _dbConnection.ExecuteAsync(sql);
            return returnVal;
        }

        public async Task<int> InsertBettingHistory(BettingHistory entity)
        {
            var sql = @"insert into bettinghistory
                            (userid, fightid, result, amount) 
                        output Inserted.BettingHistoryId
                        values 
                            (@userid, @fightid, @result, @amount)";
            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(entity);
            parameters.Output(entity, j => j.BettingHistoryId);

            var returnVal = await _dbConnection.ExecuteScalarAsync<int>(sql, parameters);
            return returnVal > 0 ? returnVal : 0;
        }

        public async Task<IEnumerable<Fight>> GetAllPlayerBet(Fight entity)
        {
            var sql = @"select * from playerbet where fightid = @FightId and eod = 0";

            return await _dbConnection.QueryAsync<Fight>(sql, entity) ?? null;
        }

        public async Task<double> GetPlayerBetSum(Fight entity)
        {
            var sql = @"select sum(cast(amount as decimal(9,2))) 
                        from 
                            playerbet 
                        where 
                            result = @result 
                        and FightId = @fightid 
                        and eod = 0;
            ";

            return await _dbConnection.ExecuteScalarAsync<double>(sql, entity);
        }
    }
}
