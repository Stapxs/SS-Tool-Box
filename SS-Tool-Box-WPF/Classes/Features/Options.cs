using System.Collections.Generic;
using System.IO;

namespace SS_Tool_Box
{
    /// <summary>
    /// 设置相关操作和功能
    /// </summary>
    public class Options
    {
        private static string optFile = "Data/Options.ini";         // 设置文件路径
        private static List<iniKey> opts = new List<iniKey>();      // 设置
        public static int optFileVer = 1;                          // 设置文件版本

        /// <summary>
        /// ini 数据结构
        /// </summary>
        public class iniKey
        {
            public string name;
            public string value;

            public iniKey(string name, string value)
            {
                this.name = name;
                this.value = value;
            }
        }

        /// <summary>
        /// 读取设置
        /// </summary>
        /// <returns>操作是否成功</returns>
        public static bool ReadOpt()
        {
            Log.AddLog("opt", "开始初始化设置……");
            try
            {
                // 初始化文件
                if (!File.Exists(@optFile))
                {
                    Directory.CreateDirectory("Data");
                    FileStream fs1 = new FileStream(optFile, FileMode.Create, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs1);
                    sw.WriteLine("optVer:" + optFileVer);
                    sw.Close();
                    fs1.Close();
                }
                // 逐行读取设置 
                string line;
                StreamReader file = new StreamReader(@optFile);
                while ((line = file.ReadLine()) != null)
                {
                    string name = line.Substring(0, line.IndexOf(":"));
                    string value = line.Substring(line.IndexOf(":") + 1);
                    opts.Add(new iniKey(name, value));
                }
                file.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取设置
        /// </summary>
        /// <param name="name">设置名</param>
        /// <returns>设置值或“ERR”</returns>
        public static List<string> GetOpt(string name)
        {
            bool get = false;
            List<string> values = new List<string>();
            foreach(iniKey info in opts)
            {
                if(info.name == name)
                {
                    get = true;
                    values.Add(info.value);
                }
            }
            if (!get)
            {
                values.Add("ERR");
            }
            return values;
        }

         /// <summary>
         /// 修改设置
         /// </summary>
         /// <param name="name">设置名</param>
         /// <param name="value">设置值</param>
         /// <returns>操作是否成功</returns>
         public static bool SetOpt(string name, string value)
        {
            string outLines = "";
            bool get = false;
            // 修改数值
            foreach (iniKey info in opts)
            {
                if (info.name == name)
                {
                    get = true;
                    // 修改
                    info.value = value;
                }
            }
            // 如果设置不存在
            if (!get)
            {
                opts.Add(new iniKey(name, value));
            }
            // 循环反序列化
            foreach (iniKey info in opts)
            {
                outLines += info.name + ":" + info.value + "\n";
            }
            // 输出
            if (outLines.Length > 0)
            {
                outLines = outLines.Substring(0, outLines.Length - 1);
                // 覆盖写入
                File.WriteAllText(@optFile, outLines);
            }
            else
            {
                return false;
            }
            return false;
        }

    }
}
