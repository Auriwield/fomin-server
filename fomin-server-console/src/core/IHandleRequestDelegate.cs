using System;

namespace fominwebsocketserver.src.core
{
    public interface IHandleRequestDelegate
    {
        string HandleRequest(string rawRequest);
    }

    public class SimpleHandleRequestDelegate : IHandleRequestDelegate
    {
        private readonly Func<string, string> _callback;

        public SimpleHandleRequestDelegate(Func<string, string> callback)
        {
            _callback = callback;
        }

        public string HandleRequest(string rawRequest)
        {
            return _callback.Invoke(rawRequest);
        }
    }
}
