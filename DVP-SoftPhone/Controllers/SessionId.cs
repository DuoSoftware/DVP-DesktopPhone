using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoSoftware.DuoSoftPhone.Controllers
{
    public class SessionHandler
    {
        private static string RandomCharId(int passwordLength)
        {
            //const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            const string allowedChars = "0123456789";
            var chars = new char[passwordLength];
            var rd = new Random();

            for (int i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        public static string UniqueId(string userName)
        {
            DateTime dateObject = DateTime.Now;
            var id = new StringBuilder();
            id.Append(dateObject.Year);
            id.Append(dateObject.Month);
            id.Append(dateObject.Day);
            id.Append(dateObject.Hour);
            id.Append(dateObject.Minute);
            id.Append(dateObject.Second);
            id.Append(RandomCharId(4));
            //id.Append(userName);
            return id.ToString();
        }
    }
}
