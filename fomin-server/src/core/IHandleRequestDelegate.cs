using System;
using fomin_server.utils;

namespace fomin_server.core
{
    public interface IHandleRequestDelegate
    {
        byte[] HandleRequest(string rawRequest);
    }

    public class SimpleHandleRequestDelegate : IHandleRequestDelegate
    {
        private readonly Func<string, string> _callback;

        public SimpleHandleRequestDelegate(Func<string, string> callback)
        {
            _callback = callback;
        }

        public byte[] HandleRequest(string rawRequest)
        {
            return _callback.Invoke(rawRequest).ToByteArray();
        }
    }
}
