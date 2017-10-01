using fomin_server.core;

namespace fomin_server.http
{
    public interface IHttpMiddleware : IMiddleware<HttpRequest, HttpResponse>
    {
        string MimeType { get; }
    }
}
