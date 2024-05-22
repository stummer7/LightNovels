using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Core.Contracts
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category> GetByIdWithMoviesAsync(int id);
    }
}
