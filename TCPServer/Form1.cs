using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCPServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Network MyServer = new Network();

        private void button1_Click(object sender, EventArgs e)
        {
            MyServer.RunServer();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            checkBox1.Checked = MyServer.ServerState; 
            if (MyServer.ReciveMessage.Length > 0)
            {
                listBox1.Items.Add(MyServer.ReciveMessage.ToString());

                MyServer.ReciveMessage.Clear();

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MyServer.SendMessage = new StringBuilder(textBox1.Text); 

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int MyPort = int.Parse(textBox3.Text);

            string IP = textBox2.Text;

            bool ConnectToServer = MyServer.ConnectToServer(MyPort, IP);

            checkBox2.Checked = ConnectToServer;


            if (ConnectToServer)
            {
                MyServer.ClientServerTalk();
            }
        }

    }
}
