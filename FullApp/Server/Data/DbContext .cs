using FullApp.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FullApp.Server.Data
{
    public class DbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }

        private readonly IConfiguration configuration;

        public DbContext(DbContextOptions<DbContext> options, IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(5, 7, 41)));
            }
        }
    }
}
