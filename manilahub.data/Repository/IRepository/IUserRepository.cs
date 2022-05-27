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
        Task<Agent> GetAgentInfo(int id);
        Task<Player> GetById(int id);
        Task<IEnumerable<Player>> GetAll();
        Task<Player> UpdatePlayerStatus(Player entity);

        Task<Agent> UpdateCommission(Agent entity);
    }
}
