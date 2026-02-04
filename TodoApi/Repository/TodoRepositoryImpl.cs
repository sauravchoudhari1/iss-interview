using Microsoft.EntityFrameworkCore;
using TodoApi.DbContext;
using TodoApi.Models.Domain;

namespace TodoApi.Repository
{
    public class TodoRepositoryImpl : ITodoRepository
    {
        private readonly TodoDbContext _db;

        public TodoRepositoryImpl(TodoDbContext db)
        {
            _db = db;
        }

        public async Task<Todo> AddAsync(Todo todo)
        {
            var entry = await _db.Todos.AddAsync(todo);

            await _db.SaveChangesAsync();

            return entry.Entity;
        }

        public async Task<IEnumerable<Todo>> GetAllAsync()
        {
            return await _db.Todos.AsNoTracking().OrderByDescending(t => t.CreatedAt).ToListAsync();
        }

        public async Task<Todo?> GetByIdAsync(Guid id)
        {
            return await _db.Todos.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Todo?> UpdateAsync(Guid id, Todo todo)
        {
            var existing = await _db.Todos.FirstOrDefaultAsync(x => x.Id == id);
            if (existing == null) return null;

            existing.Title = todo.Title;
            existing.Description = todo.Description;
            existing.IsCompleted = todo.IsCompleted;

            await _db.SaveChangesAsync();
            return existing;
        }

        public async Task<Todo?> DeleteAsync(Guid id)
        {
            var existing = await _db.Todos.FirstOrDefaultAsync(x => x.Id == id);
            if (existing == null) return null;

            _db.Todos.Remove(existing);

            await _db.SaveChangesAsync();

            return existing;
        }
    }

}
