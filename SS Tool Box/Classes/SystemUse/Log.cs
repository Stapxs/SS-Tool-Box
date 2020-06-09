using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SS_Tool_Box
{
    class Log
    {
        #region 全局变量

        /// <summary>
        /// 日志进程锁
        /// </summary>
        public static Mutex mutex = new Mutex();

        #endregion

        #region 写入日志
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        /// <returns>写入是否成功</returns>
        private static bool Write(string msg)
        {
            // 申请互斥锁
            mutex.WaitOne();
            // 处理日志写入
            String nowTime = DateTime.Now.TimeOfDay.ToString();
            msg = "[" + nowTime.Substring(0, nowTime.IndexOf(".") + 3) + "] " + msg;
            if (!Directory.Exists(FilePath.log))    // 目录不存在则创建
            {
                Directory.CreateDirectory(FilePath.log);
            }
            try     // 写入
            {
                File.AppendAllText(FilePath.log + "log.log", "\r\n" + msg);
            }
            catch (Exception ex)
            {
                MessageBox.Show("写入Log文件错误：" + ex);
                mutex.ReleaseMutex();
                return false;
            }
            // 释放互斥锁
            mutex.ReleaseMutex();
            return true;
        }

        #endregion

        #region 写入错误日志
        /// <summary>
        /// 写入错误日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        /// <param name="err">错误类型</param>
        /// <returns>写入是否成功</returns>
        public static bool Error(string msg, string err)
        {
            string errormsg = "【" + err + "】" + msg;
            return Write(errormsg);
        }
        #endregion

        #region 写入普通日志
        /// <summary>
        /// 写入普通日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        /// <returns>写入是否成功</returns>
        public static bool Info(string msg)
        {
            string infomsg = "【信息】" + msg;
            return Write(infomsg);
        }
        #endregion
    }
}
