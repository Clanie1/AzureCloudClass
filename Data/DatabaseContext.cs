using Microsoft.EntityFrameworkCore;
using Entities;

namespace Data;

public class DatabaseContext: DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }
    
    
    public DbSet<Usuario> cosa { get; set; }
    public DbSet<Objetos> cosa2 { get; set; }
        
    
    
}