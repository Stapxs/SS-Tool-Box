using Newtonsoft.Json.Linq;
using SS_Tool_Box.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SS_Tool_Box
{
    /// <summary>
    /// QQChats.xaml 的交互逻辑
    /// </summary>
    public partial class QQChats : UserControl
    {
        public static readonly DependencyProperty QQProperty =
           DependencyProperty.Register(
           "QQ", typeof(string),
            typeof(QQChats),
             new FrameworkPropertyMetadata(new PropertyChangedCallback(OnQQChanged)
             )
           );
        public string QQ
        {
            get { return (string)GetValue(QQProperty); }
            set { SetValue(QQProperty, value); }
        }
        private static void OnQQChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string QQ = e.NewValue.ToString();
            String iduri = "https://api.toubiec.cn/qq?qq=" + QQ + "&size=300";
            string GetJson = HttpUitls.Get(iduri, "DEFALT");
            JObject obj = JObject.Parse(GetJson);
            Info.QQName = obj["name"].ToString();
        }

        public static readonly DependencyProperty QQSayProperty =
           DependencyProperty.Register(
           "Says", typeof(string),
            typeof(QQChats),
             new FrameworkPropertyMetadata(new PropertyChangedCallback(OnQQSaysChanged)
             )
           );
        public string Says
        {
            get { return (string)GetValue(QQSayProperty); }
            set { SetValue(QQSayProperty, value); }
        }
        private static void OnQQSaysChanged(DependencyObject d, DependencyPropertyChangedEventArgs r)
        {
            Info.QQSays = r.NewValue.ToString();
        }

        class QQInfo : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            private string qQName = "QQ Name";
            public string QQName
            {
                get { return qQName; }
                set
                {
                    qQName = value;
                    if (this.PropertyChanged != null)
                    {
                        this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("QQName"));
                    }
                }
            }
            private string qQSays = "QQ Says";
            public string QQSays
            {
                get { return qQSays; }
                set
                {
                    qQSays = value;
                    if (this.PropertyChanged != null)
                    {
                        this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("QQSays"));
                    }
                }
            }
        }
        static QQInfo Info;

        public QQChats()
        {
            InitializeComponent();

            Info = new QQInfo();
            
            Binding QQNamebinding = new Binding();
            QQNamebinding.Source = Info;
            QQNamebinding.Path = new PropertyPath("QQName");
            BindingOperations.SetBinding(this.QQName, TextBlock.TextProperty, QQNamebinding);

            Binding QQSaysbinding = new Binding();
            QQSaysbinding.Source = Info;
            QQSaysbinding.Path = new PropertyPath("QQSays");
            BindingOperations.SetBinding(this.QQSays, TextBlock.TextProperty, QQSaysbinding);

        }
    }
}
