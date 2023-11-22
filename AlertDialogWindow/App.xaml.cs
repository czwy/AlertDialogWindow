using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AlertDialogWindow
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Application.Current.Resources.MergedDictionaries.Clear();
            ResourceDictionary source = (ResourceDictionary)Application.LoadComponent(new Uri("Themes/Generic.xaml", UriKind.Relative));
            Application.Current.Resources.MergedDictionaries.Add(source);
        }
    }
}
