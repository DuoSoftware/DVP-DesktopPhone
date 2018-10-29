using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    class FileHandler
    {
        public static JObject WriteUserData(string name, string password, string domain,string delay)
        {
            //string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string filePath = Path.Combine(appDataFolder, "veery");
            filePath = Path.Combine(appDataFolder, "veeryphone.txt");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            dynamic jsonObject = new JObject();
            jsonObject.Date = DateTime.Now;
            jsonObject.name = name;
            jsonObject.password = password;
            jsonObject.domain = domain;
            jsonObject.Delay = delay;

            File.WriteAllText(filePath, StringCipher.Encrypt(jsonObject.ToString(), "veery"));

            File.Encrypt(filePath);

            return jsonObject;

        }

        public static JObject ReadUserData()
        {
            //string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string filePath = Path.Combine(appDataFolder, "veery");
            filePath = Path.Combine(appDataFolder, "veeryphone.txt");
            if (File.Exists(filePath))
            {
                var data = File.ReadAllText(filePath);
                var vData = StringCipher.Decrypt(data, "veery");
                return JObject.Parse(vData);
            }
            return null;
            
           
        }
    }
}
