using AlertDialogWindow.Helper;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace AlertDialogWindow.Toolkit
{
    /// <summary>
    /// 基础子窗体
    /// </summary>
    [TemplatePart(Name = "ContentGrid", Type = typeof(Grid))]
    public class BaseChildWindow : Window, INotifyPropertyChanged, INotifyPropertyChanging
    {
        private DispatcherFrame _dispatcherFrame;
        private Grid _grid;
        private bool? _dialogResult = null;
        /// <summary>
        /// 是否正在关闭
        /// </summary>
        private bool _isClosing = false;
        /// <summary>
        /// 是否已经关闭完成
        /// </summary>
        private bool _isClosed = false;

        /// <summary>
        /// 是否已被释放
        /// </summary>
        public bool IsDisposed { get; private set; } = false;

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        static BaseChildWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseChildWindow), new FrameworkPropertyMetadata(typeof(BaseChildWindow)));
        }

        public BaseChildWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void RaisePropertyChanging(string propertyName)
        {
            var handler = PropertyChanging;
            if (handler != null)
            {
                handler(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        protected virtual void OnShowDialog()
        {

        }

        public bool? ShowDialog(DependencyObject parent)
        {
            if (this.Parent == null && parent != null)
            {
                Grid layer = new Grid() { Name = "maskLayer", Background = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0)) };
                _grid = Window.GetWindow(parent).FindFirstVisualChild<Grid>();
                if (_grid.FindAllVisualChilds<Grid>().FirstOrDefault(r => r.Name == "maskLayer") == null)
                    _grid.Children.Add(layer);
                if (_grid.RowDefinitions.Count > 0)
                    Grid.SetRowSpan(layer, _grid.RowDefinitions.Count);
                if (_grid.ColumnDefinitions.Count > 0)
                    Grid.SetColumnSpan(layer, _grid.ColumnDefinitions.Count);
                this.Owner = Window.GetWindow(parent);
                this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            return ShowDialog();
        }


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            IsDisposed = true;

            if (_grid != null)
            {
                Grid layer = _grid.FindAllVisualChilds<Grid>().FirstOrDefault(r => r.Name == "maskLayer");
                _grid.Children.Remove(layer);
                //_grid = null;
            }

            // 当关闭终止消息循环
            if (_dispatcherFrame != null)
            {
                _dispatcherFrame.Continue = false;
                _dispatcherFrame = null;
                ComponentDispatcher.PopModal();
            }
        }

        protected void Set<T>(ref T oldValue, T newValue, [CallerMemberName]string propertyName = null)
        {
            if (!Object.Equals(oldValue, newValue))
            {
                oldValue = newValue;
                RaisePropertyChanged(propertyName);
            }
        }

    }
}

