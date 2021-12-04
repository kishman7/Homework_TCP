using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Homework_TCP
{
    class Program
    {
        static void Main(string[] args)
        {
            // TcpListener - інкапсулює пасивні сокети
            // TcpClient - активні сокети

            const int port = 2020;
            const int size = 100;
            TcpListener server = new TcpListener(IPAddress.Any, port);
            try
            {
                server.Start();

                Console.WriteLine("Wait for connection");
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine($"Connected: {client.Client.RemoteEndPoint}");

                    using (NetworkStream stream = client.GetStream())
                    {

                        byte[] buffer = new byte[size];
                        int count = 0;
                        do
                        {
                            count += stream.Read(buffer, 0, buffer.Length);
                        } while (stream.DataAvailable);

                        string data = Encoding.UTF8.GetString(buffer, 0, count);
                        Console.WriteLine($"Got: {count} bytes, data: {data}");

                        var responce = $"Responce: got {count} bytes";
                        stream.Write(Encoding.UTF8.GetBytes(responce), 0, responce.Length);
                    }

                    // stream.Close();
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
