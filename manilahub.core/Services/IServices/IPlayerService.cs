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
        IEnumerable<Player> GetPendingStatus();

        Task<Player> UpdateStatus(Player model);
    }
}
