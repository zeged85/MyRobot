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
                    // Set the TcpListener on port 13000.
                 //   Int32 port = 13000;
                    //    IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                    // IPAddress localAddr = Dns.Resolve("localhost").AddressList[0];
                    IPAddress localAddr = Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);



                    // TcpListener server = new TcpListener(port);
                    listner = new TcpListener(localAddr, port);

                      System.Console.WriteLine("server ip:" + localAddr + ":" + port);
                    listner.Start();
                    while (!stop)
                    {
                        if (listner.Pending())
                        {
                            Socket someClient = listner.AcceptSocket();//blocks
                            Stream s = new NetworkStream(someClient);
                            ThreadPool.QueueUserWorkItem(delegate (Object threadContext)
                            {

                                //communicate over stream
                                ch(new StreamReader(s), new StreamWriter(s));


                                s.Close();
                                someClient.Close();
                            });
                       
                        }
                        else
                        {
                            System.Console.WriteLine("sleeping...");
                            Thread.Sleep(5000);
                        }
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
