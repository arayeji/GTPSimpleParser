using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTEParser
{
    static class Conver
    {

        public static uint ToInt(this byte[] bt)
        {
            if (bt.Length == 4)
            {
                return (uint)(((bt[0] & 0xff) << 24) | ((bt[1] & 0xff) << 16) | ((bt[2] & 0xff) << 8) | (bt[3] & 0xff));
            }
            else if (bt.Length == 3)
            {
                return (uint)(((bt[0] & 0xff) << 16) | ((bt[1] & 0xff) << 8) | (bt[2] & 0xff));
            }
            else if (bt.Length == 2)
            {
                return (uint)(((bt[0] & 0xff) << 8) | (bt[1] & 0xff));
            }
            else
            {
                return (uint)(bt[0] & 0xff);
            }
        }
        public static string ToString(this byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static string IEArrayToString(this byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
            {
                string s = string.Format("{0:x2}", b);
                s = s[1].ToString() + s[0];
                hex.Append(s);
            }
            return hex.ToString().Replace("f", "");
        }
        public static string ToString(this byte[] ba, uint count)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            for (int x = 0; x < count; x++)
                hex.AppendFormat("{0:x2}", ba[x]);
            return hex.ToString();
        }
        public static byte[] ToByteArray(this String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}
