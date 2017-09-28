namespace fominwebsocketserver.src.core
{
    public interface IRequest
    {
        string Raw { get; }
    }

    public class SimpleRequest : IRequest
    {
        public string Raw { get; }

        public SimpleRequest(string raw)
        {
            Raw = raw;
        }
    }
}