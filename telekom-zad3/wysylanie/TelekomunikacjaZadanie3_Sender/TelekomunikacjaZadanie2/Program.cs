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
            StartClient();
            return 0;
        }


        public static void StartClient()
        {
            byte[] bytes = new byte[1024];

            try
            {
                // Connect to a Remote server  
                // Get Host IP Address that is used to establish a connection  
                // In this case, we get one IP address of localhost that is IP : 127.0.0.1  
                // If a host has multiple addresses, you will get a list of addresses  
                IPHostEntry host = Dns.GetHostEntry("DESKTOP-DC90A2U");
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 6969);

                // Create a TCP/IP  socket.    
                Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.    
                try
                {
                    // Connect to Remote EndPoint  
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());

                    string input = File.ReadAllText("C:\\Users\\Hp\\Desktop\\wysylanie\\TelekomunikacjaZadanie3_Sender\\TelekomunikacjaZadanie2\\inputData.txt");
                    Console.WriteLine("--- FILE READ FROM FILE ---");
                    //File.WriteAllText("E:\\4_SEM\\Telekomuna\\Top_secret\\TelekomunikacjaZadanie3\\TelekomunikacjaZadanie2\\TelekomunikacjaZadanie2\\inputData.txt", input, Encoding.UTF8);
                    HuffmanTree huffmanTree = new HuffmanTree();

                    // Build the Huffman tree
                    huffmanTree.Build(input);

                    // Encode
                    BitArray encoded = huffmanTree.Encode(input);

                    // Encode the data string into a byte array.    
                    byte[] msg = Util.BitArrayToByteArray(encoded);

                    Console.Write("Encoded: ");
                    foreach (bool bit in encoded)
                    {
                        Console.Write((bit ? 1 : 0) + "");
                    }
                    Console.WriteLine();

                    // Send the data through the socket.
                    //msg = Encoding.ASCII.GetBytes("This is a test<EOF>");

                    byte [] bytestest = huffmanTree.prepareTree();
                    //int bytesTestLength = bytestest.Length;
                    //sender.Send(bytesTestLength);
                    byte[] treeBytesLength = BitConverter.GetBytes(bytestest.Length);
                    sender.Send(treeBytesLength);
                    sender.Send(bytestest);
                    /*huffmanTree.deserializeTree(bytestest);
                    string decoded = huffmanTree.Decode(encoded);
                    Console.Write(decoded);*/

                    byte[] bytesMSG = BitConverter.GetBytes(msg.Length);
                    sender.Send(bytesMSG);
                    int bytesSent = sender.Send(msg);
                    Console.Write("Dziala tutaj");

                    // Receive the response from the remote device.    
                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine("Echoed test = {0}",
                        Encoding.ASCII.GetString(bytes, 0, bytesRec));

                    // Release the socket.    
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}

        /*static void Main(string[] args)
        {
            Console.WriteLine("Program by Filip Izydorczyk & Karolina Zaborowska");
            string input = File.ReadAllText("E:\\4_SEM\\Telekomuna\\Top_secret\\TelekomunikacjaZadanie3\\TelekomunikacjaZadanie2\\inputData.txt");
            Console.WriteLine("--- FILE READ FROM FILE ---");
            //File.WriteAllText("E:\\4_SEM\\Telekomuna\\Top_secret\\TelekomunikacjaZadanie3\\TelekomunikacjaZadanie2\\TelekomunikacjaZadanie2\\inputData.txt", input, Encoding.UTF8);
            HuffmanTree huffmanTree = new HuffmanTree();

            // Build the Huffman tree
            huffmanTree.Build(input);

            // Encode
            BitArray encoded = huffmanTree.Encode(input);

            Console.Write("Encoded: ");
            foreach (bool bit in encoded)
            {
                Console.Write((bit ? 1 : 0) + "");
            }
            Console.WriteLine();

            //test--------------------------

            Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            System.Net.IPAddress ipAdd = System.Net.IPAddress.Parse("127.0.0.1");
            System.Net.IPEndPoint remoteEP = new IPEndPoint(ipAdd, 12025);
            soc.Connect(remoteEP);

            if (soc == null)
            Console.WriteLine("Connection failed");

            int caseSwitch = 0;
            Console.WriteLine("Choose 1 if you want to send a message or 2 if you want to receive it");
            caseSwitch = int.Parse(Console.ReadLine());

            switch (caseSwitch)
            {
                case 1:
                    byte[] textConverted = Util.BitArrayToByteArray(encoded);   //any message must be serialized (converted to byte array)

                    //byte[] byData = System.Text.Encoding.ASCII.GetBytes("un:" + username + ";pw:" + password);
                    soc.Send(textConverted);
                    //soc.BeginAccept();
                    break;

                case 2:
                    byte[] buffer = new byte[1024];

                   

                    int bytes = 0;

                    do
                    {
                        bytes = soc.Receive(buffer, buffer.Length, 0);
                        encoded = new BitArray(soc.Receive(buffer));
                    } while (bytes > 0);

                    encoded = new BitArray(soc.Receive(buffer));
                    Console.Write("Still encoded?: ");
                    foreach (bool bit in encoded)
                    {
                        Console.Write((bit ? 1 : 0) + "");
                    }
                    Console.WriteLine();
                    string decoded = huffmanTree.Decode(encoded);
                    File.WriteAllText("E:\\4_SEM\\Telekomuna\\Top_secret\\TelekomunikacjaZadanie3\\TelekomunikacjaZadanie2\\outputData.txt", decoded, Encoding.UTF8);

                    Console.WriteLine("--- FILE CODED AND WRITTEN TO FILE. ENOJY! ---");

                    Console.ReadLine();
                    break;
            }

            //test end-----------------------

            // Decode

        }
    }*/