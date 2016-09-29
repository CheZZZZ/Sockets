using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CChatClient
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

        // Declare member objects
        // Client for tcp, network stream to read bytes in socket
        Windows.Networking.Sockets.StreamSocket tcpClient = new Windows.Networking.Sockets.StreamSocket();
        //TcpClient tcpClient = new TcpClient();
        //Stream streamOut = tcpClient.OutputStream.AsStreamForWrite();
        Stream serverStream = null;
        string readData = string.Empty;
        string msg = "Conected to Chat Server ...";


        // Purpose:     Connect to node.js application (lamechat.js)
        // End Result:  node.js app now has a socket open that can send
        //              messages back to this tcp client application
        private async void cmdConnect_Click(object sender, RoutedEventArgs e)
        {
            AddPrompt();
            Windows.Networking.HostName serverHost = new Windows.Networking.HostName("127.0.0.1");
           await tcpClient.ConnectAsync(serverHost, "8000");

            serverStream = tcpClient.OutputStream.AsStreamForWrite();

            StreamWriter writer = new StreamWriter(serverStream);
            string request = txtChatName.Text.Trim()+ " is joining";
            await writer.WriteLineAsync(request);
            await writer.FlushAsync();

            Stream streamIn = tcpClient.InputStream.AsStreamForRead();
            StreamReader reader = new StreamReader(streamIn);
            string response = await reader.ReadLineAsync();
            //byte[] outStream = Encoding.ASCII.GetBytes(txtChatName.Text.Trim()
            //                      + " is joining");
            //serverStream.Write(outStream, 0, outStream.Length);
            //serverStream.Flush();

            // upload as javascript blob
            //Task taskOpenEndpoint = Task.Factory.StartNew(() =>
            //{
            //    while (true)
            //    {
            //        // Read bytes
            //        serverStream = tcpClient.InputStream.AsStreamForRead();
            //        byte[] message = new byte[4096];
            //        int bytesRead;
            //        bytesRead = 0;

            //        try
            //        {
            //            // Read up to 4096 bytes
            //            bytesRead = serverStream.Read(message, 0, 4096);
            //        }
            //        catch
            //        {
            //            /*a socket error has occured*/
            //        }

            //        //We have rad the message.
            //        ASCIIEncoding encoder = new ASCIIEncoding();
            //        // Update main window
            //        AddMessage(encoder.GetString(message, 0, bytesRead));
            //        //Thread.Sleep(500);
            //    }
            //});
        }

        // Purpose:     Updates the window with the newest message received
        // End Result:  Will display the message received to this tcp based client
        private async void AddMessage(string msg)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, 
             () =>
             {
                 this.txtConversation.Text += string.Format(
                          Environment.NewLine + Environment.NewLine +
                          " >> {0}", msg);

             });
        }

        // Purpose:     Adds the " >> " prompt in the text box
        // End Result:  Shows prompt to user
        private void AddPrompt()
        {
            txtConversation.Text = txtConversation.Text +
                Environment.NewLine + " >> " + msg;
        }

        // Purpose:     Send the text in typed by the user (stored in
        //              txtOutMsg)
        // End Result:  Sends text message to node.js (lamechat.js)
        private async void cmdSendMessage_Click(object sender, RoutedEventArgs e)
        {

            serverStream = tcpClient.OutputStream.AsStreamForWrite();
            StreamWriter writer = new StreamWriter(serverStream);
            string request = txtOutMsg.Text;
            await writer.WriteLineAsync(request);
            await writer.FlushAsync();
            //byte[] outStream = Encoding.ASCII.GetBytes(txtOutMsg.Text);
            //serverStream.Write(outStream, 0, outStream.Length);
            //serverStream.Flush();
        }

    }
}
