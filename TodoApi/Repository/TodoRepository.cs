using TodoApi.Models.Domain;

namespace TodoApi.Repository
{
    public interface ITodoRepository
    {
        Task<Todo> AddAsync(Todo todo);
        Task<IEnumerable<Todo>> GetAllAsync();
        Task<Todo?> GetByIdAsync(Guid id);
        Task<Todo?> UpdateAsync(Guid id, Todo todo);
        Task<Todo?> DeleteAsync(Guid id);
    }

}
