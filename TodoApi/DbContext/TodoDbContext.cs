using Microsoft.EntityFrameworkCore;
using TodoApi.Models.Domain;

namespace TodoApi.DbContext
{
    public class TodoDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }

        public DbSet<Todo> Todos { get; set; }
    }
}