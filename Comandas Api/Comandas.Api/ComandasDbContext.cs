using Microsoft.EntityFrameworkCore;

namespace Comandas.Api
{
    public class ComandasDbContext : DbContext
    {
        public ComandasDbContext(DbContextOptions<ComandasDbContext> options) : base(options)
        {
        }
        // Definir algumas configurações adicionais no banco
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Usuario>()
                .HasData(
                    new Models.Usuario
                    {
                        Id = 1,
                        Nome = "Admin",
                        Email = "admin@admin.com",
                        Senha = "admin123"
                    }
                );
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Models.Mesa>()
                .HasData(
                    new Models.Mesa
                    {
                        Id = 1,
                        NumeroMesa = 1,
                        SituacaoMesa = 1
                    },
                    new Models.Mesa
                    {
                        Id = 2,
                        NumeroMesa = 2,
                        SituacaoMesa = 2
                    },
                    new Models.Mesa
                    {
                        Id = 3,
                        NumeroMesa = 3,
                        SituacaoMesa = 3
                    }
                );
            modelBuilder.Entity<Models.Mesa>();

            modelBuilder.Entity<Models.CardapioItem>()
                .HasData(
                    new Models.CardapioItem
                    {
                        Id = 1,
                        Titulo = "a",
                        Descricao = "a",
                        Preco = 10,
                        PossuiPreparo = true,
                    },
                    new Models.CardapioItem
                    {
                        Id = 2,
                        Titulo = "b",
                        Descricao = "b",
                        Preco = 10,
                        PossuiPreparo = true,
                    },
                    new Models.CardapioItem
                    {
                        Id = 3,
                        Titulo = "c",
                        Descricao = "c",
                        Preco = 10,
                        PossuiPreparo = true,
                    }
                );
            modelBuilder.Entity<Models.CardapioItem>();
        }

        public DbSet<Models.Usuario> Usuarios { get; set; } = default!;
        public DbSet<Models.Mesa> Mesas { get; set; } = default!;
        public DbSet<Models.Reserva> Reservas { get; set; } = default!;
        public DbSet<Models.Comanda> Comandas { get; set; } = default!;
        public DbSet<Models.ComandaItem> ComandaItens { get; set; } = default!;
        public DbSet<Models.PedidoCozinha> pedidoCozinhas { get; set; } = default!;
        public DbSet<Models.PedidoCozinhaItem> PedidoCozinhaItems { get; set; } = default!; 
        public DbSet<Models.CardapioItem> CardapioItens { get; set; } = default!;

    }
}
