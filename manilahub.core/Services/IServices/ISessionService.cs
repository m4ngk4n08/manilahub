using manilahub.data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace manilahub.core.Services.IServices
{
    public interface ISessionService
    {
        Session GetUserSession(string userId);
        bool Insert(Session entity);

        bool Logout(string username);
    }
}
