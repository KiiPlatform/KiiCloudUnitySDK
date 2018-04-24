using System;
using System.Text;

namespace KiiCorp.Cloud.Storage
{
    public class TextUtils
    {
        public TextUtils()
        {
        }

        public static string randomAlphaNumeric(int length)
        {   
            if (length <= 0)
            {
                return "";
            }
            string str = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder sb = new StringBuilder(length);
            Random random = new Random();
            for (int index = 0; index < length; index++)
            {
                int strPosition = random.Next(str.Length);
                sb.Append(str[strPosition]);
            }
            return sb.ToString();
        }

        public static string generateUUID()
        {
            Guid guidValue = Guid.NewGuid();
            return guidValue.ToString();
        }
    }
}
