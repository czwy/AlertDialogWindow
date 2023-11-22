using AlertDialogWindow.Helper;
using AlertDialogWindow.Toolkit;
using AlertDialogWindow.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AlertDialogWindow.View
{
    /// <summary>
    /// AlterDialogWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AlterDialogWindow : BaseChildWindow
    {
        public AlterDialogWindow()
        {
            InitializeComponent();

        }

        public AlterDialogWindow(AlertDialogMode dialogMode, AlertDialogType dialogType, string content, string subcontent, string yesbuttonText = "确定", string nobuttonText = "取消", int countdown = -1,DependencyObject parent=null)
        :this(){
            switch (dialogMode)
            {
                case AlertDialogMode.Mini:
                    Height = 64;
                    countdown = 3;
                    break;
                case AlertDialogMode.Normal:
                    Height = 180;
                    break;
                case AlertDialogMode.Full:
                    Height = 240;
                    break;
            }
            DataContext = new VMAlterDialog() { DialogMode = dialogMode, DialogType = dialogType, Content = content, SubContent = subcontent, YesButtonText = yesbuttonText, NoButtonText = nobuttonText, Countdown = countdown };
        }

        public AlertDialogResult Show(DependencyObject parent)
        {
            ShowDialog(parent);
            return (DataContext as VMAlterDialog).IsConfirm;
        }
    }

    public class AlterDialogWindowButtonDataTemplateSelector : DataTemplateSelector
    {
        private static readonly DataTemplate EmptyDataTemplate = new DataTemplate();

        /// <summary>
        /// 有取消按钮模板
        /// </summary>
        public DataTemplate Template0 { get; set; }
        /// <summary>
        /// 没取消按钮模板
        /// </summary>
        public DataTemplate Template1 { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (GalaSoft.MvvmLight.ViewModelBase.IsInDesignModeStatic) return EmptyDataTemplate;

            var vm = item as VMAlterDialog;
            if (vm == null) return EmptyDataTemplate;

            if (vm.DialogType == AlertDialogType.Ask)
            {
                return Template0;
            }
            else
            {
                return Template1;
            }
        }
    }

    public class AlterDialogWindowContentTemplateSelector : DataTemplateSelector
    {
        private static readonly DataTemplate EmptyDataTemplate = new DataTemplate();

        /// <summary>
        /// 迷你模式模板
        /// </summary>
        public DataTemplate Template0 { get; set; }
        /// <summary>
        /// 普通模式模板
        /// </summary>
        public DataTemplate Template1 { get; set; }
        /// <summary>
        /// 完全模式模板
        /// </summary>
        public DataTemplate Template2 { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (GalaSoft.MvvmLight.ViewModelBase.IsInDesignModeStatic) return EmptyDataTemplate;

            var vm = item as VMAlterDialog;
            if (vm == null) return EmptyDataTemplate;

            if (vm.DialogMode == AlertDialogMode.Mini)
            {
                return Template0;
            }
            else if (vm.DialogMode == AlertDialogMode.Normal)
            {
                return Template1;
            }
            else
                return Template2;
        }
    }

    public class AlterDialogWindow_ButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dialogType = (AlertDialogType)Enum.Parse(typeof(AlertDialogType), value.ToString());
            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            string[] paras = parameter.ToString().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (paras[0] == "0")//确定按钮
            {
                if (dialogType == AlertDialogType.Warning)
                {
                    if (paras[1] == "0")
                        return new Uri($"/{assemblyName};component/Themes/Images/AlterDialog/BtnYes_Warning.png",UriKind.RelativeOrAbsolute);
                    else if (paras[1] == "1" || paras[1] == "2")
                        return new Uri($"/{assemblyName};component/Themes/Images/AlterDialog/BtnYes_Warning_Hover.png", UriKind.RelativeOrAbsolute);
                    else
                        return "#ffffff";
                }
                else
                {
                    if (paras[1] == "0")
                        return new Uri($"/{assemblyName};component/Themes/Images/AlterDialog/BtnYes.png", UriKind.RelativeOrAbsolute);
                    else if (paras[1] == "1" || paras[1] == "2")
                        return new Uri($"/{assemblyName};component/Themes/Images/AlterDialog/BtnYes_Hover.png", UriKind.RelativeOrAbsolute);
                    return "#ffffff";
                }
            }
            else//取消按钮
            {
                if (paras[1] == "0")
                    return new Uri($"/{assemblyName};component/Themes/Images/AlterDialog/BtnNo.png", UriKind.RelativeOrAbsolute);
                else
                    return new Uri($"/{assemblyName};component/Themes/Images/AlterDialog/BtnNo_Hover.png", UriKind.RelativeOrAbsolute);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class AlterDialogWindow_IconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            return new Uri($"/{assemblyName};component/Themes/Images/AlterDialog/{value.ToString()}.png", UriKind.RelativeOrAbsolute);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

}
