using manilahub.data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace manilahub.data.Repository.IRepository
{
    public interface IRegisterRepository
    {
        bool Insert(Player entity);
    }
}
