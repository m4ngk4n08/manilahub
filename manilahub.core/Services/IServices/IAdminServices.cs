using manilahub.data.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace manilahub.core.Services.IServices
{
    public interface IAdminServices
    {
        Task<bool> Insert(Player model);

        Task<bool> UpdateRole(Player model);
    }
}
