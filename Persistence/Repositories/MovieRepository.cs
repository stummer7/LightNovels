using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Contracts;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories;

namespace Persistence.Repositories
{
    public class MovieRepository : GenericRepository<Movie>, IMovieRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public MovieRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DeleteMovie(int id)
        {
            await _dbContext!.Database.ExecuteSqlRawAsync($"DELETE FROM Movies where id = {id}");
            _dbContext.ChangeTracker.Clear();
        }

        public async Task<Movie> GetByIdWithCategoryAsync(int id)
        {
            return await _dbContext.Movies.Include(m => m.Category).FirstAsync(m => m.Id == id);
        }
    }
}