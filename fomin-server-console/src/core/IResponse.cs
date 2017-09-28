
namespace fominwebsocketserver.src
{
    public interface IResponse
    {
        string Raw { get; }
    }

    public class SimpleResponse : IResponse
    {
        public string Raw { get; }

        public SimpleResponse(string raw)
        {
            Raw = raw;
        }
    }
}
