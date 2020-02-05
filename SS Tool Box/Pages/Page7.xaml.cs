using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Panuon.UI.Silver;
using System.Net;
using SS_Tool_Box.Classes;
using Newtonsoft.Json.Linq;
using SS_Tool_Box_By_WPF;
using System.IO;
using Newtonsoft.Json;
using MaterialDesignThemes.Wpf;
using System.Windows.Media;
using System.Collections.Generic;
using System.Windows.Input;
using Panuon.UI.Silver.Core;
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
        int FileVersion = 4;
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

            this.Height = 500;

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
                if (!err && int.Parse(Notes["Stat"]["NumOfDef"].ToString()) != 0)
                {
                    for (int i = 1; i <= int.Parse(Notes["Stat"]["NumOfDef"].ToString()); i++)
                    {
                        if(int.Parse(Notes["Cards"]["Def"][i.ToString()]["Stat"].ToString()) == 0)
                        {
                            Notes["Cards"]["Def"][i.ToString()].Parent.Remove();
                            Notes["Stat"]["NumOfDef"] = (int.Parse(Notes["Stat"]["NumOfDef"].ToString()) - 1).ToString();
                            delCard++;
                            continue;
                        }
                        AddColtrols(1, i, -1, Notes["Cards"]["Def"][i.ToString()]["Title"].ToString());
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
                if (String.IsNullOrWhiteSpace(What.Text) && int.Parse(AddWhat.SelectedValue.ToString()) == 0 && int.Parse(AddWhere.SelectedValue.ToString()) == 0)
                {
                    this.CD2.Visibility = Visibility.Collapsed;
                    return;
                }
                if(int.Parse(AddWhat.SelectedValue.ToString()) == 1)
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
                    AddColtrols(int.Parse(AddWhat.SelectedValue.ToString()), int.Parse(Notes["Stat"]["NumOfDef"].ToString()), -1, What.Text);
                }
                if (int.Parse(AddWhat.SelectedValue.ToString()) == 2)
                {
                    int num = int.Parse(Notes["Cards"]["Def"][AddWhere.SelectedValue.ToString()]["Stat"].ToString());
                    Notes["Cards"]["Def"][AddWhere.SelectedValue.ToString()]["Stat"] = (num + 1).ToString();
                    num += 1;
                    Notes["Cards"]["Def"][AddWhere.SelectedValue.ToString()]["Notes" + num + "Title"] = What.Text;
                    Notes["Cards"]["Def"][AddWhere.SelectedValue.ToString()]["Notes" + num + "Finished"] = false;
                    SaveData(Notes);
                    AddColtrols(int.Parse(AddWhat.SelectedValue.ToString()), int.Parse(Notes["Cards"]["Def"][AddWhere.SelectedValue.ToString()]["Stat"].ToString()), int.Parse(AddWhere.SelectedValue.ToString()), What.Text);
                }
                this.CD2.Visibility = Visibility.Collapsed;
                this.AdW.Visibility = Visibility.Collapsed;
            }
        }

        private bool AddColtrols(int addWhat, int another1, int another2, String anothers1)
        {
            /* addWhat - 创建什么, another,anothers - 其他数据
             * 1 - 普通卡片
             * 2 - 普通事件
             */
            if (addWhat == 1)
            {
                Card card = new Card();
                StackPanel stackPanel = new StackPanel();
                TextBlock textBlock = new TextBlock();
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

                textBlock.Text = another1 + ". " + anothers1;
                textBlock.HorizontalAlignment = HorizontalAlignment.Left;
                textBlock.TextWrapping = TextWrapping.Wrap;
                textBlock.Margin = new Thickness(34, 25, 34, 10);
                textBlock.Foreground = baseColora.Fg;
                textBlock.FontFamily = baseColora.Fonts;
                textBlock.FontSize = 15;

                card.Name = "CDD" + another1;
                card.Margin = new Thickness(5);
                card.Width = 540;
                card.Background = baseColora.Card;
                

                stackPanel.Children.Add(textBlock);
                card.Content = stackPanel;
                ScrollViewerHelper.SetScrollBarCornerRadius(listView, new CornerRadius(3));
                stackPanel.Children.Add(listView);
                MainIn.Children.Add(card);

                RegisterName("CDD" + another1, card);
            }
            else if(addWhat == 2)
            {
                Card card = GetChildObjects<Card>(MainIn, "CDD" + another2.ToString());
                if(card == null)
                {
                    this.RunCard.Visibility = Visibility.Visible;
                    error.ErrorTo("创建控件失败，未找到分类卡片。", Percent, Errorsay);
                    error.logWriter("Tool -7:未找到分类卡片。卡片名称：CDD" + another2, false);
                    return false;
                }
                List<StackPanel> lstControl = GetChildObjects<StackPanel>(card, typeof(StackPanel));
                if (lstControl == null)
                {
                    this.RunCard.Visibility = Visibility.Visible;
                    error.ErrorTo("创建控件失败，未找到分类卡片中的StackPanel控件。", Percent, Errorsay);
                    error.logWriter("Tool -7:未找到分类卡片中的StackPanel控件。卡片名称：CDD" + another2, false);
                    return false;
                }
                List<ListView> lstControl1 = GetChildObjects<ListView>(lstControl[0], typeof(ListView));
                if (lstControl1 == null)
                {
                    this.RunCard.Visibility = Visibility.Visible;
                    error.ErrorTo("创建控件失败，未找到分类卡片中的StackPanel控件中的列表控件。", Percent, Errorsay);
                    error.logWriter("Tool -7:未找到分类卡片中的StackPanel控件中的列表控件。卡片名称：CDD" + another2, false);
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
                contextMenu.Items.Add(menuItem2);
                menuItem3.Name = "MenuItem3";
                menuItem3.Header = "置顶";
                menuItem3.Icon = "";
                menuItem3.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(ME_Top);
                contextMenu.Items.Add(menuItem3);

                contextMenu.Width = 150;
                contextMenu.Background = baseColora.Card;
                contextMenu.Foreground = baseColora.Fg;
                ContextMenuHelper.SetShadowColor(contextMenu, baseColora.DBg.Color);
                ContextMenuHelper.SetItemHeight(contextMenu, 30);

                ListViewItem listViewItem = new ListViewItem();
                listViewItem.Name = "CDD" + another2 + "IT" + another1;
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

            if (int.Parse(AddWhat.SelectedValue.ToString()) != 1)
            {
                for (int i = 1; i <= int.Parse(Notes["Stat"]["NumOfDef"].ToString()); i++)
                {
                    string str = Notes["Cards"]["Def"][i.ToString()]["Title"].ToString();
                    if(Notes["Cards"]["Def"][i.ToString()]["Title"].ToString().Length > "四个汉字".Length)
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
            DeleteItem(0, false, false,true);
        }

        void ME_Top(object sender, MouseButtonEventArgs e)
        {

        }

        void ME_Del(object sender, MouseButtonEventArgs e)
        {
            DeleteItem(0, true, true, false);
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
                DeleteItem(1, true, true, false);
            }
        }

        private void RunButton_Click_1(object sender, RoutedEventArgs e)
        {
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

            if (!err && int.Parse(Notes["Stat"]["NumOfDef"].ToString()) != 0)
            {
                for (int i = 1; i <= int.Parse(Notes["Stat"]["NumOfDef"].ToString()); i++)
                {
                    if (int.Parse(Notes["Cards"]["Def"][i.ToString()]["Stat"].ToString()) != 0)
                    {
                        for (int j = 1; j <= int.Parse(Notes["Cards"]["Def"][i.ToString()]["Stat"].ToString()); j++)
                        {
                            AddColtrols(2, j, i, Notes["Cards"]["Def"][i.ToString()]["Notes" + j + "Title"].ToString());
                        }
                    }
                }
            }
            this.CD1.Visibility = Visibility.Visible;
            this.LOAD.Visibility = Visibility.Collapsed;
            this.MT1.Visibility = Visibility.Collapsed;
        }

        private bool DeleteItem(int nMode, bool IsDelInFile, bool IsDelInNotes, bool IsFinish)
        {
            if(nMode == 0)
            {
                list.Items.Remove(listi);

                Card Card = (Card)VisualTreeHelper.GetParent((Grid)VisualTreeHelper.GetParent((ContentPresenter)VisualTreeHelper.GetParent((StackPanel)VisualTreeHelper.GetParent(list))));

                string CardName = Card.Name;

                if(CardName[2] == 'D')          //普通卡片
                {
                    string CardType = "Def";
                    string CardID = CardName.Substring(3, CardName.Length - 3);
                    int DelWho = int.Parse(listi.Name.Substring(CardName.Length + 2, listi.Name.Length - (CardName.Length + 2)));

                    if(!IsDelInFile && IsFinish)
                    {
                        int FinishNum = int.Parse(Notes["Cards"][CardType][CardID]["Finished"].ToString()) - 1;
                        Notes["Cards"][CardType][CardID]["Finished"] = FinishNum.ToString();
                        Notes["Cards"][CardType][CardID]["Notes" + DelWho + "Finished"] = true;
                        Notes["Cards"][CardType][CardID]["Notes" + FinishNum + "Title"] = Notes["Cards"][CardType][CardID]["Notes" + DelWho + "Title"];
                        Notes["Cards"][CardType][CardID]["Notes" + FinishNum + "Finished"] = true;
                        Notes["Cards"][CardType][CardID]["Notes" + DelWho + "Title"].Parent.Remove();
                        Notes["Cards"][CardType][CardID]["Notes" + DelWho + "Finished"].Parent.Remove();
                        Notes["Cards"][CardType][CardID]["Stat"] = (int.Parse(Notes["Cards"][CardType][CardID]["Stat"].ToString()) - 1).ToString();
                        SaveData(Notes);
                        listi = new ListViewItem();
                        return true;
                    }

                    for(int i = DelWho; i < int.Parse(Notes["Cards"][CardType][CardID]["Stat"].ToString()); i++)
                    {
                        Notes["Cards"][CardType][CardID]["Notes" + i + "Title"] = Notes["Cards"][CardType][CardID]["Notes" + (i + 1) + "Title"];
                        Notes["Cards"][CardType][CardID]["Notes" + i + "Finished"] = Notes["Cards"][CardType][CardID]["Notes" + (i + 1) + "Finished"];
                    }
                    Notes["Cards"][CardType][CardID]["Notes" + DelWho + "Title"].Parent.Remove();
                    Notes["Cards"][CardType][CardID]["Notes" + DelWho + "Finished"].Parent.Remove();
                    Notes["Cards"][CardType][CardID]["Stat"] = (int.Parse(Notes["Cards"][CardType][CardID]["Stat"].ToString()) - 1).ToString();
                }
            }
            else if(nMode == 1)
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
            }
            if (IsDelInFile)
            {
                SaveData(Notes);
            }
            listi = new ListViewItem();
            return true;
        }
    }
}
