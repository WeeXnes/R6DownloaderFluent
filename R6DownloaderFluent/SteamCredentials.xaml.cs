using System.Windows;
using Wpf.Ui.Controls.Window;

namespace R6DownloaderFluent;

public partial class SteamCredentials : FluentWindow
{
    public SteamCredentials()
    {
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        MainWindow.steam_credentials = new steamCreds(loginName.Text, loginPassword.Text);
        MainWindow.steam_credentials.writeJsonToFile();
        this.Close();
    }

    private void ButtonBase2_OnClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}