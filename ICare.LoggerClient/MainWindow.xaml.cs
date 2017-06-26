using System;
using System.Collections.Generic;
using System.Linq;
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
using Log4MongoDB;

namespace ICare.LoggerClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            foreach (var name in Enum.GetValues(typeof( LogType)))
            {
                ComboBox.Items.Add(new ComboBoxItem
                {
                    Content = name
                });
                ComboBox1.Items.Add(new ComboBoxItem
                {
                    Content = name
                });
            }
            foreach (var name in Enum.GetValues(typeof(LogLevel)))
            {
                ComboBox2.Items.Add(new ComboBoxItem
                {
                    Content = name
                });
              
            }
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            var logtype = (LogType) ComboBox.SelectionBoxItem;

            var list= Log4MongoDB.LogInner.MongoDbLogger.QueryList(logtype);
            DataGrid.ItemsSource = list;
           
        }

        private void Button1_OnClick(object sender, RoutedEventArgs e)
        {
            var logtype = (LogType)ComboBox1.SelectionBoxItem;
            var loglevel = (LogLevel) ComboBox2.SelectionBoxItem;
            Log4MongoDB.LogInner.MongoDbLogger.Insert(logtype, loglevel,TextBox.Text);
        }
    }
}
