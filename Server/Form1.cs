using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Net.NetworkInformation;

namespace Server
{
    public partial class Form1 : Form
    {
        Socket server, client;
        byte[] data = new byte[1024];


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipServer = new IPEndPoint(IPAddress.Any, 1234);
            server.Bind(ipServer);
            server.Listen(5);
            server.BeginAccept(new AsyncCallback(AcceptClient), server);

        }
        private void AcceptClient(IAsyncResult i)
        {
            client = ((Socket)i.AsyncState).EndAccept(i);
            client.BeginReceive(data, 0, data.Length, SocketFlags.None, new AsyncCallback(ReceiveData), client);
        }

        private void ReceiveData(IAsyncResult i)
        {
            ((Socket)i.AsyncState).EndReceive(i);
            this.Invoke((MethodInvoker)(() => listBox1.Items.Add("Client: " + Encoding.ASCII.GetString(data))));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text = txtMess.Text;
            listBox1.Items.Add("Server: " + text);
            txtMess.Text = "";
            byte[] data = new byte[1024];
            data = Encoding.ASCII.GetBytes(text);
            client.Send(data);
            data = new byte[1024];
            client.Receive(data);
            listBox1.Items.Add("Client: " + Encoding.ASCII.GetString(data));
        }
    }
}
