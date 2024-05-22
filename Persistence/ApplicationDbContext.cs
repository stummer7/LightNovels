﻿using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Movie> Movies => Set<Movie>();
        public DbSet<Novel> Novels => Set<Novel>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Environment.CurrentDirectory)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                var configuration = builder.Build();
                Debug.Write(configuration.ToString());
                string connectionString = configuration["ConnectionStrings:DefaultConnection"];
                optionsBuilder.UseSqlServer(connectionString);
                //optionsBuilder.UseLoggerFactory(GetLoggerFactory());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }

}
