using Dominios.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Data
{
    public class SenffContext : DbContext
    {
        public DbSet<Sala> Sala { get; set; }
        public DbSet<Reserva> Reserva { get; set; }

        public SenffContext(DbContextOptions<SenffContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configurações do modelo

            modelBuilder.Entity<Sala>()
                .HasKey(p => p.SalaId);

            modelBuilder.Entity<Reserva>(x =>
            {
                x.HasKey(p => p.ReservaId);

                x.HasOne(p => p.Sala)
                .WithMany()
                .HasForeignKey(y => y.SalaId);

                x.HasOne(p => p.Usuario)
                .WithMany()
                .HasForeignKey(y => y.UsuarioId);
            });

            modelBuilder.Entity<Usuario>()
                .HasKey(p => p.UsuarioId);
        }
    }
}