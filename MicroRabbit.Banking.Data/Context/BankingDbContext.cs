using MicroRabbit.Banking.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MicroRabbit.Banking.Data.Context
{
    public class BankingDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options)
        {
        }

        public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BankingDbContext>
        {
            public BankingDbContext CreateDbContext(string[] args)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(@Directory.GetCurrentDirectory() + "/../MicroRabbit.Banking.Api/appsettings.json")
                    .Build();
                var builder = new DbContextOptionsBuilder<BankingDbContext>();
                var connectionString = configuration.GetConnectionString("BankingDbConnection");
                builder.UseSqlServer(connectionString);
                return new BankingDbContext(builder.Options);
            }
        }
    }
}
