using System.Windows;
using Wpf.Ui.Controls.Window;

namespace R6DownloaderFluent;

public partial class Notify : FluentWindow
{
    public Notify(string _notifyText)
    {
        InitializeComponent();
        NotifyText.Content = _notifyText;
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}