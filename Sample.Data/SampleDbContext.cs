using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Sample.Definitions;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Sample.Data.Entities;
using Microsoft.Extensions.Configuration;

namespace Sample.Data
{
    public class SampleDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        private string _connectionString = null;

        public SampleDbContext(DbContextOptions<SampleDbContext> options) : base(options)
        {

        }

        public SampleDbContext()
        {
        }

        public SampleDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            if (!string.IsNullOrWhiteSpace(_connectionString))
                optionsBuilder.UseSqlServer(_connectionString);
        }

        public DbSet<Company> Companies { get; set; }
    }

    public class DesignTimeFactory : IDesignTimeDbContextFactory<SampleDbContext>
    {
        public SampleDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets("YBP-B24A7B7F-D538-4230-9AEB-11928B687712")
                .Build();

            return new SampleDbContext(config["YbpSampleAppConnectionString"]);
        }
    }

}
