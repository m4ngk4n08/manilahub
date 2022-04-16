using manilahub.data.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace manilahub.core.Services.IServices
{
    public interface ISessionService
    {
        Task<Session> GetUserSession(string userId);
        Task<bool> Insert(Session entity);

        Task<bool> Logout(string username);
    }
}
