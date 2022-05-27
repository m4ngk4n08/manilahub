using manilahub.data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace manilahub.data.Repository.IRepository
{
    public interface IBetRepository
    {
        Task<int> InsertPlayerBet(Fight entity);
        Task<bool> UpdateEod(Fight entity);
        Task<int> InsertFightResult(FightResult entity);
        Task<bool> UpdateFightResult(FightResult entity);
        Task<int> InsertBettingHistory(BettingHistory entity);
        Task<IEnumerable<Fight>> GetLatestFight();
        Task<IEnumerable<Fight>> GetAllPlayerBet(Fight entity);
        Task<double> GetPlayerBetSum(Fight entity);

        Task<int> CloseGame();
    }
}
