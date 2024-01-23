using Microsoft.EntityFrameworkCore;
using TodoList.Infrastructure.DataModels;

namespace TodoList.Infrastructure;

public class TodoListDbContext : DbContext
{
    public TodoListDbContext(DbContextOptions<TodoListDbContext> options) : base(options)
    {
    }

    public DbSet<TodoListDataModel> TodoLists { get; set; }
    public DbSet<TodoItemDataModel> TodoItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var todoListBuilder = modelBuilder.Entity<TodoListDataModel>();
        todoListBuilder
            .ToTable("TodoList")
            .HasKey(t => t.Id);

        todoListBuilder
            .HasMany<TodoItemDataModel>(t => t.TodoItems)
            .WithOne()
            .HasForeignKey(t => t.TodoListId);

        modelBuilder
            .Entity<TodoItemDataModel>()
            .ToTable("TodoItem")
            .HasKey(t => t.Id);
    }
}