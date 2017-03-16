using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyRobot.Model.Network
{
    public delegate void ClientHandler(StreamReader r, StreamWriter w);
    class MyServer
    {
        int port;
        ClientHandler ch;
        volatile bool stop;
        public MyServer(int port, ClientHandler ch, int maxTreads)
        {
            this.port = port;
            this.ch = ch;
            stop = false;
            ThreadPool.SetMaxThreads(maxTreads, 0);
        }
        public void start()
        {
            new Thread(delegate ()
            {
                TcpListener listner = null;
                try
                {
                    IPAddress localAddr = IPAddress.Parse("192.168.1.5");
                    listner = new TcpListener(localAddr, port);
                    System.Console.WriteLine("server ip:" + localAddr + ":" + port);
                    listner.Start();
                    while (!stop)
                    {
                       
                            Socket someClient = listner.AcceptSocket();//blocks
                            ThreadPool.QueueUserWorkItem(delegate (Object threadContext)
                            {
                                using (Socket threadSocket = someClient)
                                {
                                    using (Stream s = new NetworkStream(someClient))
                                    {
                                        ch(new StreamReader(s), new StreamWriter(s));
                                    }
                                }
                            });

                            System.Console.WriteLine("sleeping...");
                            Thread.Sleep(400);
                        
                    }
                    // listner.Stop();

                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                }
                finally
                {
                    // Stop listening for new clients.
                    listner.Stop();
                }
            }).Start();
        }



        public void stopServer()
        {
            stop = true;
        }



        //MUST ADD try
        //EXCEPTION when connectino lost during flash
       public static void MyClientHandler(StreamReader inFromClient, StreamWriter outToClient)
        {
            string line = string.Empty;
            while ((line = inFromClient.ReadLine()) != null){

                //  ).Equals("exit")))
                System.Console.WriteLine(line);
                string reverseLine = new string(line.ToCharArray().Reverse().ToArray());
                outToClient.WriteLine(reverseLine);
                outToClient.Flush();
            }
        }

    }
}
