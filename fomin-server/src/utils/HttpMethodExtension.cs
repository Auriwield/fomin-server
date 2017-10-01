using System;
using fomin_server.http;

namespace fomin_server.utils
{
    public static class HttpMethodExtension
    {
        public static string ToString(this HttpMethod httpMethod)
        {
            switch (httpMethod)
            {
                case HttpMethod.Get: return "GET";
                case HttpMethod.Post: return "POST";
                case HttpMethod.Put: return "PUT";
                case HttpMethod.Patch: return "PATCH";
                case HttpMethod.Head: return "HEAD";
                case HttpMethod.Delete: return "DELETE";
                case HttpMethod.Options: return "OPTIONS";
                case HttpMethod.Connect: return "CONNECT";
                default:
                    throw new Exception("wtf");
            }
        }

        public static HttpMethod IdentifyHttpMethod(string method)
        {
            switch (method)
            {
                case "GET": return HttpMethod.Get;
                case "POST": return HttpMethod.Post;
                case "PUT": return HttpMethod.Put;
                case "PATCH": return HttpMethod.Patch;
                case "HEAD": return HttpMethod.Head;
                case "DELETE": return HttpMethod.Delete;
                case "OPTIONS": return HttpMethod.Options;
                case "CONNECT": return HttpMethod.Connect;
                default:
                    return default (HttpMethod);
            }
        }
    }
}