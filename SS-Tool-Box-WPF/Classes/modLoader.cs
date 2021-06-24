using Microsoft.Win32;
using SS_Tool_Box.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SS_Tool_Box.Function
{
    class modLoader
    {

        private static string modDir = "Data/Expand";
        private static List<string> modList = null;      // mod 列表
        
        /// <summary>
        /// 装载 SPK
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool Install(string file)
        {
            Log.AddLog("modLoader", "正在装载扩展包……");
            try
            {
                // 验证目录
                if (!Directory.Exists(modDir))
                {
                    Directory.CreateDirectory(modDir);
                }
                // 创建目录
                Directory.CreateDirectory(modDir + "/" + file.Substring(file.LastIndexOf("\\") + 1, file.Length - file.LastIndexOf("\\") - 1));
                // 解压 spk
                ZipFile.ExtractToDirectory(file, modDir + "/" + file.Substring(file.LastIndexOf("\\") + 1, file.Length - file.LastIndexOf("\\") - 1));
                // 结束
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// 刷新 MOD 并加载新增 MOD
        /// </summary>
        /// <returns></returns>
        public static bool Reload()
        {
            Log.AddLog("modLoader", "正在刷新扩展包列表……");
            // 遍历 MOD 文件夹
            List<FileSystemInfo> DirList = new List<FileSystemInfo>();
            List<FileSystemInfo> successList = new List<FileSystemInfo>();
            FileSystemInfo[] files = new DirectoryInfo(modDir).GetFileSystemInfos();
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo file = files[i] as FileInfo;
                if (file == null)
                    DirList.Add(files[i]);
            }
            // 验证信息
            foreach (FileSystemInfo mod in DirList)
            {
                String modName = mod.Name.Substring(0, mod.Name.Length - 4);
                // 加载 DLL
                Assembly nowAss = Assembly.LoadFile(mod.FullName + "\\" + modName + ".dll");
                // 获取 Info 类
                Type[] tys = nowAss.GetTypes();
                foreach (Type ty in tys)
                {
                    if (ty.Name == "Info")
                    {
                        // 获取 MOD 信息
                        ConstructorInfo magicConstructor = ty.GetConstructor(Type.EmptyTypes);
                        object magicClassObject = magicConstructor.Invoke(new object[] { });
                        MethodInfo mi = ty.GetMethod("Get");
                        object infoFun = mi.Invoke(magicClassObject, null);
                        List<string> info = (List<string>)infoFun;
                        if (double.Parse(info[0]) > MainWindow.verInfo.verNum)
                        {
                            Log.AddErr("modLoader", "验证扩展包失败，扩展包要求版本：" + info[2] + "，当前客户端版本：" + MainWindow.verInfo.verNum);
                            ToastHelper.Add("验证扩展包失败：过时的客户端");
                            return false;
                        }
                        // 添加到列表
                        successList.Add(mod);
                        FieldInfo name = ty.GetField("name");
                        Log.AddLog("modLoader", "添加扩展包 " + info[1] + " 成功！");
                    }
                }
            }
            // TODO: 因为这玩意还没写完，我们暂时不进行去重以及加载新增 MOD
            return false;
        }
    }
}
