using FluentAssertions;
using TodoList.Domain;
using TodoList.Domain.Enums;

namespace TodoList.UnitTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    [TestCase("")]
    [TestCase(null)]
    public void Title_Must_Has_Value(string title)
    {
        var newTodoListFunc = () => new Domain.TodoList(title);
        var newTodoItemFunc = () => new TodoItem(title, Priority.Low, Guid.NewGuid());
        
        newTodoListFunc.Should().Throw<Exception>().WithMessage("Title is required");
        newTodoItemFunc.Should().Throw<Exception>().WithMessage("Title is required");
    }
}