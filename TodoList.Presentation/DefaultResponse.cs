namespace TodoList.Presentation;

public class DefaultResponse
{
    public string Message { get; set; }
}

public class DefaultResponse<T> : DefaultResponse
{
    public T Data { get; set; }
}