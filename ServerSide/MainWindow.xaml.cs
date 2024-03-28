using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace ServerSide
{
    public partial class MainWindow : Window
    {
        private Socket serverSocket;
        private Thread receiveThread;

        public MainWindow()
        {
            InitializeComponent();
            SetupServer();
        }

        private void SetupServer()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 5357);
            serverSocket.Bind(endPoint);

            receiveThread = new Thread(new ThreadStart(ReceiveData));
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }

        private void ReceiveData()
        {
            byte[] buffer;
            IPEndPoint senderEndPoint = new IPEndPoint(IPAddress.Any, 0);
            EndPoint remoteEndPoint = (EndPoint)senderEndPoint;
            while (true)
            {
                buffer = new byte[1024];
                try
                {
                    int receivedLength = serverSocket.ReceiveFrom(buffer, ref remoteEndPoint);
                    string receivedMessage = Encoding.ASCII.GetString(buffer, 0, receivedLength);
                    if (receivedMessage.StartsWith("[NAME]"))
                    {
                        Dispatcher.Invoke(() =>
                        {
                            connectedname.Content = receivedMessage.Substring(6);
                        });
                    }
                    else if (receivedMessage.StartsWith("[MESSAGE]"))
                    {
                        Dispatcher.Invoke(() =>
                        {
                            serverListBox.Items.Add("Username: " + connectedname.Content + " Message: " + receivedMessage.Substring(9));
                        });
                    }
                }
                catch (SocketException ex)
                {
                    MessageBox.Show("Socket error: " + ex.Message);
                }
            }
        }
    }
}
