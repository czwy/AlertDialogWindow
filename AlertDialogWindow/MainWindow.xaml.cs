using AlertDialogWindow.Helper;
using AlertDialogWindow.View;
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
using System.Windows.Threading;

namespace AlertDialogWindow
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        /// <summary>
        /// 显示系统消息弹出框
        /// </summary>
        /// <param name="dialogType">消息弹出框的类型</param>
        /// <param name="msg">消息内容</param>
        public AlertDialogResult ShowAlertDialog(AlertDialogMode dialogMode, AlertDialogType dialogType, string msg, string subcontent = "", string yesbuttonText = "确定", string nobuttonText = "取消", int countdown = -1, DependencyObject parent = null)
        {
            if (string.IsNullOrEmpty(msg)) return AlertDialogResult.Quit;
            AlertDialogResult ret = AlertDialogResult.Quit;
            Dispatcher.CurrentDispatcher.Invoke(delegate
            {
                ret = new AlterDialogWindow(dialogMode, dialogType, msg, subcontent, yesbuttonText, nobuttonText, countdown).Show(parent);
            });

            return ret;
        }

        private void askbtn_Click(object sender, RoutedEventArgs e)
        {
            ShowAlertDialog(AlertDialogMode.Normal, AlertDialogType.Ask, "是否退出？", yesbuttonText: "确定", nobuttonText: "取消", countdown: 3, parent: this);
        }

        private void infobtn_Click(object sender, RoutedEventArgs e)
        {
            ShowAlertDialog(AlertDialogMode.Normal, AlertDialogType.Info, "打开[a href=https://www.chinadaily.com.cn/]中国日报[/a]网站", yesbuttonText: "确定", nobuttonText: "取消", parent: this);
        }

        private void mini_Click(object sender, RoutedEventArgs e)
        {
            ShowAlertDialog(AlertDialogMode.Mini, AlertDialogType.Info, "是否退出？", yesbuttonText: "确定", nobuttonText: "取消", parent: this);
        }

        private void Full_Click(object sender, RoutedEventArgs e)
        {
            ShowAlertDialog(AlertDialogMode.Full, AlertDialogType.Info, "是否退出？", subcontent:"此处可以显示更为详细的信息", yesbuttonText: "确定", nobuttonText: "取消", parent: this);
        }

        private void askWithoutOwner_Click(object sender, RoutedEventArgs e)
        {
            ShowAlertDialog(AlertDialogMode.Normal, AlertDialogType.Ask, "是否退出？", yesbuttonText: "确定", nobuttonText: "取消");
        }

        private void error_Click(object sender, RoutedEventArgs e)
        {
            ShowAlertDialog(AlertDialogMode.Full, AlertDialogType.Fail, "操作失败", parent: this);
        }
    }
}
