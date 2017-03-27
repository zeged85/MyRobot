using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace MyRobot.Model.Network
{
    class WebSocket
    {
        public static void StartServer()
        {
            TcpListener server = new TcpListener(IPAddress.Any, 9998);

            server.Start();
            Console.WriteLine("Server has started on " + HTTPServer.GetLocalIPAddress() +":80.{0}Waiting for a connection...", Environment.NewLine);

            TcpClient client = server.AcceptTcpClient();

            Console.WriteLine("A client connected.");

            NetworkStream stream = client.GetStream();

            //enter to an infinite cycle to be able to handle every change in stream
            while (true)
            {
                while (!stream.DataAvailable) ;

                Byte[] bytes = new Byte[client.Available];

                stream.Read(bytes, 0, bytes.Length);



                //translate bytes of request to string
                String data = Encoding.UTF8.GetString(bytes);

                Console.WriteLine(data);

                //Handshake
                if (new Regex("^GET").IsMatch(data)) {
                    Byte[] response = Encoding.UTF8.GetBytes(
                        "HTTP/1.1 101 Switching Protocols" + Environment.NewLine
                       + "Connection: Upgrade" + Environment.NewLine
                       + "Upgrade: websocket" + Environment.NewLine
                       + "Sec-WebSocket-Accept: "
                       + Convert.ToBase64String(SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(new Regex("Sec-WebSocket-Key: (.*)").Match(data).Groups[1].Value.Trim() + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"))) + Environment.NewLine
                       + Environment.NewLine);

    stream.Write(response, 0, response.Length);
                }
                else
                {//respond
                    Console.WriteLine("New frame arrived!");
                    Console.Write("|");
                    foreach (byte b in bytes)
                    {
                        Console.Write(b + "|");
                    }
                    Console.WriteLine();


                    Byte bit1 = bytes[0]; //129 == new text frame
                    
                  ///FIN (Is this the whole message?)	RSV1	RSV2	RSV3	Opcode
                  /// 1                                  0       0       0     0x1 = 0001
                    if (bit1 == 129)
                    {
                        Console.WriteLine("129 - single text frame");
                    }


                    Byte bit2 = bytes[1]; //If the second byte minus 128 is between 0 and 125, this is the length of message. If it is 126, the following 2 bytes (16-bit unsigned integer), if 127, the following 8 bytes (64-bit unsigned integer) are the length.

                    int length = bit2 - 128;
                    if (length > 125)
                    {
                        Console.WriteLine("odd size");
                        if (length == 126)
                        {

                        }

                        if (length == 127)
                        {

                        }

                    }
                    else
                    {
                        Console.WriteLine("normal size:" + length + " Bytes.");
                        
                    }

                    Byte[] decoded = new Byte[length];
                    Byte[] encoded = new Byte[length];

                    for (int i = 0; i<length; i++)
                    {
                        encoded[i] = bytes[i + 6];
                    }
                    Byte[] key = new Byte[4] { bytes[2], bytes[3], bytes[4], bytes[5]};

                    for (int i = 0; i < encoded.Length; i++)
                    {
                        decoded[i] = (Byte)(encoded[i] ^ key[i % 4]);
                    }

                    Console.WriteLine();
                    Console.Write("Message decoded:");
                    String message = Encoding.UTF8.GetString(decoded);
                    Console.WriteLine(message);





                    ///Encoding and sending commands

                   // byte[] newMessage = new byte[8] {129, 130, bytes[2], bytes[3], bytes[4], bytes[5], 128 , 129};

                    //stream.Write(newMessage, 0, newMessage.Length);
                }

            }
        }
    }
}
