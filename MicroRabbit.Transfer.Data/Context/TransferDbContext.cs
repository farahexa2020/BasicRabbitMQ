using MicroRabbit.Transfer.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MicroRabbit.Transfer.Data.Context
{
    public class TransferDbContext : DbContext
    {
        public DbSet<TransferLog> TransferLog { get; set; }

        public TransferDbContext(DbContextOptions<TransferDbContext> options) : base(options)
        {
        }

        public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TransferDbContext>
        {
            public TransferDbContext CreateDbContext(string[] args)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(@Directory.GetCurrentDirectory() + "/../MicroRabbit.Transfer.Api/appsettings.json")
                    .Build();
                var builder = new DbContextOptionsBuilder<TransferDbContext>();
                var connectionString = configuration.GetConnectionString("TransferDbConnection");
                builder.UseSqlServer(connectionString);
                return new TransferDbContext(builder.Options);
            }
        }
    }
}
