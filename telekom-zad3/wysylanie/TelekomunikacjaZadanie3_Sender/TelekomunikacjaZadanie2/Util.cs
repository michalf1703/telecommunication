using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace HuffmanTest
{
    class Util
    {
        public static byte[] BitArrayToByteArray(BitArray bits)
        {
            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }

        public static BitArray ByteToBitArray(byte bity, int rozmiar)
        {
            BitArray nowe = new BitArray(8);

            byte pow = 128;

            for (int i = 0; i < 8; ++i)
            {
                if (pow <= bity)
                {
                    nowe[i] = true;
                    bity -= pow;
                }

                if (pow != 1)
                    pow /= 2;
                else
                    pow = 128;
            }

            return nowe;
        }

        public byte[] addByteToArray(byte[] bArray, byte newByte)
        {
            byte[] newArray = new byte[bArray.Length + 1];
            bArray.CopyTo(newArray, 1);
            newArray[0] = newByte;
            return newArray;
        }
    }
}
