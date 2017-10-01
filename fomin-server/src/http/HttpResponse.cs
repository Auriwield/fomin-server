using System.IO;
using fomin_server.core;
using fomin_server.utils;

namespace fomin_server.http
{
    public class HttpResponse : IResponse
    {
        public byte[] Raw
        {
            get
            {
                string response = "";
                response += HttpVersion.StringValue() + " ";
                response += ResponseCode.StringValue();
                response += "\r\n";
                // response += "Content-Type: text/html; charset=utf-8\r\n";

                var contentIsEmpty = !Content.IsEmpty();
                if (contentIsEmpty)
                {
                    response += "Content-Length: " + Content.Length + "\r\n";
                    response += "Content-Type: " + ContentType + "; charset=utf-8\r\n";
                }

                response += "Connection: Close\r\n";

                if (!contentIsEmpty) return response.ToByteArray();

                response += "\r\n";

                return response.ToByteArray().Append(Content);
            }
        }

        public ResponseCode ResponseCode { get; }
        public HttpVersion HttpVersion { get; }
        public string ContentType { get; }
        public byte[] Content { get; set; }

        public HttpResponse(ResponseCode responseCode,
            HttpVersion version = HttpVersion.Http11)
        {
            ResponseCode = responseCode;
            HttpVersion = version;
        }

        public HttpResponse(ResponseCode responseCode, string content, 
            string contentType = "text/plain", HttpVersion version = HttpVersion.Http11)
        {
            ResponseCode = responseCode;
            HttpVersion = version;
            ContentType = contentType;
            Content = content.ToByteArray();
        }

        public HttpResponse(ResponseCode responseCode, string contentType, 
            byte[] content, HttpVersion version = HttpVersion.Http11)
        {
            ResponseCode = responseCode;
            HttpVersion = version;
            ContentType = contentType;
            Content = content;
        }


        public static HttpResponse ReturnFile(string filePath)
        {
            if (filePath.IsEmpty()) return new HttpResponse(ResponseCode.BadRequest);

            if (filePath.Contains("/"))
            {
                filePath = filePath.Replace("/", @"\");
            }

            if (filePath.StartsWith(@"\"))
            {
                filePath = filePath.Substring(1);
            }

            var rootPath = Path.GetFullPath(@"..\..\public\");

            var resultPath = Path.Combine(rootPath, filePath);
            if (!resultPath.Contains(rootPath) || !File.Exists(resultPath))
                return new HttpResponse(ResponseCode.NotFound);

            var extension = Path.GetExtension(resultPath);
            if (extension.IsEmpty()) return new HttpResponse(ResponseCode.NotFound);

            var mimeType = MimeTypeUtils.GetMimeType(extension);
            var content = File.ReadAllBytes(resultPath);

            return new HttpResponse(ResponseCode.Ok, mimeType, content);
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
}