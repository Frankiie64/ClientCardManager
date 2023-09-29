using ClientCardManager.Core.Domain.Common;
using ClientCardManager.Core.Domain.Entidad;
using Microsoft.EntityFrameworkCore;

namespace ClientCardManager.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<TipoTarjeta> TipoTarjetas { get; set; }
        public virtual DbSet<ClienteTarjeta> ClienteTarjetas { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opt) : base(opt)
        {}
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<EntidadBaseAuditoria>())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.Entity.UltimaModificacion = DateTime.Now;
                        break;
                    case EntityState.Added:
                        entry.Entity.Creado = DateTime.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder mb)
        {
            // Definition tables
            mb.Entity<Cliente>().ToTable("Clientes");
            mb.Entity<TipoTarjeta>().ToTable("TipoTarjetas");
            mb.Entity<ClienteTarjeta>().ToTable("ClienteTarjetas");

            // Primary Key
            mb.Entity<Cliente>().HasKey(x => x.Id);
            mb.Entity<TipoTarjeta>().HasKey(x => x.Id);
            mb.Entity<ClienteTarjeta>().HasKey(x => x.Id);

            // Foreign Key
            mb.Entity<Cliente>()
          .HasMany<ClienteTarjeta>(x => x.Tarjetas)
          .WithOne(x => x.Cliente)
          .HasForeignKey(x => x.IdCliente)
          .OnDelete(deleteBehavior: DeleteBehavior.Restrict);

            mb.Entity<TipoTarjeta>()
          .HasMany<ClienteTarjeta>(x => x.Tarjetas)
          .WithOne(x => x.Tarjeta)
          .HasForeignKey(x => x.IdTipoTarjeta)
          .OnDelete(deleteBehavior: DeleteBehavior.Restrict);

            mb.Entity<ClienteTarjeta>()
          .HasOne<Cliente>(x => x.Cliente)
          .WithMany(x => x.Tarjetas)
          .HasForeignKey(x => x.IdCliente)
          .OnDelete(deleteBehavior: DeleteBehavior.Restrict);

            mb.Entity<ClienteTarjeta>()
          .HasOne<TipoTarjeta>(x => x.Tarjeta)
          .WithMany(x => x.Tarjetas)
          .HasForeignKey(x => x.IdTipoTarjeta)
          .OnDelete(deleteBehavior: DeleteBehavior.Restrict);

            // Required Properties
            mb.Entity<Cliente>(e =>
            {
                e.Property(p => p.Id).IsRequired();
                e.Property(p => p.Creado).IsRequired();
                e.Property(p => p.Nombre).IsRequired().HasMaxLength(150);
                e.Property(p => p.Apellido).IsRequired().HasMaxLength(150);
                e.Property(p => p.Telefono).IsRequired().HasMaxLength(50);
                e.Property(p => p.Ocupacion).IsRequired().HasMaxLength(150);
                e.Property(p => p.UltimaModificacion).IsRequired(false);

            });

            mb.Entity<TipoTarjeta>(e =>
            {
                e.Property(p => p.Id).IsRequired();
                e.Property(p => p.Creado).IsRequired();
                e.Property(p => p.Nombre).IsRequired().HasMaxLength(100);               
                e.Property(p => p.UltimaModificacion).IsRequired(false);

            });

            mb.Entity<ClienteTarjeta>(e =>
            {
                e.Property(p => p.Id).IsRequired();
                e.Property(p => p.Creado).IsRequired();
                e.Property(p => p.IdCliente).IsRequired();
                e.Property(p => p.IdTipoTarjeta).IsRequired();
                e.Property(p => p.Numero).IsRequired().HasMaxLength(20);
                e.Property(p => p.Banco).IsRequired();
                e.Property(p => p.AnioVencimiento).IsRequired();
                e.Property(p => p.MesVencimiento).IsRequired();
                e.Property(p => p.UltimaModificacion).IsRequired(false);

                e.HasIndex(p => p.Numero).IsUnique();
            });
        }

    }
}