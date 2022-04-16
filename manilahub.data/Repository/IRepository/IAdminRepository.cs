using manilahub.data.Entity;
using System.Threading.Tasks;

namespace manilahub.data.Repository.IRepository
{
    public interface IAdminRepository
    {
        Task<int> Insert(Agent entity);
    }
}
