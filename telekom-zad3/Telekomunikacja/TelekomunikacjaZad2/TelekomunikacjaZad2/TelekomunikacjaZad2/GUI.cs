using System;
using System.Collections;
using System.ComponentModel.Design.Serialization;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using Telekomunikacja1;
using static System.Net.Mime.MediaTypeNames;
using System.Text;

namespace TelekomunikacjaZad2
{
    public partial class GUI : Form
    {
        FileOperations fileMenager = new FileOperations();
        Huffman huffman = new Huffman();
        EncodeFile fileEncoder = new EncodeFile();
        ReciveMessage fileReciever = new ReciveMessage();
        SendFile fileSender = new SendFile();
        Node tree = null;
        Serializer serializer = new Serializer();
        string bitCode = String.Empty;
        string text = String.Empty;
        string treeDictionary = String.Empty;

        public GUI()
        {
            InitializeComponent();
            Port2.Text = Convert.ToString(FreeTcpPort());  //displaying a free port 
            IPAddr2.Text = GetLocalIPAddress();           //displaying the local IP address
        }

        //loading a text file
        private void ReadButton_Click(object sender, EventArgs e)   
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "C:\\Users\\Hp\\Desktop\\pliczki";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFileName = openFileDialog1.FileName;
                text = fileMenager.readText(selectedFileName);

                StringText.Text = text;                     
            }
        }
        // encode the message to 0 and 1 and create a huffman tree and display the code dictionary
        //save file with bits
        private void CodeButton_Click(object sender, EventArgs e)       
        {
            if (text != String.Empty)
            {
                huffman.getFrequencies(text);
                treeDictionary = huffman.generateDictionary();
                List<HuffmanDictionary> dictionary = huffman.DictionaryList;
                bitCode = huffman.getHuffmanString(text, dictionary);
                tree = huffman.GenerateTree();
                BitText.Text = bitCode;
                DicionaryText.Text = treeDictionary;

                //SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                //saveFileDialog1.OverwritePrompt = true;

                //if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                //{
                  //  fileMenager.saveText(bitCode, saveFileDialog1.FileName);
               // }


            }
        }


        //send a file, provided that the recipient is waiting for a message and that the port and ip are given
        private void SendButton_Click(object sender, EventArgs e)       
        {
            byte[] send = fileEncoder.encodeHuffman(bitCode);

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.OverwritePrompt = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileMenager.saveBytes(send, saveFileDialog1.FileName);
            }

            Console.WriteLine("sums: " + string.Join(" ", send));
            if (text != String.Empty && IPAddr1.Text != "ip" && Port1.Text != "port")
            {
                string response = fileSender.sendMessage(IPAddr1.Text, Convert.ToInt32(Port1.Text), send);

            }
        }

        //waiting for a message
        private void RecieveButtonClick(object sender, EventArgs e)     
        {
            bitCode = fileReciever.getMessage(Convert.ToInt32(Port2.Text));
            StringText.Text = String.Empty;
            BitText.Text = bitCode;
        }

        // decode the file based on the code 0 and 1 and the binary tree
        private void DecodeButton_Click(object sender, EventArgs e)     
        {
            if (bitCode != String.Empty && tree != null)
            {
                text = fileEncoder.DecodeHuffman(bitCode, tree);
                StringText.Text = text;
            }
            if (text != String.Empty)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.OverwritePrompt = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    fileMenager.saveText(StringText.Text, saveFileDialog1.FileName);
                }
            }
        }

        //write the decoded file in the dialog box
        private void SaveButton_Click(object sender, EventArgs e)       
        {
            if (text != String.Empty)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.OverwritePrompt = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    fileMenager.saveText(StringText.Text, saveFileDialog1.FileName);
                }
            }
        }

        //receive the code tree
        private void RecieveTreeButton_Click(object sender, EventArgs e)    
        {
            tree = fileReciever.getTree(Convert.ToInt32(Port2.Text));
            treeDictionary = huffman.generateDictionaryForTransmition(tree);
            DicionaryText.Text = treeDictionary;
        }

        //broadcast the code tree provided that the recipient is listening
        private void SendTreeButton_Click(object sender, EventArgs e)      
        {
            if (tree != null)
            {
                serializer.serialize(tree);
                string SignString = serializer.Text;
                string FrequenciesStirng = serializer.Frequencies;

                string msg = SignString + "*" + FrequenciesStirng;
                Console.WriteLine(msg);
                byte[] bytes = Encoding.ASCII.GetBytes(msg);
                if (treeDictionary != String.Empty && IPAddr1.Text != "ip" && Port1.Text != "port")
                {
                    string response = fileSender.sendMessage(IPAddr1.Text, Convert.ToInt32(Port1.Text), bytes);

                }
            }
        }

        //function getting local ip address
        private static string GetLocalIPAddress()   
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        //function to get some free port
        private static int FreeTcpPort()       
        {
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void BitInfo_TextChanged(object sender, EventArgs e)
        {

        }

        private void DicionaryText_TextChanged(object sender, EventArgs e)
        {

        }

        private void DictionaryInfo_TextChanged(object sender, EventArgs e)
        {

        }
    }
}