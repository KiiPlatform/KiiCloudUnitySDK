using System;
using System.Text;

namespace KiiCorp.Cloud.Storage
{
    public class StringUtils
    {
        private static Random random = new Random((int)DateTime.Now.Ticks);

        public static string RandomAlphabetic(int size){
            StringBuilder builder = new StringBuilder();
            
            for (int i = 0; i < size; i++)
            {
                char ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            
            return builder.ToString().ToLower();
        }
    }
}