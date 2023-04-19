using System.Net.Sockets;
using System.Net;
using System.Text;
using TelekomunikacjaZad2;

namespace Telekomunikacja1
{
    //methods for reading and writing files
    internal class FileOperations      
    {
        public byte[] readBytes(string path) { return File.ReadAllBytes(path); }                    

        public void saveBytes(byte[] data, string filePath) => File.WriteAllBytes(filePath, data);  

        public string readText(string filePath) => File.ReadAllText(filePath);                     

        public void saveText(string data, string filePath) { File.WriteAllText(filePath, data); }   
    }
    //class implements a socket server that listens and waits for a client connect.
    internal class ReciveMessage         
    {
        EncodeFile fileEncoder = new EncodeFile();

        //getMessage ->a function that receives a reduced coded message
        public string getMessage(int port)   //number of port                                 
        {
            //tcpListner -> listens on the port passed as an argument to the function.
            TcpListener listener = new TcpListener(IPAddress.Any, port);        
            listener.Start();                                       //start listening                           
            TcpClient client = listener.AcceptTcpClient();          // creating a client that accepts tcp connections                  
            NetworkStream stream = client.GetStream();              // create a stream                                                                
            StreamReader streamReader = new StreamReader(stream);   //create a sender and recipient of the message           
            StreamWriter streamWriter = new StreamWriter(stream);

            string Encodedmessage = string.Empty;                              
            try
            {
                byte[] buffer = new byte[1024];                  //create a buffor               
                stream.Read(buffer, 0, buffer.Length);           //save the message into the buffer              
                int recieved = 0;
                foreach (byte b in buffer)                       //cut zero-bytes               
                {
                    if (b != 0)
                    {
                        recieved++;
                    }
                }
                byte[] bufferReduced = new byte[recieved];
                for (int i = 0; i < recieved; i++)              //copying without redundant bytes                           
                {
                    bufferReduced[i] = buffer[i];
                }
                Encodedmessage = fileEncoder.getString(bufferReduced);     //Converting the received bytes into a sequence of zeros and ones
                if (Encodedmessage.Length > 0)
                {             
                    streamWriter.Flush();
                    streamWriter.Close();
                    streamReader.Close();
                    stream.Close();
                    client.Close();
                    listener.Stop();
                    return Encodedmessage;
                }
            }
            catch (Exception ex)
            {

            }

            streamWriter.Flush();
            streamWriter.Close();
            streamReader.Close();
            stream.Close();
            client.Close();
            listener.Stop();
            return Encodedmessage;
        }

        //getTree function implements a socket server that listens on a specific port and waits for a client to connect.
        //a function that receives a binary tree
        public Node getTree(int port)                                     
        {
            string stringOfFrequencies = string.Empty;            //part of the message with frequencies      
            string stringOfSigns = string.Empty;                  //part of the message with characters     

            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            TcpClient client = listener.AcceptTcpClient();
            NetworkStream stream = client.GetStream();
            StreamReader streamReader = new StreamReader(stream);
            StreamWriter streamWriter = new StreamWriter(stream);
            string message = string.Empty;
            try
            {
                byte[] buffer = new byte[1024];
                stream.Read(buffer, 0, buffer.Length);

                stream.Flush();
                int recieved = 0;
                foreach (byte b in buffer)
                {
                    if (b != 0)
                    {
                        recieved++;
                    }
                }
                byte[] bufferReduced = new byte[recieved];
                for (int i = 0; i < recieved; i++)
                {
                    bufferReduced[i] = buffer[i];
                }
                message = Encoding.ASCII.GetString(bufferReduced);      //converting the bytes with the message to a string
            }
            catch (Exception ex)
            {

            }
            stringOfSigns = message.Substring(0, message.IndexOf('*'));                 //cut the string into 2 parts one with characters one with frequencies
            stringOfFrequencies = message.Substring(message.IndexOf('*') + 1);    
            streamWriter.Flush();                                               
            stream.Flush();
            streamWriter.Close();
            streamReader.Close();
            stream.Close();
            client.Close();
            listener.Stop();
            Console.WriteLine(stringOfSigns);
            Console.WriteLine(stringOfFrequencies);
            Serializer serializer = new Serializer();                   
            serializer.makeLists(stringOfSigns, stringOfFrequencies);               // create lists from received strings
            Node node = serializer.deserialize();                                  // deserialization from lists
            return node;                                                            //return tree-node
        }
    }
    //a class that transmits a message over socket
    internal class SendFile   
    {
        //send message function
        public string sendMessage(string ip, int port, byte[] file)         
        {
            string response = string.Empty; 
            try
            {
                //creating a client (recipient) based on its port and local ip provided by the second user of the program
                TcpClient tcpClient = new TcpClient(ip, port);      

                NetworkStream stream = tcpClient.GetStream();                    //opening the stream for transmission      
                stream.Write(file, 0, file.Length);                             //sending a message
                StreamReader streamReader = new StreamReader(stream);          //creating a reader that reads the server's (recipient's) responses
                response = streamReader.ReadLine();                           //reading the answer
                while (response == string.Empty)
                {
                    response = streamReader.ReadLine();
                }
                streamReader.Close();                                      
                stream.Close();
                tcpClient.Close();
            }
            catch (Exception ex)
            {
                
            }
            return response;
        }
    }

    class EncodeFile   //class that converts data types    
    {

        //Function to convert byte to 8 bits
        private bool[] getBits(byte[] bytes)   
        {
            bool[] bits = new bool[bytes.Length * 8];
            for (int i = 0; i < bytes.Length; i++)
            {
                bool[] BoolArray = new bool[8];
                for (int j = 0; j < 8; j++)
                    BoolArray[j] = (bytes[i] & (1 << j)) != 0;  
                Array.Reverse(BoolArray);                       
                for (int j = 0; j < 8; j++)
                    bits[(i * 8) + j] = BoolArray[j];
            }
            return bits;
        }


        //Function that generates a string 0 and 1 from a byte
        public string getString(byte[] bytes)  
        {
            string retStr = "";
            bool[] bits = getBits(bytes);
            for (int i = 0; i < bits.Length; i++)
            {
                if (bits[i] == true)
                {
                    retStr += '1';
                }
                else
                {
                    retStr += '0';
                }
            }
            return retStr;
        }

        // the inverse of the first function converts the bool array to a byte
        private byte BoolArrayToByte(bool[] source)  
        {
            byte result = 0;
            int index = 8 - source.Length;

            foreach (bool b in source)
            {
                if (b)
                    result |= (byte)(1 << (7 - index));

                index++;
            }

            return result;
        }


        //Function used to encode the file to a smaller size creating a byte from a string with zeros and ones
        private byte compression(string str)      
        {
            bool[] bools = new bool[8];

            for (int i = 0; i < 8; i++)
            {
                if (str[i] == '1')
                {
                    bools[i] = true;
                }
                else
                {
                    bools[i] = false;
                }
            }
            return BoolArrayToByte(bools);
        }

       
        //Function encoding the message with huffman code
        public byte[] encodeHuffman(string str)        
        {
            var neededZeros = 0;                    // calculate how many zeros to add to make the message divisible by 8
            if (str.Length % 8 != 0)
            {
                neededZeros = 8 - str.Length % 8;
            }

            if (neededZeros > 0)
                str = string.Concat(str, new string('0', neededZeros));     //add zeros

            byte[] retBytes = new byte[str.Length / 8];

            string temp = "";
            for (int i = 0; i < str.Length; i += 8)
            {
                for (int j = 0; j < 8; j++)
                {
                    temp += str[i + j];
                }
                retBytes[i / 8] = compression(temp); 
                temp = "";
            }

            return retBytes;
        }

        public string DecodeHuffman(string bits, Node tree)         
        {
            string str = "";
            for (int i = 0; i < bits.Length; i++)
            {
                Node root = tree;
                int j = 0;
                while (root.Sign == '$')                                    
                {
                    if (i + j > bits.Length - 1)
                    {
                        return str;
                    }
                    if (bits[i + j] == '1')
                    {
                        root = root.Right;
                    }
                    else
                    {
                        root = root.Left;
                    }
                    j++;
                }
                i += j - 1;
                str += root.Sign;                                         
            }
            return str;
        }
    }
}
