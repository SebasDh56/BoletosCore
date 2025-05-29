using Microsoft.EntityFrameworkCore;
using BoletosCore.Models;

namespace BoletosCore.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Pasajero> Pasajeros { get; set; }
        public DbSet<Boleto> Boletos { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Cooperativa> Cooperativas { get; set; }
        public DbSet<BoletoRedirigido> BoletosRedirigidos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pasajero>()
                .HasIndex(p => p.Cedula)
                .IsUnique();
        }
    }
}