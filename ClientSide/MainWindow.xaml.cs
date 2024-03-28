using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace ClientSide
{
    public partial class MainWindow : Window
    {
        private Socket clientSocket;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string clientName = clientNameTextBox.Text;
                byte[] data = Encoding.ASCII.GetBytes("[NAME]" + clientName);
                IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse("192.168.0.105"), 5357);
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                clientSocket.SendTo(data, serverEP);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void Button_Send_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string message = clientMessageTextBox.Text;
                byte[] data = Encoding.ASCII.GetBytes("[MESSAGE]" + message);
                IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse("192.168.0.105"), 5357);
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                clientSocket.SendTo(data, serverEP);
                clientMessageTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
