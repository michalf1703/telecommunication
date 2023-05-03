
namespace TelekomunikacjaZad2
{
    // logic of huffman coding and creating a binary tree
    internal class Huffman  
    {
        List<Char> charList = new List<Char>();
        List<int> freqList = new List<int>();
        List<HuffmanDictionary> dictionaryList = new List<HuffmanDictionary>();
        internal List<HuffmanDictionary> DictionaryList { get => dictionaryList;}


        //Function to count the occurrences of each character to create a huffman binary tree

        public void getFrequencies(string text)                      
        {
            freqList = new List<int>();                                 //List of their frequencies                  
            charList = new List<Char>();                                //List containing chars without repetition                     

            for (int i = 0; i < text.Length ; i++)                      //Add to the list of chars that are not already in it
            {
                if (!charList.Contains(text[i]))
                    charList.Add(text[i]);
            }
            
            for (int i = 0;i < charList.Count ; i++)                   //Add each char in the char list to the frequency list
            {
                Char c = charList[i];
                int freq = text.Count(f => f == c);
                freqList.Add(freq);
            }
            for (int i = 0  ; i < freqList.Count ; i++)
            {
                Console.WriteLine(charList[i] + ": " + freqList[i]);
            }            
        }
        //The function that generates the huffman binary tree
        public Node GenerateTree()                                       
        {
            Node left, right, top;

            List<Node> huffmanList = new  List<Node>();

            for (int i = 0 ; i<charList.Count ; i++)
            {
                huffmanList.Add(new Node(freqList[i], charList[i]));                 //Create a List with huffman nodes using the lists from the previous function 
            }

            while (huffmanList.Count != 1)                                          //Loop will stop until only one node remains (root of the tree)                                          
            {
                huffmanList.Sort(new CompareHuffmanNodes());                      //Sort the list so that the least common characters are at the front        
                left = huffmanList[0];
                huffmanList.Remove(left);
                right = huffmanList[0];
                huffmanList.Remove(right);


                top = new Node(left.Frequency + right.Frequency, '$');          //Create a new parent node for the two least frequent ones

                top.Left = left;
                top.Right = right;

                huffmanList.Add(top);
            }

            return huffmanList[0];                                             // Root of the tree

        }

        //The function recursively enters 0 and 1 to create a user-presentable code dictionary
        public void createDictionary(Node root, string str)             
        {
            if (root == null)
                return;

            if (root.Sign != '$')
                dictionaryList.Add(new HuffmanDictionary(root.Sign, str));
            createDictionary(root.Left, str + "0");
            createDictionary(root.Right, str + "1");
        }
        //Convert the dictionary to a displayable string
        public string generateDictionary()                                              
        {
            dictionaryList.Clear();
            createDictionary(GenerateTree(), "");
            dictionaryList.Sort(new CompareHuffmanDictionary());
            string str = string.Empty;
            for (int i = 0 ; i<dictionaryList.Count ; i++)
            {
                str += (dictionaryList[i].Sign + ": " + dictionaryList[i].Code + "\n");
            }
            return str;
        }

        //Replaces the message with a string of 0s and 1s presented as a string for visual representation to the user
        public string getHuffmanString(string text, List<HuffmanDictionary> dictionary) 
        {
            string str = "";

            for (int i = 0 ; i<text.Count(); i++)
            {
                str += dictionary.Where(f => f.Sign == text[i]).First().Code;
            }

            return str;
        }

        //Function creating a String of a code dictionary from a ready-made tree (received in transmission)
        public string generateDictionaryForTransmition(Node root)                             
        {
            dictionaryList.Clear();
            createDictionary(root, "");
            dictionaryList.Sort(new CompareHuffmanDictionary());
            string str = string.Empty;
            for (int i = 0; i < dictionaryList.Count; i++)
            {
                str += (dictionaryList[i].Sign + ": " + dictionaryList[i].Code + "\n");
            }
            return str;
        }
    }
    //The class represents a character with its representation as 0s and 1s from the tree
    internal class HuffmanDictionary    
    {
        private char sign;
        private string code;

        public HuffmanDictionary(char sign, string code)
        {
            this.sign = sign;
            this.code = code;
        }

        public char Sign { get => sign; set => sign = value; }
        public string Code { get => code; set => code = value; }
    }


    //representation of a node in a huffman binary tree
        internal class Node                  
    {
        private Node? left, right;
        private int frequency;
        private char sign;

        public Node()
        {
        }

        public Node(int frequency, char sign)
        {
            left = right = null;
            this.frequency = frequency;
            this.sign = sign;
        }

        public char Sign { get => sign; set => sign = value; }
        public int Frequency { get => frequency; set => frequency = value; }
        internal Node? Left { get => left; set => left = value; }
        internal Node? Right { get => right; set => right = value; }
    }

    //A class that serializes the tree to a string and reverses the process (on receipt)
    internal class Serializer   
    {
        string text = string.Empty;
        string frequencies = string.Empty;
        Node tree = new Node();
        List<int> freqList = new List<int>();
        List<char> signList = new List<char>();
        List<Node> nodeList = new List<Node>();
        int deserializeInt = 0;

        public string Text { get => text; set => text = value; }
        public string Frequencies { get => frequencies; set => frequencies = value; }
        internal Node Tree { get => tree; set => tree = value; }
        public List<int> FreqList { get => freqList; set => freqList = value; }
        public List<char> SignList { get => signList; set => signList = value; }
        internal List<Node> NodeList { get => nodeList; set => nodeList = value; }

        //Function that creates 2 strings from the binary tree (one for characters, one for frequencies)
        public void serialize(Node root)     
        {                                           
            if (root == null)                       //If the next node is null we append '&' when changing node we append '^'
            {
                text += "&" + "^";
                frequencies += "&" + "^";
                return;
            }
            text += root.Sign + "^";
            frequencies += Convert.ToString(root.Frequency) + "^";
            serialize(root.Left);                   
            serialize(root.Right);
            //Execute recursively until all tree nodes are served
        }

        //The function creates lists from strings received in transmission (Socked)
        //remove unnecessary ^ and & characters from them
        public void makeLists(string txt, string freq)  
        {                                               
            signList.Clear();
            freqList.Clear();

            for (int i = 0; i < txt.Length; i++)
            {
                if (txt[i] != '^')
                {
                    signList.Add(txt[i]);
                }
            }
            int k;
            for (int i = 0; i < freq.Length; i = k + 1)
            {
                k = findFrequency(freq, i);
                string myInt = freq.Substring(i, k - i);
                if (myInt != "&")
                {
                    freqList.Add(Convert.ToInt32(myInt));
                }
            }
        }

        //Function that finds how many chars contain a number (node.frequency)
        private int findFrequency(string freq, int i)   
        {
            for (int j = i; j < freq.Length; j++)
            {
                if (freq[j] == '^')
                {
                    return j;
                }
            }
            return 0;
        }


        // use recursive string deserialization and output binary tree
        public Node deserialize()        
        {
            deserializeInner1();
            tree = deserializeInner2();
            return tree;
        }

        //replacing chars and ints from lists made of strings received in the transmission into a list of nodes
        public void deserializeInner1()         
        {
            nodeList.Clear();
            int j = 0;
            for (int i = 0; i < signList.Count; i++, j++)
            {
                if (signList[i] == '&')         
                {
                    i++;
                    nodeList.Add(null);
                    if (signList[i] == '&')
                    {
                        i++;
                        nodeList.Add(null);
                    }
                }
                if (j == freqList.Count)    // interrupt condition (frequencies will be less than characters because in the list of characters it is encoded where are nulls
                {
                    return;
                }
                int frq = freqList[j];
                char sig = signList[i];
                Node node = new Node(freqList[j], signList[i]);
                nodeList.Add(node);
            }
        }

        // recursive deserlization (tree creation) from the list of nodes created earlier
        public Node deserializeInner2()  
        {
            if (nodeList[deserializeInt] == null)
            {
                return null;
            }
            Node root = nodeList[deserializeInt];
            deserializeInt++;
            root.Left = deserializeInner2();
            deserializeInt++;
            root.Right = deserializeInner2();
            return root;
        }
    }

    //compare the list to display it after the code length 0 and 1 of a given character
    internal class CompareHuffmanDictionary : IComparer<HuffmanDictionary>  
    {
        public int Compare(HuffmanDictionary x, HuffmanDictionary y) => x.Code.Length.CompareTo(y.Code.Length);
    }

    //sorts the nodes of the huffman tree to arrange them in descending order of frequency
    internal class CompareHuffmanNodes : IComparer<Node> 
    {
        public int Compare(Node x, Node y) => x.Frequency.CompareTo(y.Frequency);
    }
}
