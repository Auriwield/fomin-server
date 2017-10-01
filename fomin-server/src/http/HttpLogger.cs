using System;
using fomin_server.utils;

namespace fomin_server.http
{
    public class HttpLogger : IHttpMiddleware
    {
        public string MimeType => HttpAccessDelegate.MimeTypeAll;

        public void Render(HttpRequest request, HttpResponse response)
        {
            if (request.IsValid)
            {
                Logger.I(request.HttpMethod.StringValue() + " " + request.Url + " " + request.HttpVersion.StringValue());
            }
            else
            {
                if (request.Raw.IsEmpty())
                {
                    Logger.E("Empty request\n");
                }
                else
                {
                    Logger.E("Invalid request\n" + request.Raw + "\n");
                }
            }

            if (response.ResponseCode == ResponseCode.Ok
                || response.ResponseCode == ResponseCode.NotModified)
            {
                Logger.I(response.HttpVersion.StringValue() + " " + response.ResponseCode.StringValue());
            }
            else
            {
                Logger.E(response.Raw.StringValue() + "\n");
            }
        }
    }
}