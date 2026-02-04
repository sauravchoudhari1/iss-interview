using Microsoft.EntityFrameworkCore;
using TodoApi.DbContext;
using TodoApi.Models.Domain;
using TodoApi.Repository;
using System;
using System.Linq;


namespace TodoApi.Tests.Repository
{
    public class TodoRepositoryTests
    {
        private TodoDbContext CreateInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new TodoDbContext(options);
        }

        [Fact]
        public async Task AddAsync_PersistsEntity()
        {
            var db = CreateInMemoryDb();
            var repo = new TodoRepositoryImpl(db);

            var todo = new Todo { Id = Guid.NewGuid(), Title = "RepoTest" };
            var created = await repo.AddAsync(todo);

            Assert.NotNull(created);
            Assert.Equal("RepoTest", created.Title);

            var fromDb = await db.Todos.FindAsync(created.Id);
            Assert.NotNull(fromDb);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsEntity()
        {
            var db = CreateInMemoryDb();
            var todo = new Todo { Id = Guid.NewGuid(), Title = "FindMe" };
            db.Todos.Add(todo);
            await db.SaveChangesAsync();

            var repo = new TodoRepositoryImpl(db);
            var found = await repo.GetByIdAsync(todo.Id);

            Assert.NotNull(found);
            Assert.Equal("FindMe", found.Title);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAll()
        {
            var db = CreateInMemoryDb();
            db.Todos.Add(new Todo { Id = Guid.NewGuid(), Title = "1" });
            db.Todos.Add(new Todo { Id = Guid.NewGuid(), Title = "2" });
            await db.SaveChangesAsync();

            var repo = new TodoRepositoryImpl(db);
            var list = await repo.GetAllAsync();

            Assert.Equal(2, list.Count());
        }

        [Fact]
        public async Task GetAllAsync_ReturnsEmpty_WhenNone()
        {
            var db = CreateInMemoryDb();
            var repo = new TodoRepositoryImpl(db);
            var list = await repo.GetAllAsync();
            Assert.Empty(list);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesEntity()
        {
            var db = CreateInMemoryDb();
            var todo = new Todo { Id = Guid.NewGuid(), Title = "Old" };
            db.Todos.Add(todo);
            await db.SaveChangesAsync();

            var repo = new TodoRepositoryImpl(db);
            var update = new Todo { Title = "New", Description = "d", IsCompleted = true };

            var updated = await repo.UpdateAsync(todo.Id, update);

            Assert.NotNull(updated);
            Assert.Equal("New", updated.Title);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNull_WhenMissing()
        {
            var db = CreateInMemoryDb();
            var repo = new TodoRepositoryImpl(db);
            var update = new Todo { Title = "New" };

            var updated = await repo.UpdateAsync(Guid.NewGuid(), update);

            Assert.Null(updated);
        }

        [Fact]
        public async Task DeleteAsync_DeletesEntity()
        {
            var db = CreateInMemoryDb();
            var todo = new Todo { Id = Guid.NewGuid(), Title = "Del" };
            db.Todos.Add(todo);
            await db.SaveChangesAsync();

            var repo = new TodoRepositoryImpl(db);
            var deleted = await repo.DeleteAsync(todo.Id);

            Assert.NotNull(deleted);
            var fromDb = await db.Todos.FindAsync(todo.Id);
            Assert.Null(fromDb);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsNull_WhenMissing()
        {
            var db = CreateInMemoryDb();
            var repo = new TodoRepositoryImpl(db);
            var deleted = await repo.DeleteAsync(Guid.NewGuid());
            Assert.Null(deleted);
        }
    }
}
