using System;
using System.Net;
using fomin_server.http;
using fomin_server.utils;

namespace fomin_server
{
    class Program
    {
        // ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
        {
            HttpServer httpServer = new HttpServer(new IPEndPoint(IPAddress.Loopback, 7654));

            httpServer.ListenUrl("/", request
                => new HttpResponse(ResponseCode.Ok, "Ураааа")
            );

            httpServer.ListenUrl("/*", request
                => HttpResponse.ReturnFile(request.Url)
            );

            httpServer.AddMiddleware(new HttpMiddleware());
            httpServer.AddMiddleware(new HttpLogger());


            httpServer.Start();
        }

        public class HttpMiddleware : IHttpMiddleware
        {
            public void Render(HttpRequest request, HttpResponse response)
            {
                var stringResponse = response.Content.StringValue();

                stringResponse = stringResponse.Replace("{%time}", DateTime.Now.ToString("h:mm:ss tt"));
                stringResponse = stringResponse.Replace("{%date}", DateTime.Now.ToString("dd MM yyyy"));
                stringResponse = stringResponse.Replace("{%host}", request.Host);
                stringResponse = stringResponse.Replace("{%method}", request.HttpMethod.ToString());
                stringResponse = stringResponse.Replace("{%code}", response.ResponseCode.StringValue());
                response.Content = stringResponse.ToByteArray();
            }

            public string MimeType { get; } = "text/html";
        }
    }
}