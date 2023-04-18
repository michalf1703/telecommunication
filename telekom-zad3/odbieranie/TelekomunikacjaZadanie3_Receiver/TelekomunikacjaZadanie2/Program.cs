using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace HuffmanTest
{
    class Program
    {
        public static int Main(String[] args)
        {
            StartServer();
            return 0;
        }


        public static void StartServer()
        {
            // Get Host IP Address that is used to establish a connection  
            // In this case, we get one IP address of localhost that is IP : 127.0.0.1  
            // If a host has multiple addresses, you will get a list of addresses  
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 6969);


            try
            {

                // Create a Socket that will use Tcp protocol      
                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                // A Socket must be associated with an endpoint using the Bind method  
                listener.Bind(localEndPoint);
                // Specify how many requests a Socket can listen before it gives Server busy response.  
                // We will listen 10 requests at a time  
                listener.Listen(10);

                Console.WriteLine("Waiting for a connection...");
                Socket handler = listener.Accept();


                /* WERSJA PIERWSZA Z EOF
                string dataTree = null;
                byte[] treeBytes = null;
                while (true)
                {
                    treeBytes = new byte[20];
                    int bytesRecTree = handler.Receive(treeBytes);

                    if (dataTree.IndexOf("<EOF>") > -1)
                    {
                        break;
                    }
                }
                string dataTreeFixed = dataTree.Remove(5);
                Console.Write(dataTreeFixed);
                HuffmanTree huffmanTree = new HuffmanTree();
                huffmanTree.deserializeTree(dataTree);
                Console.Write("Dziala tutaj - przeslalo sie drzewo");

                string data = null;
                byte[] bytes = null;

                bytes = new byte[256];
                int bytesRec = handler.Receive(bytes);
                */

                byte[] bytesMSGTree = new byte[4];
                handler.Receive(bytesMSGTree);
                int bytesSizeTree = 0;
                bytesSizeTree = BitConverter.ToInt32(bytesMSGTree, 0);

                string dataTree = null;
                byte[] treeBytes = null;
                treeBytes = new byte[bytesSizeTree];
                int bytesRecTree = handler.Receive(treeBytes);

                Console.Write(treeBytes);
                HuffmanTree huffmanTree = new HuffmanTree();
                huffmanTree.deserializeTree(treeBytes);
                Console.Write("Dziala tutaj - przeslalo sie drzewo");

                byte[] bytesMSG = new byte[4];
                handler.Receive(bytesMSG);
                int bytesSize = 0;
                bytesSize = BitConverter.ToInt32(bytesMSG, 0);

                string data = null;
                byte[] bytes = null;

                bytes = new byte[bytesSize];
                int bytesRec = handler.Receive(bytes);
                


                /*while (true)
                {
                    bytes = new byte[256];
                    int bytesRec = handler.Receive(bytes, handler.Available,
                                           SocketFlags.None);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    if (data.IndexOf("<EOF>") > -1)
                    {
                        break;
                    }
                }*/

                BitArray encoded;

                encoded = new BitArray(bytes);
                Console.Write("Still encoded?: ");
                foreach (bool bit in encoded)
                {
                    Console.Write((bit ? 1 : 0) + "");
                }
                Console.WriteLine();

                do
                {
                    while (!Console.KeyAvailable)
                    {
                        // Do something
                    }
                } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

                huffmanTree.FixTree();
                string decoded = huffmanTree.Decode(encoded);
                Console.Write(decoded);
                File.WriteAllText("E:\\4_SEM\\Telekomuna\\Top_secret\\TelekomunikacjaZadanie3_Receiver\\TelekomunikacjaZadanie2\\outputData.txt", decoded, Encoding.UTF8);

                Console.WriteLine("--- FILE CODED AND WRITTEN TO FILE. ENOJY! ---");

                Console.ReadLine();

                //Console.WriteLine("Text received : {0}", data);

                byte[] msg = Encoding.ASCII.GetBytes(data);
                handler.Send(msg);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\n Press any key to continue...");
            Console.ReadKey();
        }
    }
}