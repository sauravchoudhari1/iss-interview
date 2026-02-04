using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoApi.Controllers;
using TodoApi.Models.Domain;
using TodoApi.Services;
using TodoApi.Models.DTO;

namespace TodoApi.Tests.Controller
{
    public class TodosControllerTests
    {
        private readonly Mock<ITodoService> _serviceMock = new();

        [Fact]
        public async Task GetAll_ReturnsOk_WithList()
        {
            var id = Guid.NewGuid();
            _serviceMock.Setup(s => s.GetAllAsync())
                        .ReturnsAsync(new List<Todo> { new Todo { Id = id, Title = "t"} });

            var controller = new TodoController(_serviceMock.Object);
            var result = await controller.GetAll();

            var ok = Assert.IsType<ActionResult<IEnumerable<Todo>>>(result);
            var actionResult = Assert.IsType<OkObjectResult>(ok.Result);
            var list = Assert.IsAssignableFrom<IEnumerable<Todo>>(actionResult.Value);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenMissing()
        {
            var id = Guid.NewGuid();
            _serviceMock.Setup(s => s.GetByIdAsync(id))
                        .ReturnsAsync((Todo?)null);

            var controller = new TodoController(_serviceMock.Object);
            var result = await controller.GetById(id);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Create_ReturnsCreated()
        {
            var createdTodo = new Todo { Id = Guid.NewGuid(), Title = "new" };
            _serviceMock.Setup(s => s.CreateAsync(It.IsAny<Todo>()))
                        .ReturnsAsync(createdTodo);

            var controller = new TodoController(_serviceMock.Object);
            var result = await controller.Create(new AddTodoDto { Title = "new" });

            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returned = Assert.IsAssignableFrom<Todo>(created.Value);
            Assert.Equal("new", returned.Title);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenModelInvalid()
        {
            var controller = new TodoController(_serviceMock.Object);
            controller.ModelState.AddModelError("Title", "Required");

            var result = await controller.Create(new AddTodoDto { Title = null });

            var bad = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.NotNull(bad.Value);
        }

        [Fact]
        public async Task Delete_ReturnsOk_WhenDeleted()
        {
            var id = Guid.NewGuid();
            var existing = new Todo { Id = id, Title = "del" };
            _serviceMock.Setup(s => s.DeleteAsync(id)).ReturnsAsync(existing);
            var controller = new TodoController(_serviceMock.Object);

            var result = await controller.Delete(id);

            var ok = Assert.IsType<ActionResult<Todo>>(result);
            var action = Assert.IsType<OkObjectResult>(ok.Result);
            var value = Assert.IsType<Todo>(action.Value);
            Assert.Equal(id, value.Id);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenMissing()
        {
            var id = Guid.NewGuid();

            _serviceMock.Setup(s => s.DeleteAsync(id)).ReturnsAsync((Todo?)null);

            var controller = new TodoController(_serviceMock.Object);

            var result = await controller.Delete(id);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Update_ReturnsOk_WhenUpdated()
        {
            var id = Guid.NewGuid();
            var updatedTodo = new Todo { Id = id, Title = "updated", Description = "d", IsCompleted = true };

            _serviceMock.Setup(s => s.UpdateAsync(id, It.IsAny<Todo>()))
                        .ReturnsAsync(updatedTodo);

            var controller = new TodoController(_serviceMock.Object);

            var result = await controller.Update(id, new UpdateTodoDto { Title = "updated", Description = "d", IsCompleted = true });

            var ok = Assert.IsType<ActionResult<Todo>>(result);
            var action = Assert.IsType<OkObjectResult>(ok.Result);
            var value = Assert.IsType<Todo>(action.Value);
            Assert.Equal(id, value.Id);
            Assert.Equal("updated", value.Title);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenMissing()
        {
            var id = Guid.NewGuid();

            _serviceMock.Setup(s => s.UpdateAsync(id, It.IsAny<Todo>()))
                        .ReturnsAsync((Todo?)null);

            var controller = new TodoController(_serviceMock.Object);

            var result = await controller.Update(id, new UpdateTodoDto { Title = "nope" });

            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
