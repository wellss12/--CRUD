using Microsoft.EntityFrameworkCore;
using TodoList.Application.Extensions;
using TodoList.Domain;
using TodoList.Infrastructure;
using TodoList.Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediator();
builder.Services.AddScoped<ITodoListRepository, TodoListRepository>();
var connectionString = builder.Configuration.GetConnectionString("TodoListDb");
builder.Services.AddDbContext<TodoListDbContext>(options =>
    options.UseSqlite(connectionString));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program{}