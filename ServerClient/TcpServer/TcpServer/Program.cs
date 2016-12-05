using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpServer
{
    class Program
    {
        private static string __ip;
        private static int __port;

        static void Main(string[] args)
        {
            __port = 1334;
           
            IPAddress ipAd = IPAddress.Any;
            TcpListener myList = new TcpListener(ipAd, __port);
            myList.Start();
            Console.WriteLine("Listening");
            Socket s = myList.AcceptSocket();
            Console.WriteLine("Client connected");
            byte[] b = new byte[100];
            int k = s.Receive(b);
            string ress = "message from client";
            for (int i = 0; i < k; i++)
                ress += Convert.ToChar(b[i]);
            Console.WriteLine(ress);
            s.Send(b); 
            s.Close();
            myList.Stop();
            Console.ReadLine();
        }

    }
}
