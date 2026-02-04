using Microsoft.AspNetCore.Mvc;
using TodoApi.Models.Domain;
using TodoApi.Models.DTO;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpPost]
        public async Task<ActionResult<Todo>> Create([FromBody] AddTodoDto todo)
        {
            var todoDomain = new Todo
            {
                Id = new Guid(),
                Title = todo.Title,
                Description = todo.Description,
                IsCompleted = todo.IsCompleted,
                CreatedAt = DateTime.UtcNow
            };

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _todoService.CreateAsync(todoDomain);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetAll()
        {
            var todos = await _todoService.GetAllAsync();

            return Ok(todos);
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<Todo>> GetById(Guid id)
        {
            var todo = await _todoService.GetByIdAsync(id);

            if (todo == null) return NotFound();

            return Ok(todo);
        }

        [HttpPut("{id:Guid}")]
        public async Task<ActionResult<Todo>> Update(Guid id, [FromBody] UpdateTodoDto todo)
        {
            var todoDomain = new Todo
            {
                Id = new Guid(),
                Title = todo.Title,
                Description = todo.Description,
                IsCompleted = todo.IsCompleted,
                CreatedAt = DateTime.UtcNow
            };

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _todoService.UpdateAsync(id, todoDomain);

            if (updated == null) return NotFound();

            return Ok(updated);
        }



        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<Todo>> Delete(Guid id)
        {
            var deleted = await _todoService.DeleteAsync(id);

            if (deleted == null) return NotFound();

            return Ok(deleted);
        }
    }
}
