using Microsoft.EntityFrameworkCore;
using Prova.Models;

namespace Prova.Data;
public class AppDatabase : DbContext
{
    public AppDatabase(DbContextOptions<AppDatabase> options) : base(options) { }
        
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=database.db");

    public DbSet<Funcionario> Funcionarios { get; set; }
    public DbSet<FolhaPagamento> Folhas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    { 
        base.OnModelCreating(modelBuilder);
    }
}
