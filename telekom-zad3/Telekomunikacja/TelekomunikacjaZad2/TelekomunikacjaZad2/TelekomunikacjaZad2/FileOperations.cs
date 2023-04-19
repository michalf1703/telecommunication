using System.Net.Sockets;
using System.Net;
using System.Text;
using TelekomunikacjaZad2;

namespace Telekomunikacja1
{
    internal class FileOperations      
    {
        public byte[] readBytes(string path) { return File.ReadAllBytes(path); }                    

        public void saveBytes(byte[] data, string filePath) => File.WriteAllBytes(filePath, data);  

        public string readText(string filePath) => File.ReadAllText(filePath);                     

        public void saveText(string data, string filePath) { File.WriteAllText(filePath, data); }   
    }
    internal class FileReciever         
    {
        FileEncoder fileEncoder = new FileEncoder();

        public string recieveMessage(int port)                                  
        {
            TcpListener listener = new TcpListener(IPAddress.Any, port);        
            listener.Start();                                                   
            TcpClient client = listener.AcceptTcpClient();                      
            NetworkStream stream = client.GetStream();                          
            StreamReader streamReader = new StreamReader(stream);               
            StreamWriter streamWriter = new StreamWriter(stream);

            string Encodedmessage = string.Empty;                               
            try
            {
                byte[] buffer = new byte[1024];                                 
                stream.Read(buffer, 0, buffer.Length);                          
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
                Encodedmessage = fileEncoder.getStringFromBytes(bufferReduced); 
                if (Encodedmessage.Length > 0)
                {
                    streamWriter.WriteLine("Dostałem wiadomość");               
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

        public Node recieveTree(int port)                        
        {
            string stringOfFrequencies = string.Empty;                  
            string stringOfSigns = string.Empty;                        

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
                message = Encoding.ASCII.GetString(bufferReduced);      
            }
            catch (Exception ex)
            {

            }
            stringOfSigns = message.Substring(0, message.IndexOf('*'));          
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
            serializer.makeLists(stringOfSigns, stringOfFrequencies);           
            Node node = serializer.deserialize();                       
            return node;                                                        
        }
    }
    internal class FileSender   
    {
        public string sendMessage(string ip, int port, byte[] file)         
        {
            string response = string.Empty; 
            try
            {
                TcpClient tcpClient = new TcpClient(ip, port);      

                NetworkStream stream = tcpClient.GetStream();               
                stream.Write(file, 0, file.Length);                         
                StreamReader streamReader = new StreamReader(stream);       
                response = streamReader.ReadLine();                         
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

    class FileEncoder       
    {

        private bool[] getBitsFromBytes(byte[] bytes)   
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

        public string getStringFromBytes(byte[] bytes)  
        {
            string retStr = "";
            bool[] bits = getBitsFromBytes(bytes);
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

        private byte ConvertBoolArrayToByte(bool[] source)  
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

        private byte ConvertStringOf8ZerosAndOnesToByte(string str)      
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
            return ConvertBoolArrayToByte(bools);
        }


        public byte[] HuffmanEncoder(string str)        
        {
            var neededZeros = 0;                       
            if (str.Length % 8 != 0)
            {
                neededZeros = 8 - str.Length % 8;
            }

            if (neededZeros > 0)
                str = string.Concat(str, new string('0', neededZeros)); 

            byte[] retBytes = new byte[str.Length / 8];

            string temp = "";
            for (int i = 0; i < str.Length; i += 8)
            {
                for (int j = 0; j < 8; j++)
                {
                    temp += str[i + j];
                }
                retBytes[i / 8] = ConvertStringOf8ZerosAndOnesToByte(temp); 
                temp = "";
            }

            return retBytes;
        }

        public string HuffmanDecoder(string bits, Node tree)         
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
