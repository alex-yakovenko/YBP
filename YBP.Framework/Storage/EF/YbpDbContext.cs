using Microsoft.EntityFrameworkCore;

namespace YBP.Framework.Storage.EF
{
    public class YbpDbContext: DbContext
    {
        public YbpDbContext(DbContextOptions options) : base(options)
        {
            Options = options;
        }

        private static readonly bool[] _migrated = { false };

        public YbpDbContext()
        {
            if (!_migrated[0])
                lock (_migrated)
                    if (!_migrated[0])
                    {
                        Database.Migrate(); 
                        _migrated[0] = true;
                    }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<YbpProcess>()
                .HasIndex(b => new { b.Pfx, b.InstanceId })
                .IsUnique(true)
                .HasName("IX_Ybp_Process");

            modelBuilder.Entity<YbpActionHistory>()
                .HasIndex(b => b.Name)
                .HasName("IX_Ybp_ActionHistory_Name");
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=YBP;Trusted_Connection=True;");
        }

        public DbContextOptions Options { get; }

        public DbSet<YbpProcess> YbpProcesses { get; set; }
        public DbSet<YbpFlag> YbpFlags { get; set; }
        public DbSet<YbpFlagHistory> YbpFlagHistory { get; set; }
        public DbSet<YbpActionHistory> YbpActionHistory { get; set; }
    }
}
