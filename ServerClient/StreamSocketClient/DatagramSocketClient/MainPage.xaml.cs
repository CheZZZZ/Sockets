using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Sockets;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DatagramSocketClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        private async void btnConnect_Click(object sender, RoutedEventArgs e)
        {

            Windows.Networking.Sockets.DatagramSocket socket = new Windows.Networking.Sockets.DatagramSocket();

            socket.MessageReceived += Socket_MessageReceived;

            //You can use any port that is not currently in use already on the machine. We will be using two separate and random 
            //ports for the client and server because both the will be running on the same machine.
            string serverPort = "8001";
            string clientPort = "8002";

            //Because we will be running the client and server on the same machine, we will use localhost as the hostname.
            Windows.Networking.HostName serverHost = new Windows.Networking.HostName("127.0.0.1");

            //Bind the socket to the clientPort so that we can start listening for UDP messages from the UDP echo server.
            await socket.BindServiceNameAsync(clientPort);
            await socket.ConnectAsync(serverHost, serverPort);
            //Write a message to the UDP echo server.
            Stream streamOut = (await socket.GetOutputStreamAsync(serverHost, serverPort)).AsStreamForWrite();
            StreamWriter writer = new StreamWriter(streamOut);
            string message = "I'm the message from client!";
            await writer.WriteLineAsync(message);
            await writer.FlushAsync();
        }

        private async void Socket_MessageReceived(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
        {
            //Read the message that was received from the UDP echo server.
            try
            {
                Stream streamIn = args.GetDataStream().AsStreamForRead();
                StreamReader reader = new StreamReader(streamIn);
                string message = await reader.ReadLineAsync();

                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    txtresult.Text ="server said:"+ message;
                });

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }

        }
    }
}
