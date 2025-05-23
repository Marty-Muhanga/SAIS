using Microsoft.EntityFrameworkCore;
using SAIS.Models;
using SAIS.ViewModels;

namespace SAIS.Data
{
    public class SAISContext : DbContext
    {
        public SAISContext(DbContextOptions<SAISContext> options) : base(options) { }

        public DbSet<Applicant> Applicants { get; set; } = null!;
        public DbSet<County> Counties { get; set; } = null!;
        public DbSet<SubCounty> SubCounties { get; set; } = null!;
        public DbSet<Location> Locations { get; set; } = null!;
        public DbSet<SubLocation> SubLocations { get; set; } = null!;
        public DbSet<Village> Villages { get; set; } = null!;
        public DbSet<Models.Program_> Programs { get; set; } = null!;
        public DbSet<ApplicantProgram> ApplicantPrograms { get; set; } = null!;
        public DbSet<SexLookup> SexLookups { get; set; } = null!;
        public DbSet<MaritalStatusLookup> MaritalStatusLookups { get; set; } = null!;
        public DbSet<ApplicationStatusLookup> ApplicationStatusLookups { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SearchViewModel>().HasNoKey();
            base.OnModelCreating(modelBuilder);
        }
    }


}