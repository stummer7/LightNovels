using Core.Contracts;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class NovelRepository : GenericRepository<Novel>, INovelRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public NovelRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Novel>> GetAllNovelsAsync()
        {
            return await _dbContext.Novels.Include(n => n.Categories).ToListAsync();
        }
    }
}
