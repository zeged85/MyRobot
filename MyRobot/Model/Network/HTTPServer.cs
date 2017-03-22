using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyRobot.Model.Network
{
    public class HTTPServer
    {
        /// <summary>
        /// http://www.java2s.com/Code/CSharp/Network/SimpleTcpServer.htm
        /// </summary>
        public static void StartServer()
        {
            while (true) { 
            int recv;
            byte[] data = new byte[1024];
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any,
                                   80);

            Socket newsock = new
                Socket(AddressFamily.InterNetwork,
                            SocketType.Stream, ProtocolType.Tcp);

            newsock.Bind(ipep);
            newsock.Listen(10);

                bool online = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

                Console.WriteLine("Waiting for a client on ip:"+ GetLocalIPAddress() + " port:80");


                ///http://stackoverflow.com/questions/6803073/get-local-ip-address
                ///

                


       // To check if you're connected or not:


                Socket client = newsock.Accept();
            IPEndPoint clientep =
                         (IPEndPoint)client.RemoteEndPoint;
            Console.WriteLine("Connected with {0} at port {1}",
                            clientep.Address, clientep.Port);


            //Encoding.UTF8.GetBytes(request_url)



            string httpHeader = "HTTP/1.1 200 OK\r\nContent-Type: text/html\r\n\r\n";

            data = Encoding.UTF8.GetBytes(httpHeader);
            // data = Encoding.ASCII.GetBytes(welcome);

            //  client.Send(data, data.Length, SocketFlags.None);



            //string mainPage = "<!doctype html><head><meta name=\"viewport\" content=\"initial-scale=1, maximum-scale=1\"><link rel=\"stylesheet\" href=\"http://code.jquery.com/mobile/1.4.0/jquery.mobile-1.4.0.min.css\" /><script src=\"http://code.jquery.com/jquery-1.9.1.min.js\"></script><script src=\"http://code.jquery.com/mobile/1.4.0/jquery.mobile-1.4.0.min.js\"></script></head><style>h3, h4 {text-align: center;}span {font-weight: bold;}</style><div data-role=\"page\" data-theme=\"b\"><div data-role=\"header\"><div><h3>ESP8266 Web Control</h3></div></div><div data-role=\"content\"><form><p>The button is <span id=\"buttonState\"></span></p><br><select name=\"flip-1\" id=\"flip-1\" data-role=\"slider\" style=\"float: left;\"><option value=\"off\">LED off</option><option value=\"on\">LED on</option></select></form></div> <div data-role=\"footer\"><div><h4>ESP8266</h4></div></div><script type=text/javascript>$( document ).ready(function() {$('#flip-1').change(function() { if($('#flip-1').val()==\"off\"){$.get(\"/LED=OFF\", function(data, status) {});}else{ $.get(\"/LED=ON\", function(data, status) {});}}); });</script></div>";



                string htmlPath = @"C:\Users\Nir\OneDrive\Documents\Smart Home\MyRobot\MyRobot\main.html";
                string htmlFile = File.OpenText(htmlPath).ReadToEnd();
         
            

            data = Encoding.UTF8.GetBytes(httpHeader + htmlFile);
            client.Send(data, data.Length, SocketFlags.None);




            //   while (true)
            //   {
            byte[] response;
            response = new byte[1024];
            recv = client.Receive(response); //BLOCKING
                                             //      if (recv == 0)
                                             //          break;

            Console.WriteLine(
                     Encoding.ASCII.GetString(response, 0, recv));

            string resp = Encoding.ASCII.GetString(response, 0, recv);

            string line = resp.Split('\r')[0];

            Console.WriteLine(line);

                //CLIENT INPUT COMMANDS TO SERVER
            if (line.IndexOf("/LED=ON") != -1)
            {
                //   digitalWrite(LED, LOW);
                //   sendRespond("got on request");
            }
            else
            if (line.IndexOf("/LED=OFF") != -1)
            {
                //  digitalWrite(LED, HIGH);
                //  sendRespond("got on off");
            }
            else
            {
                //   sendRespond(mainpage);
            }


            //   data = Encoding.UTF8.GetBytes(httpHeader + mainPage);


            //       client.Send(data, recv, SocketFlags.None);

            //      break;

            //      }



                //SERVER OUTPUT COMMANDS INTO CLIENT INPUT

                //1.LOOP CYCLE
                //2.FLUSHING


            Console.WriteLine("Disconnected from {0}",
                              clientep.Address);
            client.Close();
            newsock.Close();
        }
        }
        //To get local Ip Address:

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
    }

}
