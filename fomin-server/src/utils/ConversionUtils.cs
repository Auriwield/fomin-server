using System.Text;

namespace fomin_server.utils
{
    public static class ConversionUtils
    {
        public static string StringValue(this byte[] bytes, int len = -1)
        {
            if(len == -1) len = bytes.Length;
            return Encoding.UTF8.GetString(bytes, 0, len);
        }

        public static byte[] ToByteArray(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static bool NotEmpty<T>(this T[] array) {
            return array.Length != 0;
        }
    }
}