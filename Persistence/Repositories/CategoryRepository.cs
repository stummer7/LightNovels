using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Persistence.Repositories;
using Core.Entities;
using Core.Contracts;
using Persistence;

namespace Persistence.Repositories
{
    internal class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Category>> GetAllCategoriesAsync()
        {
            return _dbContext.Categories.Include(n=> n.Novels).OrderBy(c => c.Name).ToListAsync();
        }

        public Task<Category> GetByIdWithMoviesAsync(int id)
        {
            return _dbContext.Categories.Include(n => n.Novels).FirstAsync(c => c.Id == id);
        }
    }
}