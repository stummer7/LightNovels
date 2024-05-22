using Core.Contracts;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext? _dbContext;

        /// <summary>
        /// ConnectionString kommt aus den appsettings.json
        /// </summary>
        public UnitOfWork() : this(new ApplicationDbContext())
        {
        }

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            Movies = new MovieRepository(_dbContext);
            Categories = new CategoryRepository(_dbContext);
            Novels = new NovelRepository(_dbContext);
        }

        public IMovieRepository Movies { get; }
        public ICategoryRepository Categories { get; }
        public INovelRepository Novels { get; }

        public async Task<int> SaveChangesAsync()
        {
            var entities = _dbContext!.ChangeTracker.Entries()
                .Where(entity => entity.State == EntityState.Added
                                 || entity.State == EntityState.Modified)
                .Select(e => e.Entity)
                .ToArray();  // Geänderte Entities ermitteln

            foreach (var entity in entities)
            {
                ValidateEntity(entity);
            }
            return await _dbContext.SaveChangesAsync();
        }

        private void ValidateEntity(object entity)
        {
            var validationContext = new ValidationContext(entity, null, null);
            // so the validating entity class will be able to request this unit of work as a service
            validationContext.InitializeServiceProvider(serviceType => this);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(entity, validationContext, validationResults,
                validateAllProperties: true);

            if (!isValid)
            {
                var memberNames = new List<string>();
                var validationExceptions = new List<ValidationException>();
                foreach (var validationResult in validationResults)
                {
                    validationExceptions.Add(new ValidationException(validationResult, null, null));
                    memberNames.AddRange(validationResult.MemberNames);
                }

                if (validationExceptions.Count == 1)  // eine Validationexception werfen
                {
                    throw validationExceptions.Single();
                }
                else  // AggregateException mit allen ValidationExceptions als InnerExceptions werfen
                {
                    throw new ValidationException($"Entity validation failed for {string.Join(", ", memberNames)}",
                        new AggregateException(validationExceptions));
                }
            }

        }

        public async Task DeleteDatabaseAsync() => await _dbContext!.Database.EnsureDeletedAsync();
        public async Task MigrateDatabaseAsync() => await _dbContext!.Database.MigrateAsync();
        public async Task CreateDatabaseAsync() => await _dbContext!.Database.EnsureCreatedAsync();

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore(true);
            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsyncCore(bool disposing)
        {
            if (_dbContext != null)
            {
                if (disposing)
                {
                    await _dbContext.DisposeAsync();
                }
            }
            _dbContext = null;
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
            GC.SuppressFinalize(this);
            _dbContext = null;
        }
    }
}
