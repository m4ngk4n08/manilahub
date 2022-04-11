using manilahub.data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace manilahub.core.Services.IServices
{
    public interface IPlayerService
    {
        Player Get(string username);
        IEnumerable<Player> GetPendingStatus();

        Player UpdateStatus(Player model);
    }
}
