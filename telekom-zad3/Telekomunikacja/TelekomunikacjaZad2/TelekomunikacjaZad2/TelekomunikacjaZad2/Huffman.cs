
namespace TelekomunikacjaZad2
{
    internal class Huffman  
    {
        List<Char> charList = new List<Char>();
        List<int> freqList = new List<int>();
        List<HuffmanDictionary> dictionaryList = new List<HuffmanDictionary>();
        internal List<HuffmanDictionary> DictionaryList { get => dictionaryList;}

        public void countFrequencies(string text)                      
        {
            freqList = new List<int>();                                
            charList = new List<Char>();                                

            for (int i = 0; i < text.Length ; i++)                      
            {
                if (!charList.Contains(text[i]))
                    charList.Add(text[i]);
            }
            
            for (int i = 0;i < charList.Count ; i++)                   
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

        public Node GenerateTree()                                       
        {
            Node left, right, top;

            List<Node> huffmanList = new  List<Node>();

            for (int i = 0 ; i<charList.Count ; i++)
            {
                huffmanList.Add(new Node(freqList[i], charList[i]));     
            }

            while (huffmanList.Count != 1)                                                                          
            {
                huffmanList.Sort(new CompareHuffmanNodes());                    
                left = huffmanList[0];
                huffmanList.Remove(left);
                right = huffmanList[0];
                huffmanList.Remove(right);


                top = new Node(left.Frequency + right.Frequency, '$');  

                top.Left = left;
                top.Right = right;

                huffmanList.Add(top);
            }

            return huffmanList[0];                                             
        }

        public void createDictionary(Node root, string str)             
        {
            if (root == null)
                return;

            if (root.Sign != '$')
                dictionaryList.Add(new HuffmanDictionary(root.Sign, str));
            createDictionary(root.Left, str + "0");
            createDictionary(root.Right, str + "1");
        }

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

        public string getHuffmanString(string text, List<HuffmanDictionary> dictionary) 
        {
            string str = "";

            for (int i = 0 ; i<text.Count(); i++)
            {
                str += dictionary.Where(f => f.Sign == text[i]).First().Code;
            }

            return str;
        }

        public string generateDictionary2(Node root)                             
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

        public void serialize(Node root)     
        {                                           
            if (root == null)
            {
                text += "&" + "^";
                frequencies += "&" + "^";
                return;
            }
            text += root.Sign + "^";
            frequencies += Convert.ToString(root.Frequency) + "^";
            serialize(root.Left);                   
            serialize(root.Right);
        }

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

        public Node deserialize()        
        {
            deserializeInner1();
            tree = deserializeInner2();
            return tree;
        }

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
                if (j == freqList.Count)        
                {
                    return;
                }
                int frq = freqList[j];
                char sig = signList[i];
                Node node = new Node(freqList[j], signList[i]);
                nodeList.Add(node);
            }
        }
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

    internal class CompareHuffmanDictionary : IComparer<HuffmanDictionary>  
    {
        public int Compare(HuffmanDictionary x, HuffmanDictionary y) => x.Code.Length.CompareTo(y.Code.Length);
    }
    internal class CompareHuffmanNodes : IComparer<Node> 
    {
        public int Compare(Node x, Node y) => x.Frequency.CompareTo(y.Frequency);
    }
}
