using manilahub.data.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace manilahub.data.Repository.IRepository
{
    public interface ISessionRepository
    {
        Task<IEnumerable<Session>> GetAllUserSession(string userId);
        Task<bool> Insert(Session entity);
        Task<bool> Logout(Session entity);

        Task<bool> UpdateSession(Session entity);
    }
}
