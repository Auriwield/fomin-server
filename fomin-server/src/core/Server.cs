using System;
using System.Net;
using System.Net.Sockets;
using fomin_server.utils;

namespace fomin_server.core
{
    public class Server : IServer
    {
        readonly Socket _server;
        readonly IHandleRequestDelegate _handleRequestDelegate;
        readonly byte[] _buffer = new byte[1000];

        public int Timeout { get; set; } = 10000;
        public IPEndPoint IpEndpoint { get; }

        public Server(IPEndPoint ipEndpoint,
            IHandleRequestDelegate requestDelegate)
        {
            IpEndpoint = ipEndpoint;
            _server = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _handleRequestDelegate = requestDelegate;
        }

        public void Start()
        {
            _server.Bind(IpEndpoint);
            _server.Listen(Timeout);
            Logger.I("Server listen on " + IpEndpoint);
            while (true)
            {
                try
                {
                    Socket client = _server.Accept();

                    string request = "";
                    int len;

                    do
                    {
                        len = client.Receive(_buffer);
                        request += _buffer.StringValue(len);
                    } while (len == _buffer.Length);

                    var response = _handleRequestDelegate.HandleRequest(request);

                    client.Send(response);
                    client.Close();
                }
                catch (Exception e)
                {
                    Logger.E(e.Message);
                }
            }
        }

        public void Stop()
        {
            _server.Shutdown(SocketShutdown.Both);
            _server.Dispose();
        }
    }
}