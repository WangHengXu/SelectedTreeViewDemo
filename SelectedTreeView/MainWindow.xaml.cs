using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace SelectedTreeView
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitList();
        }
        public ObservableCollection<FirstMenu> FirstMenus = new ObservableCollection<FirstMenu>();
        public void InitList()
        {
            FirstMenu firstMenu;
            for (int i = 0; i < 5; i++)
            {
                firstMenu = new FirstMenu() { Header = i.ToString(), SecondMenus = new ObservableCollection<SecondMenu>() };
                for (int j = 0; j < 5; j++)
                {
                    firstMenu.SecondMenus.Add(new SecondMenu() { Header = j.ToString() ,Parent= firstMenu });
                }
                FirstMenus.Add(firstMenu);
            }
            tree.ItemsSource = FirstMenus;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Parallel.ForEach(FirstMenus, item =>
            {
                item.IsChecked = true ;
            });
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Parallel.ForEach(FirstMenus, item =>
             {
                 item.IsChecked = false;
             });
        }
    }
    public class FirstMenu: INotifyPropertyChanged
    {
        private bool _isRunChecked=true;

        public bool IsRunChecked
        {
            get { return _isRunChecked; }
            set { _isRunChecked = value; }
        }
        private bool? _ischecked=false;
        public bool? IsChecked
        {
            get { return _ischecked; }
            set
            {
                if (_ischecked == value)
                    return;
                _ischecked = value;
                NotifyChanged();
                if(_ischecked==false && IsRunChecked == true)
                {
                    foreach (var item in SecondMenus)
                    {
                        item.IsChecked = false;
                    }
                }
                if(_ischecked == true && IsRunChecked == true)
                {
                    foreach (var item in SecondMenus)
                    {
                        item.IsChecked = true;
                    }
                }
            }
        }
        public string Header { get; set; }
        public ObservableCollection<SecondMenu> SecondMenus { get; set; }
        public int Count { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
    public class SecondMenu:INotifyPropertyChanged
    {
        private bool? _ischecked=false;

        public bool? IsChecked
        {
            get { return _ischecked; }
            set
            {
                _ischecked = value;
                NotifyChanged();
                if(Parent!=null && _ischecked==false)
                {
                    Parent.IsRunChecked = false;
                    Parent.IsChecked = false;
                    Parent.IsRunChecked = true;
                    Parent.Count--;
                }
                if(Parent != null && _ischecked == true)
                {
                    Parent.Count++;
                    if(Parent.SecondMenus.Count==Parent.Count)
                    {
                        Parent.IsRunChecked = false;
                        Parent.IsChecked = true;
                        Parent.IsRunChecked = true;
                    }
                }
            }
        }
        public FirstMenu Parent { get; set; }

        public string Header { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyChanged([CallerMemberName] string name="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
   
}
