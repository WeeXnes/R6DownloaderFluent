using System;
using System.Diagnostics;
using System.IO;
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
        string file1 = "Downloads\\" + this.cur_version.title + "\\RainbowSixGame.exe";
        bool file1_exists = false;
        string file2 = "Downloads\\" + this.cur_version.title + "\\RainbowSix.exe";
        bool file2_exists = false;
        ////////////////////////////
        if(File.Exists(file1))
            file1_exists = true;
        ////////////////////////////
        if(File.Exists(file2))
            file2_exists = true;
        
        //Hopefully working, not tested
        
        if (file1_exists && !file2_exists)
        {
            Process.Start(file1);
        }
        else if (!file1_exists && file2_exists)
        {
            Process.Start(file2);
        }
        else if (file1_exists && file2_exists)
        {
            
            FileInfo fi1 = new FileInfo(file1);
            FileInfo fi2 = new FileInfo(file2);
            if (fi1.Length > fi2.Length)
            {
                Process.Start(file1);
            }
            else if (fi2.Length > fi1.Length)
            {
                Process.Start(file2);
            }
            
        }
    }


    private void Btn_install_OnClick(object sender, RoutedEventArgs e)
    {
        ((MainWindow)App.Current.MainWindow).border_content.Child = new InstallDialog(this.cur_version);
    }

    private void Btn_uninstall_OnClick(object sender, RoutedEventArgs e)
    {
        string versionPath = "Downloads\\" + this.cur_version.title;
        if (Directory.Exists(versionPath))
        {
            Directory.Delete(versionPath, true);
            new Notify(this.cur_version.title + " has been uninstalled successfully").ShowDialog();
        }
        else
        {
            new Notify(this.cur_version.title + " is not installed").ShowDialog();
        }
    }
}