using System;
using System.Collections;
using System.IO;
using System.Threading;

namespace SS_Tool_Box
{
    class Log
    {

        private static Queue logList = new Queue();
        private static string logFile = "Data/Log/Log0.log";         // 日志文件路径
        private static string logDc = "Data/Log";                   // 日志文件文件夹路径
        private static bool logExit = false;                         // 退出标记

        public static void StartLogOut()
        {
            AddLog("log", "正在初始化日志输出功能……");
            Directory.CreateDirectory("Data");
            Directory.CreateDirectory(@logDc);
            // 遍历log文件夹处理多次log
            DirectoryInfo TheFolder = new DirectoryInfo(@logDc);
            for (int i = 3; i >= 0; i--)
            {
                foreach (FileInfo NextFile in TheFolder.GetFiles())
                {
                    if (NextFile.Name == "Log" + i + ".log")
                    {
                        if(i == 3)
                        {
                            File.Delete(logDc + "/Log3.log");
                        }
                        else
                        {
                            File.Move(logDc + "/" + NextFile.Name, logDc + "/Log" + (i + 1) + ".log");
                        }
                    }
                }
            }
            // 清空 Log.log
            if (!File.Exists(@logFile))
            {
                FileStream fs1 = new FileStream(logFile, FileMode.Create, FileAccess.Write);
                fs1.Close();
            }
            else
            {
                File.Delete(logFile);
                FileStream fs1 = new FileStream(logFile, FileMode.Create, FileAccess.Write);
                fs1.Close();
            }
            // 运行日志写入线程
            Thread thread = new Thread(runOut);
            thread.Start();
            // 计入线程堆栈（日志系统单独退出，不计入线程堆栈）
            // MainWindow.threads.Push(thread);
        }

        private static void runOut()
        {
            AddLog("log", "初始化日志输出功能完成！");
            // 循环输出日志
            while (!logExit)
            {
                while(logList.Count != 0)
                {;
                    // 输出日志
                    using (StreamWriter sw = new StreamWriter(@logFile, true))
                    {
                        sw.WriteLine(logList.Dequeue());
                        logList.TrimToSize();
                    }
                }
                Thread.Sleep(500);
            }
            using (StreamWriter sw = new StreamWriter(@logFile, true))
            {
                string time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                sw.WriteLine("[" + time + "][log] 日志输出系统已正常退出。（积压未输出日志 " + logList.Count + " 条）");
            }
        }

        public static void AddLog(string type, string log)
        {
            // 获取时间
            string time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            // 组合字符串
            String logOut = "[" + time + "][" + type + "] " + log;
            // 加入队列
            logList.Enqueue(logOut);
        }

        public static void exit()
        {
            logExit = true;
        }
    }
}
