using System;

namespace fomin_server.utils
{
    public static class TypeExtensions
    {
        public static bool IsEmpty(this string str)
        {
            return str == null || str.Equals("");
        }

        public static bool IsEmpty(this byte[] bytes)
        {
            return bytes == null || bytes.Length == 0;
        }

        public static byte[] Append(this byte[] bytes, byte[] appendBytes)
        {
            byte[] dest = new byte[bytes.Length + appendBytes.Length];
            Array.Copy(bytes, 0, dest, 0, bytes.Length);
            Array.Copy(appendBytes, 0, dest, bytes.Length, appendBytes.Length);
            return dest;
        }
    }
}
