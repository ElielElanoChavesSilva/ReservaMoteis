using Microsoft.EntityFrameworkCore;
using ReservaMoteisDomain.Entities;

namespace ReservaMoteisInfra.Context
{
    public class MainContext : DbContext
    {
        public MainContext(DbContextOptions<MainContext> options)
            : base(options)
        {
        }

        public DbSet<ReserveEntity> Reserves { get; set; }
        public DbSet<MotelEntity> Motels { get; set; }
        public DbSet<ProfileEntity> Profiles { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<SuiteEntity> Suites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainContext).Assembly);
        }
    }
}
