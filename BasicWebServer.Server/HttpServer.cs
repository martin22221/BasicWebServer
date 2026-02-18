using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using BasicWebServer.Server.Contracts;
using BasicWebServer.Server.HTTP;
using BasicWebServer.Server.Routing;

namespace BasicWebServer.Server
{
    public class HttpServer
    {
        private readonly IPAddress ipAddress;
        private readonly int port;
        private readonly TcpListener listener;

        private readonly RoutingTable routes;

        public HttpServer(string address, int port, Action<IRoutingTable> routingTableConfiguration)
        {
            this.ipAddress = IPAddress.Parse(address);
            this.port = port;
            this.listener = new TcpListener(this.ipAddress, this.port);

            routingTableConfiguration(this.routes = new RoutingTable());
        }


        public HttpServer(int port, Action<IRoutingTable> routingTable) :this("127.0.0.1", port, routingTable)
        {
            
        }
        public HttpServer(Action<IRoutingTable> routingTable) :this(8081, routingTable)
        {
            
        }


        public void Start()
        {      
            this.listener.Start();

            while (true)
            {
                TcpClient client = this.listener.AcceptTcpClient();

                using NetworkStream networkStream = client.GetStream();

                string requestText = ReadRequest(networkStream);
                Console.WriteLine(requestText);
                var request = Request.Parse(requestText);

               // client.Close();

                var response  = routes.MatchRequest(request);

                if (response.PreRenderAction != null)
                {
                    response.PreRenderAction(request, response);
                }
                WriteResponse(networkStream, response);

                client.Close();

            }
        }

        private static void WriteResponse(NetworkStream networkStream, Response response)
        {

           var responseBytes = Encoding.UTF8.GetBytes(response.ToString());
            networkStream.Write(responseBytes);
        }

        private static string ReadRequest(NetworkStream networkStream)
        {
            byte[] buffer = new byte[1024];

            StringBuilder request = new StringBuilder();

            int bytesRead;
            int totalBytesReceived = 0;

            do
            {
                bytesRead = networkStream.Read(buffer, 0, buffer.Length);
                totalBytesReceived += bytesRead;
              
                if (totalBytesReceived > 10000)
                {
                    throw new InvalidOperationException("Request is too large.");
                }
                
                string requestPart = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                request.Append(requestPart);
            }

            while (networkStream.DataAvailable);
            return request.ToString();
        }
    }
}



















