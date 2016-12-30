using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyRobot.Model
{
    class MyTelnetClient : ITelnetClient
    {
        string ip;
        int port;
        TcpClient client;
        StreamReader inFromServer;
        StreamWriter outToServer;

        public MyTelnetClient(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }
        public void connect(string ip, int port)
        {
            client = new TcpClient(ip, port);
            inFromServer = new StreamReader(client.GetStream());
            outToServer = new StreamWriter(client.GetStream());
        }

        public void disconnect()
        {
            inFromServer.Close();
            outToServer.Close();
            client.GetStream().Close();
            client.Close();
        }

        public string read()
        {
            return inFromServer.ReadLine();
        }

        public void write(string command)
        {
            outToServer.WriteLine(command);
            outToServer.Flush();
        }
    }
}
