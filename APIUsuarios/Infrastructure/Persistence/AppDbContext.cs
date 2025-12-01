using Microsoft.EntityFrameworkCore;
using APIUsuarios.Domain.Entities;

namespace APIUsuarios.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Nome).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(150);
            entity.Property(u => u.DataNascimento).IsRequired();
            entity.Property(u => u.Telefone).HasMaxLength(15);
            entity.Property(u => u.Ativo).IsRequired();
        });
    }
}
