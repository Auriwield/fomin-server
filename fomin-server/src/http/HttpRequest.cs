using System;
using System.Collections.Generic;
using fomin_server.core;
using fomin_server.utils;

namespace fomin_server.http
{
    public class HttpRequest : IRequest
    {
        public HttpVersion HttpVersion { get; }
        public HttpMethod HttpMethod { get; }
        public string Url { get; }
        public string UserAgent { get; }
        public string Host { get; }
        public IDictionary<string, string> Headers { get; }
        public IDictionary<string, string> Parameters { get; }
        public string Content { get; }
        public string Raw { get; }
        public bool IsValid { get; }

        public HttpRequest(string rawRequest)
        {
            Raw = rawRequest + "";
            if (rawRequest.IsEmpty())
                return;
            try
            {
                string[] requestsStrings = rawRequest.Split(new[] {"\r\n"}, StringSplitOptions.None);
                var str = requestsStrings[0].Split(' ');
                var method = str[0].ToLower();
                HttpMethod = HttpMethodExtension.IdentifyHttpMethod(method);
                Url = str[1].ToLower();
                HttpVersion = str[2].ToLower().Equals("http/1.1") ? HttpVersion.Http11 : HttpVersion.Http10;

                var headAndBody = rawRequest.Split(new[] {"\r\n\r\n"}, StringSplitOptions.None);
                var fields = GetFields(headAndBody[0]);
                UserAgent = fields["user-agent"];
                Host = fields["host"];
                fields.Remove("user-agent");
                fields.Remove("host");
                Headers = fields;
                Parameters = GetParameters(Url);
                Content = headAndBody[1];
            }
            catch (Exception e)
            {
                Logger.E(e.ToString());
            }

            IsValid = true;
        }

        public IDictionary<string, string> GetFields(string rawRequest)
        {
            IDictionary<string, string> fields = new Dictionary<string, string>();
            var ind = rawRequest.IndexOf("\r\n", StringComparison.Ordinal) + 2;
            string[] requestStrings = rawRequest.Remove(0, ind)
                .Split(new[] {"\r\n"}, StringSplitOptions.None);
            foreach (string str in requestStrings)
            {
                if (str.Contains(":"))
                {
                    var field = str.Split(':');
                    fields.Add(field[0].ToLower().Trim(), field[1].ToLower().Normalize());
                }
            }
            return fields;
        }

        public IDictionary<string, string> GetParameters(string url)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            if (!url.Contains("?")) return parameters;

            string pars = url.Split('?')[1];
            var prms = pars.Contains("&") ? new[] {pars} : pars.Split('&');
            foreach (string str in prms)
            {
                if (!str.Contains("=")) continue;

                var keyAndValue = str.Split('=');
                parameters.Add(keyAndValue[0], keyAndValue[1]);
            }
            return parameters;
        }
    }

    public enum HttpVersion
    {
        Http10,
        Http11
    }

    public enum HttpMethod
    {
        Get,
        Head,
        Post,
        Put,
        Patch,
        Delete,
        Options,
        Connect
    }
}