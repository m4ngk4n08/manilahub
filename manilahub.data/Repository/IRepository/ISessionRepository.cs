using manilahub.data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace manilahub.data.Repository.IRepository
{
    public interface ISessionRepository
    {
        IEnumerable<Session> GetAllUserSession(string userId);
        bool Insert(Session entity);
        bool Logout(Session entity);
    }
}
