
using fomin_server.utils;

namespace fomin_server.core
{
    public interface IResponse
    {
        byte[] Raw { get; }
    }

    public class SimpleResponse : IResponse
    {
        public byte[] Raw { get; }

        public SimpleResponse(string raw)
        {
            Raw = raw.ToByteArray();
        }

        public SimpleResponse(byte[] raw)
        {
            Raw = raw;
        }
    }
}
