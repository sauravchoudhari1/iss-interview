using Microsoft.EntityFrameworkCore;
using TodoApi.DbContext;
using TodoApi.Models.Domain;
using TodoApi.Repository;
using TodoApi.Services;
using System.Linq;
using System;

namespace TodoApi.Tests.Service
{
    public class TodoServiceTests
    {
        private TodoDbContext CreateInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new TodoDbContext(options);
        }

        [Fact]
        public async Task CreateAsync_AddsTodo()
        {
            var db = CreateInMemoryDb();
            var repo = new TodoRepositoryImpl(db);
            var service = new TodoService(repo);

            var todo = new Todo { Id = Guid.NewGuid(), Title = "Test" };

            var created = await service.CreateAsync(todo);

            Assert.NotNull(created);
            Assert.Equal("Test", created.Title);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsList()
        {
            var db = CreateInMemoryDb();
            db.Todos.Add(new Todo { Id = Guid.NewGuid(), Title = "A" });
            db.Todos.Add(new Todo { Id = Guid.NewGuid(), Title = "B" });
            await db.SaveChangesAsync();

            var repo = new TodoRepositoryImpl(db);
            var service = new TodoService(repo);

            var list = await service.GetAllAsync();

            Assert.Equal(2, list.Count());
        }

        [Fact]
        public async Task GetById_ReturnsTodo()
        {
            var db = CreateInMemoryDb();
            var todo = new Todo { Id = Guid.NewGuid(), Title = "FindMe" };
            db.Todos.Add(todo);
            await db.SaveChangesAsync();

            var repo = new TodoRepositoryImpl(db);
            var service = new TodoService(repo);

            var found = await service.GetByIdAsync(todo.Id);

            Assert.NotNull(found);
            Assert.Equal(todo.Id, found.Id);
        }

        [Fact]
        public async Task GetById_ReturnsNull_WhenMissing()
        {
            var db = CreateInMemoryDb();
            var repo = new TodoRepositoryImpl(db);
            var service = new TodoService(repo);

            var found = await service.GetByIdAsync(Guid.NewGuid());

            Assert.Null(found);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesExisting()
        {
            var db = CreateInMemoryDb();
            var todo = new Todo { Id = Guid.NewGuid(), Title = "Old", Description = "d" };
            db.Todos.Add(todo);
            await db.SaveChangesAsync();

            var repo = new TodoRepositoryImpl(db);
            var service = new TodoService(repo);

            var update = new Todo { Title = "New", Description = "updated", IsCompleted = true };

            var updated = await service.UpdateAsync(todo.Id, update);

            Assert.NotNull(updated);
            Assert.Equal("New", updated.Title);

            var fromDb = await db.Todos.FindAsync(todo.Id);
            Assert.Equal("New", fromDb.Title);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNull_WhenMissing()
        {
            var db = CreateInMemoryDb();
            var repo = new TodoRepositoryImpl(db);
            var service = new TodoService(repo);

            var update = new Todo { Title = "New" };

            var updated = await service.UpdateAsync(Guid.NewGuid(), update);

            Assert.Null(updated);
        }

        [Fact]
        public async Task DeleteAsync_DeletesAndReturnsTodo()
        {
            var db = CreateInMemoryDb();
            var todo = new Todo { Id = Guid.NewGuid(), Title = "ToDelete" };
            db.Todos.Add(todo);
            await db.SaveChangesAsync();

            var repo = new TodoRepositoryImpl(db);
            var service = new TodoService(repo);

            var deleted = await service.DeleteAsync(todo.Id);

            Assert.NotNull(deleted);
            Assert.Equal(todo.Id, deleted.Id);

            var fromDb = await db.Todos.FindAsync(todo.Id);
            Assert.Null(fromDb);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsNull_WhenMissing()
        {
            var db = CreateInMemoryDb();
            var repo = new TodoRepositoryImpl(db);
            var service = new TodoService(repo);

            var deleted = await service.DeleteAsync(Guid.NewGuid());

            Assert.Null(deleted);
        }
    }
}
