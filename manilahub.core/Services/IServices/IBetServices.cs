using manilahub.data.Entity;
using manilahub.data.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace manilahub.core.Services.IServices
{
    public interface IBetServices
    {
        Task<bool> PlayerBet(Fight model);

        Task<int> StartGame();

        Task<bool> EndGame();

        Task<ResultEnum> DeclareWinner(Fight model);

        Task<IEnumerable<Fight>> GetAllPlayerBet();
    }
}
