using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Panuon.UI.Silver;
using SS_Tool_Box.Classes;
using SS_Tool_Box_By_WPF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static SS_Tool_Box.SSMessageBox;

namespace SS_Tool_Box
{
    /// <summary>
    /// Page3.xaml 的交互逻辑
    /// </summary>
    public partial class Page7 : Page
    {
        Error error = new Error();
        BaseColor baseColora = Main.baseColor;

        JObject Notes = new JObject();
        bool err = false;
        IList<listMain> AddWhereList = new List<listMain>();
        int FileVersion = 7;
        ListView list = new ListView();
        ListViewItem listi = new ListViewItem();
        Card card = new Card();

        public class listMain
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }

        public Main ParentWindow { get; set; }

        public Page7()
        {
            InitializeComponent();

            this.Height = 477;

            String stTitle = "记事簿";
            this.Title.Foreground = baseColora.Fg;
            this.Title.FontFamily = baseColora.Fonts; ;
            this.Title.FontSize = 17;
            Title.Text = stTitle;

            String stSays = "就是个普普通通的记事簿而已。";
            this.Says.Foreground = baseColora.Fg;
            this.Says.FontFamily = baseColora.Fonts;
            this.Says.FontSize = 14;
            Says.Text = stSays;

            this.T1.Foreground = baseColora.Fg;
            this.T1.FontFamily = baseColora.Fonts;
            this.T1.FontSize = 13;
            this.T2.Foreground = baseColora.Fg;
            this.T2.FontFamily = baseColora.Fonts;
            this.T2.FontSize = 13;
            this.T4.Foreground = baseColora.Fg;
            this.T4.FontFamily = baseColora.Fonts;
            this.T4.FontSize = 13;
            this.T3.Foreground = baseColora.Fg;
            this.T3.FontFamily = baseColora.Fonts;
            this.T3.FontSize = 13;
            this.T5.Foreground = baseColora.Fg;
            this.T5.FontFamily = baseColora.Fonts;
            this.T5.FontSize = 13;
            this.T6.Foreground = baseColora.Fg;
            this.T6.FontFamily = baseColora.Fonts;
            this.T6.FontSize = 13;

            this.MT1.Foreground = baseColora.Fg;
            this.MT1.FontFamily = baseColora.Fonts;
            this.MT1.FontSize = 15;

            this.CD1.Background = baseColora.Card;
            this.CD2.Background = baseColora.Card;
            this.LOAD.Background = baseColora.Card;
            this.RunCard.Background = baseColora.Card;

            this.RunButton.Foreground = baseColora.Fg;
            this.RunButton.Background = baseColora.Tran;
            this.Start.Foreground = baseColora.Fg;
            this.Start.Background = baseColora.Tran;

            this.What.Background = baseColora.DBg;
            this.What.Foreground = baseColora.Fg;

            this.AddWhat.Background = baseColora.DBg;
            this.AddWhat.Foreground = baseColora.Fg;
            ComboBoxHelper.SetShadowColor(AddWhat, baseColora.Bg.Color);
            ComboBoxHelper.SetHoverBackground(AddWhat, baseColora.Bg);
            ComboBoxHelper.SetHoverForeground(AddWhat, baseColora.Fg);
            ComboBoxHelper.SetSelectedBackground(AddWhat, baseColora.DBg);
            ComboBoxHelper.SetSelectedForeground(AddWhat, baseColora.Fg);
            this.AddWhere.Background = baseColora.DBg;
            this.AddWhere.Foreground = baseColora.Fg;
            ComboBoxHelper.SetShadowColor(AddWhere, baseColora.Bg.Color);
            ComboBoxHelper.SetHoverBackground(AddWhere, baseColora.Bg);
            ComboBoxHelper.SetHoverForeground(AddWhere, baseColora.Fg);
            ComboBoxHelper.SetSelectedBackground(AddWhere, baseColora.DBg);
            ComboBoxHelper.SetSelectedForeground(AddWhere, baseColora.Fg);

            this.CanDel.Background = baseColora.Bg;
            CheckBoxHelper.SetCheckedBackground(CanDel, baseColora.Main);
            CheckBoxHelper.SetCheckedGlyphBrush(CanDel, baseColora.DBg);
            CheckBoxHelper.SetGlyphBrush(CanDel, baseColora.DBg);

            ButtonHelper.SetHoverBrush(RunButton, baseColora.Main);
            ButtonHelper.SetHoverBrush(Start, baseColora.Main);

            this.Load.Foreground = baseColora.Fg;

            this.Percent.Background = baseColora.DBg;

            this.RunCard.Visibility = Visibility.Collapsed;
            this.Errorsay.Visibility = Visibility.Collapsed;
            this.CD1.Visibility = Visibility.Collapsed;
            this.CD2.Visibility = Visibility.Collapsed;
            this.AdW.Visibility = Visibility.Collapsed;
            this.HID.Visibility = Visibility.Collapsed;

            //初始化列表
            IList<listMain> AddWhatList = new List<listMain>();
            AddWhatList.Add(new listMain() { ID = 0, Name = "  < 空 >" });
            AddWhatList.Add(new listMain() { ID = 1, Name = "  普通卡片" });
            AddWhatList.Add(new listMain() { ID = 2, Name = "  普通事件" });
            AddWhat.ItemsSource = AddWhatList;
            AddWhat.DisplayMemberPath = "Name";
            AddWhat.SelectedValuePath = "ID";
            AddWhat.SelectedValue = 0;

            //初始化目录&读取设置
            if (!Directory.Exists("SSTB/Files/Setups"))
            {
                Directory.CreateDirectory("SSTB/Files/Setups");
            }
            if (!File.Exists("SSTB/Files/Setups/NoteSaves.json"))
            {
                JObject Stat = new JObject { { "Version", FileVersion.ToString() }, { "NumOfDef", "0" } };
                JObject Cards = new JObject();
                Notes.Add("Stat", Stat);
                Cards.Add("Def", new JObject());
                Notes.Add("Cards", Cards);
                try
                {
                    using (StreamWriter sw = new StreamWriter("SSTB/Files/Setups/NoteSaves.json"))
                    {
                        sw.WriteLine(Notes.ToString());
                        sw.Close();
                    }
                }
                catch (Exception ex)
                {
                    this.RunCard.Visibility = Visibility.Visible;
                    error.ErrorTo("写入存储文件错误：", "" + ex, Percent, Errorsay);
                    error.logWriter("写入存储文件错误：" + ex, false);
                    err = true;
                }
            }
            else
            {
                Notes = new JObject { { "Error", "true" } };
                try
                {
                    string jsonfile = "SSTB/Files/Setups/NoteSaves.json";
                    using (System.IO.StreamReader file = System.IO.File.OpenText(jsonfile))
                    {
                        using (JsonTextReader reader = new JsonTextReader(file))
                        {
                            Notes = (JObject)JToken.ReadFrom(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.RunCard.Visibility = Visibility.Visible;
                    error.ErrorTo("读取存储文件错误：", "" + ex, Percent, Errorsay);
                    error.logWriter("读取存储文件错误：" + ex, false);
                    err = true;
                }
                try
                {
                    this.RunCard.Visibility = Visibility.Visible;
                    string Error = Notes["Error"].ToString();
                    error.ErrorTo("读取数据无效。", Percent, Errorsay);
                    error.logWriter("读取数据无效。", false);
                    err = true;
                }
                catch
                {
                    this.RunCard.Visibility = Visibility.Collapsed;
                }
                if(!err)
                {
                    if(int.Parse(Notes["Stat"]["NumOfDef"].ToString()) == 0)
                    {
                        this.MT1.Visibility = Visibility.Collapsed;
                    }
                }

                int delCard = 0;
                int[] things = new int[1];
                if (!err && int.Parse(Notes["Stat"]["NumOfDef"].ToString()) != 0)
                {
                    for (int i = 1; i <= int.Parse(Notes["Stat"]["NumOfDef"].ToString()); i++)
                    {
                        if(int.Parse(Notes["Cards"]["Def"][i.ToString()]["Stat"].ToString()) == 0 && int.Parse(Notes["Cards"]["Def"][i.ToString()]["Finished"].ToString()) == 0)
                        {
                            Notes["Cards"]["Def"][i.ToString()].Parent.Remove();
                            Notes["Stat"]["NumOfDef"] = (int.Parse(Notes["Stat"]["NumOfDef"].ToString()) - 1).ToString();
                            delCard++;
                            continue;
                        }
                        things[0] = i;
                        AddColtrols(1, things, Notes["Cards"]["Def"][i.ToString()]["Title"].ToString());
                    }
                }
                SaveData(Notes);
                if (delCard > 0)
                {
                    MT1.Text = "存在的卡片（已忽略空卡片 " + delCard + " 个）：";
                }
            }
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            if(err)
            {
                return;
            }
            if (!CD2.IsVisible)      //显示添加卡片
            {
                AddWhat.SelectedValue = 0;
                AddWhere.SelectedValue = 0;
                What.Text = "";
                this.CD2.Visibility = Visibility.Visible;
            }
            else                    //关闭添加卡片并且开始添加操作
            {
                if((String.IsNullOrWhiteSpace(What.Text) && int.Parse(AddWhat.SelectedValue.ToString()) == 1) || (int.Parse(AddWhat.SelectedValue.ToString()) == 0) || (int.Parse(AddWhat.SelectedValue.ToString()) == 2 && String.IsNullOrWhiteSpace(What.Text) && (int.Parse(AddWhere.SelectedValue.ToString()) == 0)))
                {
                    this.CD2.Visibility = Visibility.Collapsed;
                    return;
                }
                if(bool.Parse(CanDel.IsChecked.ToString()))
                {
                    SSMessageHelper.noNo = false;
                    ButtonHelper.SetIcon(SSMessageHelper.Icon, "");
                    SSMessageHelper.Title = "确认一下";
                    SSMessageHelper.Says = "真的要选择无法删除嘛，这会导致此记录无法选择删除。（你只能通过“完成”选项来消除此记录，我希望你可以好好利用这个功能来督促自己）";
                    SSMessageBox MB = new SSMessageBox();
                    ParentWindow.IsMaskVisible = true;
                    MB.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    MB.Owner = ParentWindow;
                    MB.ShowDialog();
                    ParentWindow.IsMaskVisible = false;
                    if(SSMessageHelper.buttonNO)
                    {
                        return;
                    }
                }
                try
                {
                    if (int.Parse(AddWhat.SelectedValue.ToString()) == 1)
                    {
                        JObject CardItem = new JObject();
                        int num = int.Parse(Notes["Stat"]["NumOfDef"].ToString());
                        Notes["Stat"]["NumOfDef"] = (num + 1).ToString();
                        num += 1;
                        CardItem["Title"] = What.Text;
                        CardItem["Stat"] = "0";
                        CardItem["Finished"] = "0";
                        Notes["Cards"]["Def"][num.ToString()] = CardItem;
                        SaveData(Notes);
                        int[] things = new int[1];
                        things[0] = int.Parse(Notes["Stat"]["NumOfDef"].ToString());
                        AddColtrols(int.Parse(AddWhat.SelectedValue.ToString()), things, What.Text);
                    }
                    if (int.Parse(AddWhat.SelectedValue.ToString()) == 2)
                    {
                        int num = int.Parse(Notes["Cards"]["Def"][AddWhere.SelectedValue.ToString()]["Stat"].ToString());
                        Notes["Cards"]["Def"][AddWhere.SelectedValue.ToString()]["Stat"] = (num + 1).ToString();
                        num += 1;
                        JObject Item = new JObject { { "Title", What.Text }, { "Finished", false }, { "CantDel", bool.Parse(CanDel.IsChecked.ToString()) } };
                        Notes["Cards"]["Def"][AddWhere.SelectedValue.ToString()][num.ToString()] = Item;
                        SaveData(Notes);
                        int[] things = new int[3];
                        things[0] = int.Parse(Notes["Cards"]["Def"][AddWhere.SelectedValue.ToString()]["Stat"].ToString());
                        things[1] = int.Parse(AddWhere.SelectedValue.ToString());
                        if (bool.Parse(CanDel.IsChecked.ToString()))
                        {
                            things[2] = 1;
                        }
                        else
                        {
                            things[2] = 0;
                        }
                        AddColtrols(int.Parse(AddWhat.SelectedValue.ToString()), things, What.Text);
                    }
                }
                catch (Exception ex)
                {
                    SSMessageHelper.noNo = false;
                    ButtonHelper.SetIcon(SSMessageHelper.Icon, "");
                    SSMessageHelper.Title = "写入JSON错误";
                    SSMessageHelper.bNOtext = "不用不用";
                    SSMessageHelper.bOKtext = "打开日志";
                    SSMessageHelper.Says = "我们发现了一个崩溃性的错误，是否反馈它，如果不反馈，这个错误将永远得不到修复！\n" + ex;
                    SSMessageBox MB = new SSMessageBox();
                    ParentWindow.IsMaskVisible = true;
                    MB.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    MB.Owner = ParentWindow;
                    MB.ShowDialog();
                    ParentWindow.IsMaskVisible = false;

                    error.logWriter("Tool7:读取JSON错误。" + ex, false);
                    if (SSMessageHelper.buttonOK)
                    {
                        string where = Directory.GetCurrentDirectory();
                        where = where + @"\SSTB\Log\log.log";
                        Process process;
                        process = System.Diagnostics.Process.Start(@where);
                    }
                    Application.Current.Shutdown();
                }
                this.CD2.Visibility = Visibility.Collapsed;
                this.AdW.Visibility = Visibility.Collapsed;
            }
        }

        private bool AddColtrols(int addWhat, int[] things, String anothers1)
        {
            /* addWhat - 创建什么, another,anothers - 其他数据
             * 1 - 普通卡片
             * 2 - 普通事件
             */
            if (addWhat == 1)
            {
                Card card = new Card();
                StackPanel stackPanel = new StackPanel();
                Grid grid = new Grid();
                TextBlock textBlock = new TextBlock();
                Button button = new Button();
                ListView listView = new ListView();
                ContextMenu contextMenu = new ContextMenu();
                MenuItem menuItem1 = new MenuItem();
                MenuItem menuItem2 = new MenuItem();
                MenuItem menuItem3 = new MenuItem();

                menuItem1.Name = "MenuItem1";
                menuItem1.Header = "删除卡片";
                menuItem1.Icon = "";
                menuItem1.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(CD_Del);
                contextMenu.Items.Add(menuItem1);

                contextMenu.Width = 150;
                contextMenu.Background = baseColora.Card;
                contextMenu.Foreground = baseColora.Fg;
                ContextMenuHelper.SetShadowColor(contextMenu, baseColora.DBg.Color);
                ContextMenuHelper.SetItemHeight(contextMenu, 30);

                textBlock.ContextMenu = contextMenu;
                card.ContextMenu = contextMenu;
                card.PreviewMouseRightButtonDown += WhatFuckOpenMeMe;

                textBlock.Text = things[0] + ". " + anothers1;
                textBlock.HorizontalAlignment = HorizontalAlignment.Left;
                textBlock.TextWrapping = TextWrapping.Wrap;
                textBlock.Margin = new Thickness(34, 25, 34, 10);
                textBlock.Foreground = baseColora.Fg;
                textBlock.FontFamily = baseColora.Fonts;
                textBlock.FontSize = 15;

                button.Foreground = baseColora.Fg;
                button.Margin = new Thickness(0, 10, 15, 0);
                button.Name = "BCDD" + things[0];
                button.Width = 30;
                button.Click += DelCard_Click;
                button.HorizontalAlignment = HorizontalAlignment.Right;
                ButtonHelper.SetIcon(button, "");
                IconHelper.SetFontSize(button, 16);
                ButtonHelper.SetButtonStyle(button, ButtonStyle.Link);

                card.Name = "CDD" + things[0];
                card.Margin = new Thickness(5, 15, 20, 5);
                card.Width = 540;
                card.Background = baseColora.Card;

                grid.Children.Add(textBlock);
                grid.Children.Add(button);
                stackPanel.Children.Add(grid);
                card.Content = stackPanel;
                ScrollViewerHelper.SetScrollBarCornerRadius(listView, new CornerRadius(3));
                stackPanel.Children.Add(listView);
                MainIn.Children.Add(card);

                RegisterName("CDD" + things[0], card);
            }
            else if(addWhat == 2)
            {
                Card card = GetChildObjects<Card>(MainIn, "CDD" + things[1].ToString());
                if(card == null)
                {
                    this.RunCard.Visibility = Visibility.Visible;
                    error.ErrorTo("创建控件失败，未找到分类卡片。", Percent, Errorsay);
                    error.logWriter("Tool -7:未找到分类卡片。卡片名称：CDD" + things[1], false);
                    return false;
                }
                List<StackPanel> lstControl = GetChildObjects<StackPanel>(card, typeof(StackPanel));
                if (lstControl == null)
                {
                    this.RunCard.Visibility = Visibility.Visible;
                    error.ErrorTo("创建控件失败，未找到分类卡片中的StackPanel控件。", Percent, Errorsay);
                    error.logWriter("Tool -7:未找到分类卡片中的StackPanel控件。卡片名称：CDD" + things[1], false);
                    return false;
                }
                List<ListView> lstControl1 = GetChildObjects<ListView>(lstControl[0], typeof(ListView));
                if (lstControl1 == null)
                {
                    this.RunCard.Visibility = Visibility.Visible;
                    error.ErrorTo("创建控件失败，未找到分类卡片中的StackPanel控件中的列表控件。", Percent, Errorsay);
                    error.logWriter("Tool -7:未找到分类卡片中的StackPanel控件中的列表控件。卡片名称：CDD" + things[1], false);
                    return false;
                }
                ContextMenu contextMenu = new ContextMenu();
                MenuItem menuItem1 = new MenuItem();
                MenuItem menuItem2 = new MenuItem();
                MenuItem menuItem3 = new MenuItem();

                menuItem1.Name = "MenuItem1";
                menuItem1.Header = "完成";
                menuItem1.Icon = "";
                menuItem1.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(ME_Fin);
                contextMenu.Items.Add(menuItem1);
                menuItem2.Name = "MenuItem2";
                menuItem2.Header = "删除";
                menuItem2.Icon = "";
                menuItem2.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(ME_Del);
                if(things[2] == 0)
                {
                    contextMenu.Items.Add(menuItem2);
                }

                contextMenu.Width = 150;
                contextMenu.Background = baseColora.Card;
                contextMenu.Foreground = baseColora.Fg;
                ContextMenuHelper.SetShadowColor(contextMenu, baseColora.DBg.Color);
                ContextMenuHelper.SetItemHeight(contextMenu, 30);

                ListViewItem listViewItem = new ListViewItem();
                listViewItem.Name = "CDD" + things[1] + "IT" + things[0];
                listViewItem.ContextMenu = contextMenu;
                listViewItem.PreviewMouseRightButtonDown += WhatFuckOpenMe;
                TextBlock textBlock = new TextBlock();
                textBlock.Text = anothers1;
                textBlock.Background = baseColora.Tran;
                textBlock.FontFamily = baseColora.Fonts;
                textBlock.Foreground = baseColora.Font;
                textBlock.FontSize = 15;
                textBlock.Margin = new Thickness(38, 5, 38, 5);

                listViewItem.Content = textBlock;
                lstControl1[0].Items.Add(listViewItem);
            }
            return true;
        }

        private void DelCard_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string cardname = button.Name.Substring(1);
            card = (Card)FindName(cardname);
            SSMessageHelper.noNo = false;
            ButtonHelper.SetIcon(SSMessageHelper.Icon, "");
            SSMessageHelper.Title = "确认一下";
            SSMessageHelper.Says = "确认要删除卡片嘛，它和它里面的事件们将消失很久！（真的很久很久……）";
            SSMessageBox MB = new SSMessageBox();
            ParentWindow.IsMaskVisible = true;
            MB.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            MB.Owner = ParentWindow;
            MB.ShowDialog();
            ParentWindow.IsMaskVisible = false;
            if (SSMessageHelper.buttonOK)
            {
                DeleteItem(1, new ControlItemInfo { IsDelInFile = true, IsFinish = false });
            }
        }

        private void WhatFuckOpenMe(object sender, MouseButtonEventArgs e)
        {
            VirtualizingStackPanel virtualizing = (VirtualizingStackPanel)VisualTreeHelper.GetParent((ListViewItem)sender);
            ItemsPresenter itemsPresenter = (ItemsPresenter)VisualTreeHelper.GetParent(virtualizing);
            ScrollContentPresenter scroll = (ScrollContentPresenter)VisualTreeHelper.GetParent(itemsPresenter);
            Grid grid = (Grid)VisualTreeHelper.GetParent(scroll);
            Border border = (Border)VisualTreeHelper.GetParent(grid);
            ScrollViewer scroll1 = (ScrollViewer)VisualTreeHelper.GetParent(border);
            list = (ListView)VisualTreeHelper.GetParent(scroll1);
            listi = sender as ListViewItem;
        }

        private void WhatFuckOpenMeMe(object sender, MouseButtonEventArgs e)
        {
            card = sender as Card;
        }

        private void CanDel_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void AddWhat_DropDownClosed(object sender, EventArgs e)
        {
            AddWhereList = new List<listMain>();
            AddWhereList.Add(new listMain() { ID = 0, Name = "  < 空 >" });

            if(int.Parse(AddWhat.SelectedValue.ToString()) != 1 && int.Parse(AddWhat.SelectedValue.ToString()) != 0)
            {
                this.HID.Visibility = Visibility.Visible;
            }
            else
            {
                this.HID.Visibility = Visibility.Collapsed;
            }
            try
            {
                if (int.Parse(AddWhat.SelectedValue.ToString()) != 1 && int.Parse(AddWhat.SelectedValue.ToString()) != 0)
                {
                    for (int i = 1; i <= int.Parse(Notes["Stat"]["NumOfDef"].ToString()); i++)
                    {
                        string str = Notes["Cards"]["Def"][i.ToString()]["Title"].ToString();
                        if (Notes["Cards"]["Def"][i.ToString()]["Title"].ToString().Length > "四个汉字".Length)
                        {
                            str = Notes["Cards"]["Def"][i.ToString()]["Title"].ToString().Substring(0, "四个汉字".Length) + "…";
                        }
                        AddWhereList.Add(new listMain() { ID = i, Name = "  " + str });
                    }
                    AddWhere.ItemsSource = AddWhereList;
                    AddWhere.DisplayMemberPath = "Name";
                    AddWhere.SelectedValuePath = "ID";
                    AddWhere.SelectedValue = 0;
                    this.AdW.Visibility = Visibility.Visible;
                }
                else
                {
                    this.AdW.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                SSMessageHelper.noNo = false;
                ButtonHelper.SetIcon(SSMessageHelper.Icon, "");
                SSMessageHelper.Title = "读取JSON错误";
                SSMessageHelper.bNOtext = "不用不用";
                SSMessageHelper.bOKtext = "打开日志";
                SSMessageHelper.Says = "我们发现了一个崩溃性的错误，是否反馈它，如果不反馈，这个错误将永远得不到修复！\n" + ex;
                SSMessageBox MB = new SSMessageBox();
                ParentWindow.IsMaskVisible = true;
                MB.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                MB.Owner = ParentWindow;
                MB.ShowDialog();
                ParentWindow.IsMaskVisible = false;

                error.logWriter("Tool7:读取JSON错误。" + ex, false);
                if (SSMessageHelper.buttonOK)
                {
                    string where = Directory.GetCurrentDirectory();
                    where = where + @"\SSTB\Log\log.log";
                    Process process;
                    process = System.Diagnostics.Process.Start(@where);
                    System.Diagnostics.Process.Start("https://ssteamcommunity.wordpress.com/feedback/");
                }
                Application.Current.Shutdown();
            }
        }

        private bool SaveData (JObject obj)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("SSTB/Files/Setups/NoteSaves.json"))
                {
                    sw.WriteLine(obj.ToString());
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                this.RunCard.Visibility = Visibility.Visible;
                error.ErrorTo("写入存储文件错误：", "" + ex, Percent, Errorsay);
                error.logWriter("写入存储文件错误：" + ex, false);
                return false;
            }
            return true;
        }

        public List<T> GetChildObjects<T>(DependencyObject obj, Type typename) where T : FrameworkElement
        {
            DependencyObject child = null;
            List<T> childList = new List<T>();

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T && (((T)child).GetType() == typename))
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child, typename));
            }
            return childList;
        }

        public T GetChildObjects<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject child = null;
            T grandChild = null;

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T && (((T)child).Name == name | string.IsNullOrEmpty(name)))
                {
                    return (T)child;
                }
                else
                {
                    grandChild = GetChildObjects<T>(child, name);
                    if (grandChild != null)
                        return grandChild;
                }
            }
            return null;
        }

        void ME_Fin(object sender, MouseButtonEventArgs e)
        {
            card = (Card)VisualTreeHelper.GetParent((Grid)VisualTreeHelper.GetParent((ContentPresenter)VisualTreeHelper.GetParent((StackPanel)VisualTreeHelper.GetParent(list))));
            DeleteItem(0, new ControlItemInfo { IsDelInFile = false, IsFinish = true, IsDelInUI = true });
        }

        void ME_Top(object sender, MouseButtonEventArgs e)
        {

        }

        void ME_Del(object sender, MouseButtonEventArgs e)
        {
            card = (Card)VisualTreeHelper.GetParent((Grid)VisualTreeHelper.GetParent((ContentPresenter)VisualTreeHelper.GetParent((StackPanel)VisualTreeHelper.GetParent(list))));
            DeleteItem(0, new ControlItemInfo { IsDelInFile = true, IsFinish = false, IsDelInUI = true });
        }

        void CD_Del(object sender, MouseButtonEventArgs e)
        {
            SSMessageHelper.noNo = false;
            ButtonHelper.SetIcon(SSMessageHelper.Icon, "");
            SSMessageHelper.Title = "确认一下";
            SSMessageHelper.Says = "确认要删除卡片嘛，它和它里面的事件们将消失很久！（真的很久很久……）";
            SSMessageBox MB = new SSMessageBox();
            ParentWindow.IsMaskVisible = true;
            MB.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            MB.Owner = ParentWindow;
            MB.ShowDialog();
            ParentWindow.IsMaskVisible = false;
            if(SSMessageHelper.buttonOK)
            {
                DeleteItem(1, new ControlItemInfo { IsDelInFile = true, IsFinish = false });
            }
        }


        /// <summary>
        /// RunButton_Click_1
        /// RunButton按下事件，刷新列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunButton_Click_1(object sender, RoutedEventArgs e)
        {
            //判断私隐密码
            if(Main.Settings["Features"]["Privacy"]["Password"].ToString() != "NULL")
            {
                PassWordEnterF7 EP = new PassWordEnterF7();
                ParentWindow.IsMaskVisible = true;
                EP.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                EP.Owner = ParentWindow;
                EP.ShowDialog();
                ParentWindow.IsMaskVisible = false;
            }
            else
            {
                LoadingSetter.PasswordPass = true;
            }
            if(!LoadingSetter.PasswordPass)
            {
                return;
            }
            LoadingSetter.PasswordPass = false;
            if (!err)
            {
                //判断存档版本
                if (Notes["Stat"]["Version"].ToString() != FileVersion.ToString())
                {
                    SSMessageHelper.noNo = true;
                    ButtonHelper.SetIcon(SSMessageHelper.Icon, "");
                    SSMessageHelper.Title = "存档版本不符";
                    SSMessageHelper.Says = "我们将取消读取档案，请至 File/Setups 文件夹下备份 NoteSaves.json 文件并删除以继续使用。\n（为什么要你手动删除而不自动？因为我想确保你确实去备份了，并且顺便删了。）";
                    SSMessageBox MB = new SSMessageBox();
                    ParentWindow.IsMaskVisible = true;
                    MB.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    MB.Owner = ParentWindow;
                    MB.ShowDialog();
                    ParentWindow.IsMaskVisible = false;

                    this.RunCard.Visibility = Visibility.Visible;
                    error.ErrorTo("存档版本不符。", Percent, Errorsay);
                    error.logWriter("Tool7:存档版本不符。", false);
                    err = true;
                    return;
                }
            }

            int[] things = new int[3];
            try
            {
                if (!err && int.Parse(Notes["Stat"]["NumOfDef"].ToString()) != 0)
                {
                    for (int i = 1; i <= int.Parse(Notes["Stat"]["NumOfDef"].ToString()); i++)
                    {
                        if (int.Parse(Notes["Cards"]["Def"][i.ToString()]["Stat"].ToString()) != 0)
                        {
                            for (int j = 1; j <= int.Parse(Notes["Cards"]["Def"][i.ToString()]["Stat"].ToString()); j++)
                            {
                                things[0] = j;
                                things[1] = i;
                                if (bool.Parse(Notes["Cards"]["Def"][i.ToString()][j.ToString()]["CantDel"].ToString()))
                                {
                                    things[2] = 1;
                                }
                                else
                                {
                                    things[2] = 0;
                                }
                                AddColtrols(2, things, Notes["Cards"]["Def"][i.ToString()][j.ToString()]["Title"].ToString());
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                SSMessageHelper.noNo = false;
                ButtonHelper.SetIcon(SSMessageHelper.Icon, "");
                SSMessageHelper.Title = "读取JSON错误";
                SSMessageHelper.bNOtext = "不用不用";
                SSMessageHelper.bOKtext = "打开日志";
                SSMessageHelper.Says = "我们发现了一个崩溃性的错误，是否反馈它，如果不反馈，这个错误将永远得不到修复！\n" + ex;
                SSMessageBox MB = new SSMessageBox();
                ParentWindow.IsMaskVisible = true;
                MB.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                MB.Owner = ParentWindow;
                MB.ShowDialog();
                ParentWindow.IsMaskVisible = false;

                error.logWriter("Tool7:读取JSON错误。" + ex, false);
                if(SSMessageHelper.buttonOK)
                {
                    string where = Directory.GetCurrentDirectory();
                    where = where + @"\SSTB\Log\log.log";
                    Process process;
                    process = System.Diagnostics.Process.Start(@where);
                }
                Application.Current.Shutdown();
            }
            this.CD1.Visibility = Visibility.Visible;
            this.LOAD.Visibility = Visibility.Collapsed;
            this.MT1.Visibility = Visibility.Collapsed;
        }



        /// <summary>
        /// DeleteItem
        /// 用于删除在界面上的对象并且刷新文件
        /// 参数：
        /// int nMode - 删除的对象种类
        /// ControlItemInfo info - 对象操作信息
        /// </summary>
        struct ControlItemInfo
        {
            public bool IsDelInUI;              //是否在UI上删除
            public bool IsDelInFile;            //是否在文件中删除
            public bool IsFinish;               //是否设置为完成
        }
        private bool DeleteItem(int nMode, ControlItemInfo info)
        {
            if (nMode == 0)                     //删除事件
            {
                Card Card = card;               //所属卡片
                string CardName = Card.Name;    //卡片名
                string CardType = "";           //卡片类型
                string CardID = "";             //卡片编号

                if (CardName[2] == 'D')         //普通卡片
                {
                    CardType = "Def";
                    CardID = CardName.Substring(3, CardName.Length - 3);
                }
                else
                {

                    return false;
                }

                //要删除的事件的编号
                string ItemID = listi.Name.Substring(CardName.Length + 2, listi.Name.Length - (CardName.Length + 2));
                //移除UI
                if (info.IsDelInUI)
                {
                    list.Items.Remove(listi);
                }
                //标记为完成（不在文件中删除）
                if (!info.IsDelInFile && info.IsFinish)
                {
                    //标记为完成（将编号向负数累加）
                    Notes["Cards"][CardType][CardID][ItemID]["Finished"] = true;
                    //将编号向负数累加
                    int nowFinishedNum = int.Parse(Notes["Cards"][CardType][CardID]["Finished"].ToString()) + 1;                    //要累加的编号数
                    Notes["Cards"][CardType][CardID]["Finished"] = nowFinishedNum.ToString();                                       //刷新编号数
                    Notes["Cards"][CardType][CardID]["Stat"] = (int.Parse(Notes["Cards"][CardType][CardID]["Stat"].ToString()) - 1).ToString();
                    JObject Item = new JObject();                                                                                   //将编号向负数累加
                    Item.Add("Title", Notes["Cards"][CardType][CardID][ItemID]["Title"]);
                    Item.Add("Finished", Notes["Cards"][CardType][CardID][ItemID]["Finished"]);
                    Item.Add("CantDel", Notes["Cards"][CardType][CardID][ItemID]["CantDel"]);
                    Notes["Cards"][CardType][CardID][(-nowFinishedNum).ToString()] = Item;
                    Notes["Cards"][CardType][CardID][ItemID].Parent.Remove();   //移除本项
                    listi = new ListViewItem();                                 //清空对象
                }
                //在文件中删除
                if (info.IsDelInFile)
                {
                    //循环删除
                    int i;
                    for (i = int.Parse(ItemID); i < int.Parse(Notes["Cards"][CardType][CardID]["Stat"].ToString()); i++)
                    {
                        JObject Item = new JObject();                                                                                   //将编号向负数累加
                        Item.Add("Title", Notes["Cards"][CardType][CardID][(i + 1).ToString()]["Title"]);
                        Item.Add("Finished", Notes["Cards"][CardType][CardID][(i + 1).ToString()]["Finished"]);
                        Item.Add("CantDel", Notes["Cards"][CardType][CardID][(i + 1).ToString()]["CantDel"]);
                        Notes["Cards"][CardType][CardID][i.ToString()] = Item;
                    }
                    //移除最后一项
                    if (int.Parse(Notes["Cards"][CardType][CardID]["Stat"].ToString()) > 1)
                    {
                        Notes["Cards"][CardType][CardID][i.ToString()].Parent.Remove();
                    }
                    else
                    {
                        Notes["Cards"][CardType][CardID][ItemID].Parent.Remove();
                    }
                    //刷新事件总数
                    Notes["Cards"][CardType][CardID]["Stat"] = (int.Parse(Notes["Cards"][CardType][CardID]["Stat"].ToString()) - 1).ToString();
                }
                SaveData(Notes);
            }
            else if (nMode == 1)         //删除卡片
            {
                Card Card = card;
                string CardName = Card.Name;
                string CardType = "Def";
                string CardID = CardName.Substring(3, CardName.Length - 3);
                if (CardName[2] == 'D')          //普通卡片
                {
                    Notes["Cards"][CardType][CardID].Parent.Remove();
                    Notes["Stat"]["NumOfDef"] = (int.Parse(Notes["Stat"]["NumOfDef"].ToString()) - 1).ToString();
                }
                MainIn.Children.Remove(Card);
                MainIn.UnregisterName(Card.Name);
            }
            listi = new ListViewItem();
            return true;
        }

        void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scroll = (ScrollViewer)sender;
            if (e.Delta > 0)
            {
                // 向上滚动
                scroll.LineUp();
                scroll.LineUp();
            }
            if (e.Delta < 0)
            {
                // 向下滚动
                scroll.LineDown();
                scroll.LineDown();
            }
        }

    }
}
