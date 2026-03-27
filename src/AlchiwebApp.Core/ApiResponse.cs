using System.Text.Json;

namespace AlchiwebApp.Core;

public class ApiResponse<T> : ApiResponse
{
    public T? Result { get; set; }
}
public class ApiResponse
{
    public int? StatusCode { get; set; }
    public string? Title { get; set; }
    public Dictionary<string, string[]>? Errors { get; set; }
}
