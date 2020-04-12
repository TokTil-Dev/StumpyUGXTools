using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StumpyUGXTools
{
    static class Utils
    {
        public static UInt32 CalcAdler32(byte[] array, int start, int length)
        {
            const int mod = 65521;
            uint a = 1, b = 0;
            for (int i = 0; i < length; i++)
            {
                byte c = array[start + i];
                a = (a + c) % mod;
                b = (b + a) % mod;
            }
            return (b << 16) | a;
        }
        public static int FindPatternInByteArray(byte[] array, byte[] pattern, int offset)
        {
            var len = pattern.Length;
            var limit = array.Length - len;
            for (var i = 0; i <= limit; i++)
            {
                var k = 0;
                for (; k < len; k++)
                {
                    if (pattern[k] != array[i + k]) break;
                }
                if (k == len) return i;
            }
            return -1;
        }
        public static void ReplaceRange(this List<byte> bl, byte[] barr, int index, int numToReplace)
        {
            if (numToReplace == 0) numToReplace = barr.Length;
            for (int i = 0; i < numToReplace; i++)
            {
                if (i + index >= bl.Count) { bl.Add(barr[i]); }
                else { bl[i + index] = barr[i]; }
            }
        }
        public static string GetStringFromNullTerminatedByteArray(byte[] arr, int startIndex)
        {
            int cnt = 0;
            for (int i = startIndex; i < arr.Length; i++)
            {
                if (arr[i] != '\0')
                {
                    cnt++;
                }
                else break;
            }
            byte[] b = new byte[cnt];
            Array.Copy(arr, startIndex, b, 0, cnt);
            return (Encoding.Default.GetString(b));
        }
    }
}