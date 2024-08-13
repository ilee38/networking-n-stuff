namespace WebServer;

public static class HttpConstants
{
    public static readonly Dictionary<string, string> MimeTypes = new Dictionary<string, string>
    {
        { "html", "text/html" },
        { "txt", "text/plain" },
        { "json", "application/json" },
        { "pdf", "application/pdf" },
        { "jpeg", "image/jpeg" },
        { "gif", "image/gif" },
        { "generic", "application/octet-stream" }
    };
}
