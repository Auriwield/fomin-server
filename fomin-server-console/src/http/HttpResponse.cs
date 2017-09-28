using fominwebsocketserver.src.utils;

namespace fominwebsocketserver.src.http
{
    public class HttpResponse : IResponse
    {
        public string Raw
        {
            get
            {
                string response = "";
                response += HttpVersion == HttpVersion.Http11 ? "HTTP/1.1 " : "HTTP/1.0 ";
                response += ResponseCode.StringValue();
                response += "\n";
                response += "Content-Type: text/html; charset=utf-8\n";
                response += "Content-Length" + Content.ToByteArray().Length + "\n";
                response += "Content-Type: text/plain\n";
                response += "Connection: close\n";

                return response;
            }
        }

        public ResponseCode ResponseCode { get; }
        public HttpVersion HttpVersion { get; }
        public ContentType ContentType { get; }
        public string Content { get; }

        public HttpResponse(ResponseCode responseCode, HttpVersion version,
            ContentType type, string content)
        {
            ResponseCode = responseCode;
            HttpVersion = version;
            ContentType = type;
            Content = content;
        }
    }

    public enum ResponseCode
    {
        Ok,
        Created,
        NoContent,
        NotModified,
        BadRequest,
        Unauthorized,
        Forbidden,
        NotFound,
        Conflict,
        InternalServerError
    }

    public enum ContentType
    {
        TextHtml,
        Multipart,
        TextPlain
    }
}