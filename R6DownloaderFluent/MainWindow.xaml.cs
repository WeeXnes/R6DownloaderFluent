using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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
using Wpf.Ui.Controls.ThumbRateControl;
using Wpf.Ui.Controls.TitleBarControl;
using Wpf.Ui.Controls.Window;

namespace R6DownloaderFluent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FluentWindow
    {
        public static steamCreds steam_credentials = null;
        #region utils

        private const int ATTACH_PARENT_PROCESS = -1;

        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int dwProcessId);

        /// <summary>
        /// /// Attach Programm to its own console (For JetBrains Rider)
        /// </summary>
        public static void AttachToConsole()
        {
            AttachConsole(ATTACH_PARENT_PROCESS);
        }

        #endregion
        public MainWindow()
        {
            InitializeComponent();
            AppStartup();
            
        }

        public void AppStartup()
        {
            
            AttachToConsole();
            if (File.Exists("creds.json"))
            {
                steam_credentials = steamCreds.FromJsonFile("creds.json");
            }
            else
            {
                do
                {
                    new SteamCredentials().ShowDialog();
                } while (steam_credentials == null);
            }
            //steam_credentials = steamCreds.FromJsonFile("creds.json");
            AppDomain currentDomain = default(AppDomain);
            currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += GlobalUnhandledExceptionHandler;
            using (var consoleWriter = new ConsoleWriter())
            {
                //consoleWriter.WriteEvent += ConsoleWriterOnWriteEvent;
                consoleWriter.WriteLineEvent += ConsoleWriterOnWriteLineEvent;

                Console.SetOut(consoleWriter);
            }
        }
        private static void GlobalUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            using (StreamWriter writer = new StreamWriter("error_log.txt"))
            {
                writer.WriteLine(ex.ToString());
            }
        }

        private void ConsoleWriterOnWriteLineEvent(object? sender, ConsoleWriterEventArgs e)
        {
            InstallDialog.consoleLogCache.Value = e.Value;
        }


        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            
            DepotDownloaderLib.onConsoleInput += () =>
            {
                string inputRead = (string)Dispatcher.Invoke(() =>
                {
                    //Creating a InputBox window to get user input for the 2FA Code
                    return FluentUX.CreateTwoFactorPrompt();
                });
                return inputRead;
            };
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            
            var buttonObj = ((Button) sender).CommandParameter;
            version versionFromButtonObj = (version) buttonObj;
            border_content.Child = new VersionView(versionFromButtonObj);
        }

        private void ListViewVersions_OnLoaded(object sender, RoutedEventArgs e)
        {
            VersionFile versionFile = VersionFile.FromJsonFile("versions.json");
            ListViewVersions.ItemsSource = versionFile.versions;
        }

        private void TitleBar2_OnLoaded(object sender, RoutedEventArgs e)
        {
            Version? version = Assembly.GetExecutingAssembly().GetName().Version;
            if(version == null)
                return;

            TitleBar2.Title = "R6DownloaderFluent - v" + version.ToString();
        }


        private void TitleBar2_OnMinimizeClicked(TitleBar sender, RoutedEventArgs args)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}