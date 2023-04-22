namespace TaskTracker.API.Controllers.Shared;

public class NotFoundByIdResponse
{
    public string Type { get; } = "https://tools.ietf.org/html/rfc7231#section-6.5.4";
    public string Title { get; } = "Not Found";
    public int Status { get; } = 404;
    public string Detail { get; } = null!;

    public NotFoundByIdResponse(string objectName, int id )
    {
        Detail = $"{objectName} with id {id} not found";
    }
}
