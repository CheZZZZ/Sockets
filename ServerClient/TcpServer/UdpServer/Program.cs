using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UdpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            int recv;
            byte[] data = new byte[1024];

            //Get local IP       
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, 8001);
            Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            //Bind ip
            newsock.Bind(ip);

            Console.WriteLine("This is a Server, host name is {0}", Dns.GetHostName());

            //Waiting for client.
            Console.WriteLine("Waiting for a client");

            //get the client IP
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint Remote = (EndPoint)(sender);
            recv = newsock.ReceiveFrom(data, ref Remote);
            Console.WriteLine("Message received from {0}: ", Remote.ToString());
            Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));
             
            string welcome = "hello  client "; 
            data = Encoding.ASCII.GetBytes(welcome);

            //Send message
            newsock.SendTo(data, data.Length, SocketFlags.None, Remote);
            while (true)
            {
                data = new byte[1024];   
                recv = newsock.ReceiveFrom(data, ref Remote);
                Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));
                newsock.SendTo(data, recv, SocketFlags.None, Remote);
            }
        }
    }
}
