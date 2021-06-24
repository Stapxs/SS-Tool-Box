using SS_Tool_Box.Classes.Structure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Markup;

namespace SS_Tool_Box.Helper
{
    public class LocalHelper
    {
        public static ResourceDictionary langUsed = null;
        private static string usedLang = @"Lang\en_US.xaml";

        public List<LangInfo> indexLocals = new List<LangInfo>
        {
            new LangInfo("中文（简体）", "zh_CN"),
            new LangInfo("English", "en_US"),

            new LangInfo("Select ...", "diy"),
        };

        public string ChangeLanguage(string name, bool isFist, bool isOut = false)
        {
            usedLang = @"Lang\" + name;
            try
            {
                if (!isOut)
                {
                    langUsed = (ResourceDictionary)Application.LoadComponent(new Uri(@"Lang\" + name, UriKind.Relative));
                }
                else
                {
                    FileStream fs = new FileStream(@"Lang\" + name, FileMode.Open);
                    langUsed = (ResourceDictionary)XamlReader.Load(fs);
                }
            }
            catch (Exception e)
            {
                return "ERR - " + e.Message;
            }
            if (langUsed != null)
            {
                Application app = Application.Current;
                // 如果已使用其他语言,先清空
                if (app.Resources.MergedDictionaries.Count > 0)
                {
                    List<ResourceDictionary> reses = new List<ResourceDictionary>();
                    foreach (ResourceDictionary res in app.Resources.MergedDictionaries)
                    {
                        if (res.Source != null && res.Source.ToString() == new Uri(usedLang, UriKind.Relative).ToString())
                        {
                            reses.Add(res);
                        }
                    }
                    foreach (ResourceDictionary res in reses)
                    {
                        app.Resources.MergedDictionaries.Remove(res);
                    }
                }
                app.Resources.MergedDictionaries.Add(langUsed);
            }
            return "OK";
        }
    }
}
