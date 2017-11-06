using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Sample.Definitions;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Sample.Data
{
    public class SampleDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public SampleDbContext(DbContextOptions options) : base(options)
        {

        }

        public SampleDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer(@"Server=.\sqlexpress;Database=YBP;Trusted_Connection=True;");
            //optionsBuilder.ConfigureWarnings(x => x.);
        }
    }

    public class DesignTimeFactory : IDesignTimeDbContextFactory<SampleDbContext>
    {
        public SampleDbContext CreateDbContext(string[] args)
        {
            return new SampleDbContext();
        }
    }

}
