using System;
using System.Net;
using fomin_server.core;

namespace fomin_server.http
{
    public class HttpServer : IServer
    {
        private readonly Server _server;
        private readonly HttpAccessDelegate _httpAccessDelegate;

        public HttpServer(IPEndPoint ipEndpoint)
        {
            _httpAccessDelegate = new HttpAccessDelegate();
            _server = new Server(ipEndpoint, _httpAccessDelegate);
        }

        public void ListenUrl(string url, Func<HttpRequest, HttpResponse> callback)
        {
            _httpAccessDelegate.ListenUrl(url, callback);
        }

        public void AddMiddleware(IHttpMiddleware middleware)
        {
            _httpAccessDelegate.AddMiddleware(middleware);
        }

        public void Start()
        {
            _server.Start();
        }

        public void Stop()
        {
            _server.Stop();
        }
    }
}
