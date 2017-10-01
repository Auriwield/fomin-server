using fomin_server.http;

namespace fomin_server.utils
{
    public static class HttpResponseExtension
    {
        public static string StringValue(this ResponseCode responseCode)
        {
            switch (responseCode)
            {
                case ResponseCode.Ok: return "200 OK";
                case ResponseCode.Created: return "201 CREATED";
                case ResponseCode.NoContent: return "204 NO CONTENT";
                case ResponseCode.NotModified: return "304 NOT MODIFIED";
                case ResponseCode.BadRequest: return "400 BAD REQUEST";
                case ResponseCode.Unauthorized: return "401 UNAUTHORIZED";
                case ResponseCode.Forbidden: return "403 FORBIDDEN";
                case ResponseCode.NotFound: return "404 NOT FOUND";
                case ResponseCode.Conflict: return "500 INTERNAL SERVER ERROR";
                case ResponseCode.InternalServerError: return "200 OK";
                default: return "501 NOT IMPLEMENTED";
            }
        }

        public static string StringValue(this HttpVersion httpVersion)
        {
            return httpVersion == HttpVersion.Http11 ? "HTTP/1.1" : "HTTP/1.0";
        }
    }
}