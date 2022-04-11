using manilahub.data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace manilahub.data.Repository.IRepository
{
    public interface IPlayerRepository
    {
        Player Get(string username);
        IEnumerable<Player> GetAll();

        Player UpdatePlayerStatus(Player entity);
    }
}
