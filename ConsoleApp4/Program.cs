using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    class Program
    {
        private static void Main(string[] args)
        {
            var listener1 = StartServerAsync(9999).Result;
            //StopServer(listener1);
            //var listener2 = StartServerAsync(1235).Result;
            //StopServer(listener2);
            //var listener3 = StartServerAsync(9999).Result;
            //StopServer(listener3);

            Console.ReadKey();
        }

        private static void StopServer(TcpListener listener)
        {
            if (listener != null)
            {
                Console.WriteLine("Stop on port " + listener.LocalEndpoint);
                listener.Stop();
                listener = null;
            }
        }

        private static async Task<TcpListener> StartServerAsync(int port)
        {
            var listener = new TcpListener(IPAddress.Loopback, port);
            listener.Start();
            Console.WriteLine("Started on port " + port);

            var task = Task.Run(async () => await WaitForConnection(listener));
            await Task.Delay(100);

            return listener;
        }

        private static async Task WaitForConnection(TcpListener listener)
        {
            while (true)
            {
                try
                {
                    var client = await listener.AcceptTcpClientAsync();
                    Console.WriteLine("クライアント({0}:{1})と接続しました。",
            ((System.Net.IPEndPoint)client.Client.RemoteEndPoint).Address,
            ((System.Net.IPEndPoint)client.Client.RemoteEndPoint).Port);
                }
                catch (ObjectDisposedException)
                {
                    Console.WriteLine("Failed on " + listener.LocalEndpoint);
                }
            }
        }
    }

}

