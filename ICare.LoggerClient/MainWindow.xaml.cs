using Log4MongoDB;
using Log4MongoDB.LogInner;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;

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
            this.SourceInitialized += MainWindow_SourceInitialized;
            Init();
        }

        private void MainWindow_SourceInitialized(object sender, EventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            HwndSource.FromHwnd(hwnd)?.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg != 513 && msg != 516)
                return hwnd;
            try
            {
                var w32Mouse = new Win32Point();
                GetCursorPos(ref w32Mouse);
                var realpt = PointFromScreen(w32Mouse.ToPoint());
                var cts = InputHitTest(realpt) as FrameworkElement;
                var btn = GetClickControl<Button>(cts); //点击的按钮

                var tapInfo = new PointTouchInfo
                {
                    ViewName = cts?.GetType().FullName,
                    ButtonContent = btn.Name,
                    PointX = realpt.X,
                    PointY = realpt.Y
                };
                MongoDbLogger.Insert(LogType.LogPointTouch, LogLevel.Info, tapInfo);

                Task.Run(() => { Console.Beep(1000, 200); });
            }
            catch (Exception ex)
            {
                Logger.Main.Error($"[系统打点]打点时出现异常，详情:{ex.Message}\r\n{ex.StackTrace}");
            }

            return hwnd;
        }

        private void Init()
        {
            foreach (var name in Enum.GetValues(typeof(LogType)))
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
            var logtype = (LogType)ComboBox.SelectionBoxItem;
            if (logtype == LogType.LogPointTouch)
            {
                DataGrid.ItemsSource = MongoDbLogger.QueryList<PointTouchInfo>(logtype);
                return;
            }

            DataGrid.ItemsSource = MongoDbLogger.QueryList<LogInfo>(logtype);
        }

        private void Button1_OnClick(object sender, RoutedEventArgs e)
        {
            var logtype = (LogType)ComboBox1.SelectionBoxItem;
            var loglevel = (LogLevel)ComboBox2.SelectionBoxItem;
            Log4MongoDB.LogInner.MongoDbLogger.Insert(logtype, loglevel, TextBox.Text);
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public int X;
            public int Y;

            public Point ToPoint()
            {
                return new Point(X, Y);
            }
        }

        private T GetClickControl<T>(DependencyObject element) where T : class
        {
            var target = element;

            if (target == null)
                return default(T);
            if (target is T)
                return target as T;

            //如果Button的Enabled为false，只能从子集找
            if (VisualTreeHelper.GetChildrenCount(target) > 0 && VisualTreeHelper.GetChild(target, 0) is T)
                return VisualTreeHelper.GetChild(target, 0) as T;

            //按照可视化树往上找
            target = VisualTreeHelper.GetParent(target);

            return GetClickControl<T>(target);
        }
    }
}