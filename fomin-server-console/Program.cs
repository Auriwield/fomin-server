using System;
using System.Net;
using fominwebsocketserver.src.http;

namespace fomin_server_console
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpServer httpServer = new HttpServer(new IPEndPoint(IPAddress.Loopback, 7654));

            httpServer.ListenUrl("/", request
                => new HttpResponse(
                    ResponseCode.Ok,
                    HttpVersion.Http11,
                    ContentType.TextPlain,
                    "Ураааааа")
            );

            httpServer.Start();
        }
    }
}