using System;
using System.Windows;
using System.Windows.Controls;

namespace R6DownloaderFluent;

public partial class VersionView : UserControl
{
    public version cur_version { get; set; }
    public VersionView(version _version)
    {
        InitializeComponent();
        this.cur_version = _version;
        label_name.Text = _version.title;
        label_desc.Text = _version.metadata.description;
    }

    private void Btn_start_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }


    private void Btn_install_OnClick(object sender, RoutedEventArgs e)
    {
        ((MainWindow)App.Current.MainWindow).border_content.Child = new InstallDialog(this.cur_version);
    }

    private void Btn_uninstall_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}