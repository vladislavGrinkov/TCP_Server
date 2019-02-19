using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TCPServer
{
    class Network
    {
       public  bool ServerState = false;
        TcpClient client;

        public StringBuilder SendMessage = new StringBuilder();
        public StringBuilder ReciveMessage = new StringBuilder();



        void ReciveTalk()
        {
            StringBuilder Str = new StringBuilder();

            while (client.Connected)
            {
                Str = new StringBuilder(SendRecive("", false));

                if (Str.Length > 0)
                {
                    ReciveMessage = Str;
                }
                
            }
        }


        void SendTask()
        {
            while (client.Connected)
            {
                if(SendMessage.Length > 0)
                {
                    SendRecive(SendMessage.ToString(), true);

                    SendMessage.Clear();
                }
            }
        }
        


        public void ClientServerTalk()
        {
            Task  TalkSendTask = new Task(SendTask);
            Task  TalkReciveTask = new Task(ReciveTalk);

            TalkSendTask.Start();
            TalkReciveTask.Start();

        }



        public string Byte_convert(byte[] Recive)
        {
            int buf = Recive.Length;

            string R = "";  // хранение  результата 
            int F = 0;

            for (int i = 0; i < buf; i++)
            {
                if (Recive[i] != 0)
                {
                    F++;
                }
            }

            R = System.Text.Encoding.ASCII.GetString(Recive); // байты в строку
            R = R.Substring(0, F);

            return R;
        }



        public string SendRecive(string text, bool send)
        {
            string otvet = "";
            byte[] O = new byte[text.Length];


            try
            {
                switch (send)
                {
                    case true:
                        O = System.Text.Encoding.ASCII.GetBytes(text);
                        client.Client.Send(O);
                        break;

                    case false:
                        O = new byte[256];
                        client.Client.Receive(O);

                        otvet = Byte_convert(O);

                  
                        break;
                }
            }
            catch
            {
            }

            return otvet;
        }


        void Lisen()
        {
            ServerState = true; 
            IPAddress localAdress = IPAddress.Parse("0.0.0.0");
            int Port = 7777;

            TcpListener liss = new TcpListener(localAdress, Port);

            liss.Start();

            while (ServerState)
            {
                client = liss.AcceptTcpClient();

                ClientServerTalk();

            }

        }

        public void RunServer()
        {
            ServerState = true;

            Task s = new Task(Lisen);


            s.Start();

        }

        public bool ConnectToServer(int ServerPort, string ServerIPAdress)
        {
            try
            {
                client = new TcpClient();

                client.Connect(IPAddress.Parse(ServerIPAdress), ServerPort);

                return client.Connected;
            }
            catch(Exception ex)
            {
                ReciveMessage = new StringBuilder(ex.Message);
            }

            return false;



            
        }

    }
}
