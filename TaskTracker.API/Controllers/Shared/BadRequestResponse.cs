namespace TaskTracker.API.Controllers.Shared
{
    public class BadRequestResponse
    {
        public string Type { get; } = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
        public string Title { get; } = "Bad Request";
        public int Status { get; } = 400;
        public string Detail { get; } = null!;

        public BadRequestResponse(string detail)
        {
            Detail = detail;
        }
    }
}
