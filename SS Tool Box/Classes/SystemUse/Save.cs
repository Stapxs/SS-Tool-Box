using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SS_Tool_Box
{
    class Save
    {
        public static JObject SetJson;

        private static double FileVersion = 2.1;

        #region 读取/刷新设置

        /// <summary>
        /// 读取/刷新设置
        /// </summary>
        /// <returns>是否成功</returns>
        public static bool SetReader()
        {
            if(!File.Exists(FilePath.set))
            {
                Log.Error("设置文件不存在", "警告");
                if(SetCrater())
                {
                    Log.Info("创建设置文件成功！");
                }
                else
                {
                    Log.Error("创建设置文件失败", "崩溃");
                    Application.Current.Shutdown();
                }
            }
            JObject SetJsons;
            try
            {
                string jsonfile = FilePath.set;
                using (System.IO.StreamReader file = System.IO.File.OpenText(jsonfile))
                {
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        SetJsons = (JObject)JToken.ReadFrom(reader);
                        SetJson = SetJsons;
                        try
                        {
                            if(Convert.ToDouble(SetJson["Version"]) != FileVersion)
                            {
                                SetJson = new JObject { { "Error", "设置文件版本不符合" } };
                                Log.Error("设置文件版本不符合", "严重");
                                return false;
                            }
                        }
                        catch
                        {
                            SetJson = new JObject { { "Error", "未发现设置文件版本" } };
                            Log.Error("未发现设置文件版本", "严重");
                            return false;
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                SetJson = new JObject { { "Error", "读取设置文件错误" } };
            #if DEBUG
                Log.Error("读取设置文件错误" + ex, "严重");
            #endif
            #if !DEBUG
                Log.Error("读取设置文件错误" + ex.Message, "严重");
            #endif
            }
            return false;
        }

        #endregion
        #region 保存设置

        /// <summary>
        /// 保存设置
        /// </summary>
        /// <returns>是否成功</returns>
        public static bool SetWritter()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(FilePath.set))
                {
                    sw.WriteLine(SetJson.ToString());
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
            #if DEBUG
                Log.Error("保存设置文件错误" + ex, "严重");
            #endif
            #if !DEBUG
                Log.Error("保存设置文件错误" + ex.Message, "严重");
            #endif
            }
            return true;
        }

        #endregion
        #region 创建新的设置文件

        /// <summary>
        /// 创建新的设置文件
        /// </summary>
        /// <returns>是否成功</returns>
        private static bool SetCrater()
        {
            JObject SetUps = new JObject();     //主json
            SetUps["Version"] = FileVersion.ToString();
            SetJson = SetUps;
            return SetWritter();
        }

        #endregion

    }
}
