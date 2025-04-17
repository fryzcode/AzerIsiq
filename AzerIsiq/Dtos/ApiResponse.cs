namespace AzerIsiq.Dtos;

public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public string? Path { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public ApiResponse(int statusCode, string message, T? data = default, string? path = null)
    {
        StatusCode = statusCode;
        Message = message;
        Data = data;
        Path = path;
    }

    public static ApiResponse<T> Fail(int statusCode, string message, string? path = null)
        => new(statusCode, message, default, path);

    public static ApiResponse<T> Success(T data, string message = "Success", int statusCode = 200, string? path = null)
        => new(statusCode, message, data, path);
}