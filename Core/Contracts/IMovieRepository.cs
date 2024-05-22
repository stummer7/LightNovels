using Core.Contracts;
using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Core.Contracts
{
    public interface IMovieRepository : IGenericRepository<Movie>
    {
        Task DeleteMovie(int id);
        Task<Movie> GetByIdWithCategoryAsync(int id);
    }
}
