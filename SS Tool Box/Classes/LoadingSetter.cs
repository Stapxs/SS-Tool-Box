using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SS_Tool_Box.Classes
{
    class LoadingSetter
    {

        Error error = new Error();

        public JObject newSetup()
        {
            JObject SetUps = new JObject();     //主json
            JObject Exterior = new JObject();   //外观设置
            JObject Themes = new JObject { { "WindowTran", "0" }, { "MainTheme", "1" }, { "DarkMode", "False"} };
            JObject OwnColor = new JObject { { "R", "" }, { "G", "" }, { "B", "" } };
            Exterior.Add("Themes", Themes);
            Exterior.Add("OwnColor", OwnColor);
            SetUps.Add("Exterior", Exterior);
            writeJsom(SetUps);
            return SetUps;
        }

        public JObject ReadSetup()
        {
            JObject SetJson = new JObject { { "Error", "true"} };
            try
            {
                string jsonfile = "SSTB/Setup.json";
                using (System.IO.StreamReader file = System.IO.File.OpenText(jsonfile))
                {
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        SetJson = (JObject)JToken.ReadFrom(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                error.logWriter("读取设置文件错误：" + ex, false);
            }
            return SetJson;
        }

        public bool writeJsom(JObject obj)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("SSTB/Setup.json"))
                {
                    sw.WriteLine(obj.ToString());
                    sw.Close();
                }
            }
            catch(Exception ex)
            {
                error.logWriter("写入设置文件错误：" + ex, false);
            }
            return true;
        }

        public bool fistUsed()
        {
            if (!File.Exists("SSTB/Setup.json"))
            {
                return true;
            }
            return false;
        }
    }
}
