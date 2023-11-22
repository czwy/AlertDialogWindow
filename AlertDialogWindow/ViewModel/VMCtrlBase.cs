using AlertDialogWindow.Helper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Threading;

namespace AlertDialogWindow.ViewModel
{
    public class VMCtrlBase : ViewModelBase
    {
        private System.Windows.FrameworkElement _element;

        private string _title = string.Empty;
        /// <summary>
        /// 获取或设置元素的标题
        /// </summary>
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                Set(ref _title, value);
            }
        }

        private bool _isEnabled = true;
        /// <summary>
        /// 获取或设置内容区当前是否可用
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                Set(ref _isEnabled, value);
            }
        }

        private bool _isBusy;
        /// <summary>
        /// 是否正在忙
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                Set(ref _isBusy, value);
            }
        }

        private string _busyerText;
        /// <summary>
        /// 等待器上显示的文本
        /// </summary>
        public string BusyerText
        {
            get
            {
                return _busyerText;
            }
            set
            {
                Set(ref _busyerText, value);
            }
        }

        private RelayCommand<System.Windows.FrameworkElement> _LoadedCommand;

        public RelayCommand<System.Windows.FrameworkElement> LoadedCommand
        {
            get
            {
                return _LoadedCommand = _LoadedCommand ?? new RelayCommand<System.Windows.FrameworkElement>((fe) =>
                {
                    if (IsInDesignMode) return;

                    bool isBusy = IsBusy;
                    if (isBusy)
                    {
                        IsBusy = false;
                        IsBusy = isBusy;
                    }

                    if (_element == null)
                    {
                        IntPtr winHandler = IntPtr.Zero;
                        if (fe is System.Windows.Window && (fe as System.Windows.Window).Topmost == false)
                        {
                            System.Windows.Interop.WindowInteropHelper wndHelper = new System.Windows.Interop.WindowInteropHelper(fe as System.Windows.Window);
                            winHandler = wndHelper.Handle;
                            WindowNativeMethods.SetWindowNoBorder(winHandler);
                        }

                        fe.Dispatcher.BeginInvoke((Action)delegate
                        {
                            _element = fe;
                            if (fe != null)
                            {
                                OnFrameworkLoadedHandle(fe, winHandler);
                            }

                            OnLoaded();
                        }, DispatcherPriority.Background);
                    }

                });
            }
        }

        protected VMCtrlBase()
        {

        }

        /// <summary>
        /// 从元素树中加载成功后执行的回调函数
        /// </summary>
        protected virtual void OnLoaded()
        {

        }

        /// <summary>
        ///  当从加载的元素的元素树中移除元素时执行的回调函数
        /// </summary>
        protected virtual void OnUnLoaded()
        {

        }

        protected virtual void OnFrameworkLoadedHandle(System.Windows.FrameworkElement fe, IntPtr winHandler)
        {
            fe.Unloaded -= Fe_Unloaded;
            fe.Unloaded += Fe_Unloaded;
        }

        private void Fe_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            (sender as System.Windows.FrameworkElement).Unloaded -= Fe_Unloaded;
            OnUnLoaded();
            _element = null;
        }

        public override void Cleanup()
        {
            base.Cleanup();
        }

        /// <summary>
        /// 返回对 System.Windows.Window 对象的引用，该对象承载依赖项对象所在的内容树。
        /// </summary>
        /// <returns></returns>
        public System.Windows.Window GetWindow()
        {
            if (_element == null) return null;
            System.Windows.Window win = null;
            Dispatcher.CurrentDispatcher.Invoke(delegate
            {
                win = System.Windows.Window.GetWindow(_element);
            });
            return win;
        }

        /// <summary>
        /// 获取窗体关联ViewModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetWindowViewModel<T>()
            where T : class
        {
            T ret = default(T);
            if (_element == null) return ret;
            Dispatcher.CurrentDispatcher.Invoke(delegate
            {
                System.Windows.Window win = System.Windows.Window.GetWindow(_element);
                if (win == null || win.DataContext == null) return;

                ret = (T)win.DataContext;
            });
            return ret;
        }

    }
}
