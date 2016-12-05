using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace StreamSocketClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string cm;
        private string port;
        private string ip;
        StreamSocket socket = new StreamSocket();
        public MainPage()
        {
            this.InitializeComponent();
        }
        private async void Receive_Click(object sender, RoutedEventArgs e)
        {
            var res = await receivedata();
        }
        public async System.Threading.Tasks.Task<string> receivedata()
        {
            Stream streamOut = socket.OutputStream.AsStreamForWrite();
            StreamWriter writer = new StreamWriter(streamOut);
            string request = cm;
            await writer.WriteLineAsync(request);
            await writer.FlushAsync();
            txtResult.Text += "\n" + "send request successful";

            Stream streamIn = socket.InputStream.AsStreamForRead();
            StreamReader reader = new StreamReader(streamIn);
            txtResult.Text += "\n" + "read response from server:" + reader.ReadLineAsync();
            return null;
        }

        private async void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            port = "1334";
            ip = "localhost";
            cm = "I'm the message from client";
            HostName serverHost = new HostName(ip);
            string serverPort = port;
            await socket.ConnectAsync(serverHost, serverPort);
            txtResult.Text = "Connect successfully";
        }
    }
}
