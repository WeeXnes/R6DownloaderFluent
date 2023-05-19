using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace R6DownloaderFluent;

public partial class InstallDialog : UserControl
{
    public static UpdateVar<string> consoleLogCache = new UpdateVar<string>();
    public static bool isDownloading = false;
    public version installing_version { get; set; }
    public InstallDialog(version _version)
    {
        this.installing_version = _version;
        InitializeComponent();
    }

    private void InstallDialog_OnLoaded(object sender, RoutedEventArgs e)
    {
        List<DownloadObj> dwnlds = new List<DownloadObj>();
        foreach (var content in installing_version.contents)
        {
            dwnlds.Add(new DownloadObj(new List<DownloaderArgument>()
            {
                new DownloaderArgument(ArgType.AppId, content.appId),
                new DownloaderArgument(ArgType.DepotId, content.depotId),
                new DownloaderArgument(ArgType.ManifestId, content.manifestId),
                new DownloaderArgument(ArgType.Username, MainWindow.steam_credentials.username),
                new DownloaderArgument(ArgType.Password, MainWindow.steam_credentials.password),
                new DownloaderArgument(ArgType.Directory, "Downloads\\" + installing_version.title)
            }));
        }
        DepotDownloaderLib.RunDownload(dwnlds, true);
    }

    private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
    {
        DepotDownloaderLib.onConsoleOutput += (f, s) =>
        {
            Dispatcher.Invoke(() =>
            {
                InstallProgress.Value = f;
            });
        };
        consoleLogCache.ValueChanged += () =>
        {
            Dispatcher.Invoke(() =>
            {
                installLog.AppendText(consoleLogCache.Value + "\n");
                installLog.ScrollToEnd();
            });
        };
    }
}