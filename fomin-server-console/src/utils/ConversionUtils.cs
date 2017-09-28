using System.Text;

namespace fominwebsocketserver.src.utils
{
    public static class ConversionUtils
    {
        public static string ToString(this byte[] bytes, int len = -1)
        {
            if(len == -1) len = bytes.Length;
            return Encoding.ASCII.GetString(bytes, 0, len);
        }

        public static byte[] ToByteArray(this string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        public static bool NotEmpty<T>(this T[] array) {
            return array.Length != 0;
        }
    }
}