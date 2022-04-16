using manilahub.data.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace manilahub.data.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<Player> Get(string username);
        Task<Agent> GetAgentInfo(string id);
        Task<Player> GetById(string id);
        IEnumerable<Player> GetAll();
        Task<Player> UpdatePlayerStatus(Player entity);
    }
}
