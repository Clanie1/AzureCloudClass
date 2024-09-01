using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using WebApplication1.entities;


public class StudentDbContext(DbContextOptions options): DbContext(options)
{
    public DbSet<Student> Student => Set<Student>(); 
}