using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using fomin_server.core;

namespace fomin_server.http
{
    class HttpAccessDelegate : IHandleRequestDelegate
    {
        public const string MimeTypeAll = "*";

        private readonly IDictionary<string, Func<HttpRequest, HttpResponse>> _requestMap;
        private readonly List<IHttpMiddleware> _middlewareMap;


        public HttpAccessDelegate()
        {
            _requestMap = new Dictionary<string, Func<HttpRequest, HttpResponse>>();
            _middlewareMap = new List<IHttpMiddleware>();
        }

        public void ListenUrl(string urlPattern, Func<HttpRequest, HttpResponse> callback)
        {
            _requestMap.Add(urlPattern, callback);
        }

        public void AddMiddleware(IHttpMiddleware middleware)
        {
            _middlewareMap.Add(middleware);
        }

        public HttpResponse AccessUrl(string url, HttpRequest request)
        {

            if (!request.IsValid)
            {
                return new HttpResponse(ResponseCode.BadRequest);
            }

            if (url.Contains("?"))
            {
                url = url.Split('?')[0];
                return AccessUrl(url, request);
            }

            if (_requestMap.ContainsKey(url))
            {
                return _requestMap[url].Invoke(request);
            }

            foreach (KeyValuePair<string, Func<HttpRequest, HttpResponse>> entry in _requestMap)
            {
                if (!entry.Key.Contains("*")) continue;

                if (Regex.IsMatch(url, entry.Key))
                {
                    return entry.Value.Invoke(request);
                }
            }

            return null;
        }

        public byte[] HandleRequest(string rawRequest)
        {
            HttpRequest request = new HttpRequest(rawRequest);
            var httpResponse = AccessUrl(request.Url, request);
            if (httpResponse == null) return new byte[]{};

            foreach (var middleware in _middlewareMap)
            {
                if (middleware.MimeType.Equals(httpResponse.ContentType) || middleware.MimeType.Equals(MimeTypeAll))
                {
                    middleware.Render(request, httpResponse);
                }
            }

            return httpResponse.Raw;
        }
    }
}