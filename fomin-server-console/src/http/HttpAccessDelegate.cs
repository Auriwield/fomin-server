using System;
using System.Collections.Generic;
using fominwebsocketserver.src.core;

namespace fominwebsocketserver.src.http
{
    class HttpAccessDelegate : IHandleRequestDelegate
    {
        private readonly IDictionary<string, Func<HttpRequest, HttpResponse>> _requestMap;

        public HttpAccessDelegate()
        {
            _requestMap = new Dictionary<string, Func<HttpRequest, HttpResponse>>();
        }

        public void ListenUrl(string url, Func<HttpRequest, HttpResponse> callback)
        {
            _requestMap.Add(url, callback);
        }

        public HttpResponse AccessUrl(string url, HttpRequest request)
        {
            if (_requestMap.ContainsKey(url))
            {
                return _requestMap[url].Invoke(request);
            }

            if (url.Contains("?"))
            {
                url = url.Split('?')[0];
                return AccessUrl(url, request);
            }

            return null;
        }

        public string HandleRequest(string rawRequest)
        {
            HttpRequest request = new HttpRequest(rawRequest);
            var httpResponse = AccessUrl(request.Url, request);
            if (httpResponse == null) return "";
            return httpResponse.Raw;
        }
    }
}