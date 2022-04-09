using manilahub.data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace manilahub.core.Services.IServices
{
    public interface ILoginService
    {
        bool Login(Login model);
    }
}
