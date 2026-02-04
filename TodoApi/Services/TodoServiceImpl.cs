using Microsoft.Data.Sqlite;
using TodoApi.Models.Domain;
using TodoApi.Repository;

namespace TodoApi.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repo;

        public TodoService(ITodoRepository repo)
        {
            _repo = repo;
        }

        public async Task<Todo> CreateAsync(Todo todo)
        {
            todo.CreatedAt = DateTime.UtcNow;
            return await _repo.AddAsync(todo);
        }

        public async Task<IEnumerable<Todo>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Todo?> GetByIdAsync(Guid id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<Todo?> UpdateAsync(Guid id, Todo todo)
        {
            return await _repo.UpdateAsync(id, todo);
        }

        public async Task<Todo?> DeleteAsync(Guid id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
