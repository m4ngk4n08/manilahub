using manilahub.data.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace manilahub.core.Services.IServices
{
    public interface IPlayerService
    {
        Task<Player> Get(string username);
        Task<IEnumerable<Player>> GetPendingStatus();
        Task<IEnumerable<Transaction>> GetAllByReferralCode();
        Task<Player> UpdateStatus(Player model);
    }
}
