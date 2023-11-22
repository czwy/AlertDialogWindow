using AlertDialogWindow.Helper;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;

namespace AlertDialogWindow.ViewModel
{
    /// <summary>
    /// 窗体的基础ViewModel
    /// </summary>
    public class VMWinBase : VMCtrlBase//, IVMWin
    {
        private static readonly List<WeakReference> _winList = new List<WeakReference>();

        private Window _win;
        private WeakReference _weakWin;
        private WindowState _initWinState;

        protected IntPtr _winHandle = IntPtr.Zero;
        private HwndSource _hwndSource;
        private bool __isClosed = false;

        /// <summary>
        /// 当前窗体是否最大化显示
        /// </summary>
        protected bool _isMaximized = false;

        /// <summary>
        /// 是否为用户关闭窗口的
        /// </summary>
        public bool IsUserClosed
        {
            get; private set;
        }

        public Window Owner
        {
            get { return _win?.Owner; }
            set
            {
                if (_win != null && _win.Owner == null)
                {
                    _win.Owner = value;
                }
            }
        }

        //
        // 摘要:
        //     获取或设置一个值，该值指示窗口是否出现在 Z 顺序的最顶层。
        //
        // 返回结果:
        //     如果窗口是最顶层元素，则为 true；否则为 false。
        public bool Topmost
        {
            get { return _win == null ? false : _win.Topmost; }
            set
            {
                if (_win != null && _win.Topmost != value)
                {
                    _win.Topmost = value;
                }
            }
        }

        private bool _isCanFullScreen = false;
        /// <summary>
        /// 是否可以全屏
        /// </summary>
        public bool IsCanFullScreen
        {
            get { return _isCanFullScreen; }
            set { Set(ref _isCanFullScreen, value, nameof(IsCanFullScreen)); }
        }

        private bool _isFullScreen = false;
        /// <summary>
        /// 窗体当前是否全屏显示
        /// </summary>
        public bool IsFullScreen
        {
            get { return _isFullScreen; }
            set
            {
                if (_isCanFullScreen == false) return;

                if (_isFullScreen != value)
                {
                    Set(ref _isFullScreen, value, nameof(IsFullScreen));
                    if (_win == null) return;
                    if (value)
                    {
                        WindowNativeMethods.HideTask(true);
                        _win.WindowState = WindowState.Maximized;
                        _win.Topmost = true;
                        Dispatcher.CurrentDispatcher.Invoke(delegate
                        {
                            if (_win == null) return;
                            _win.Topmost = false;
                        });

                    }
                    else
                    {
                        WindowNativeMethods.HideTask(false);
                        _win.WindowState = WindowState.Normal;
                    }
                }
            }
        }


        /// <summary>
        /// 是否已被释放
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// 获取或设置是否自动适应屏幕的大小
        /// </summary>
        public bool IsFitToScreenSize { get; set; }

        private bool _isShowInTaskbar = true;
        /// <summary>
        ///  获取或设置一个指示窗口是否具有任务栏按钮的值
        /// </summary>
        public bool IsShowInTaskbar
        {
            get
            {
                if (_win != null)
                {
                    return _isShowInTaskbar = _win.ShowInTaskbar;
                }
                return _isShowInTaskbar;
            }
            set
            {
                if (_isShowInTaskbar != value)
                {
                    _isShowInTaskbar = value;
                    if (_win != null)
                    {
                        _win.ShowInTaskbar = _isShowInTaskbar;
                    }
                }
            }
        }

        private string _taskbarItemDescription;
        /// <summary>
        /// 获取或设置任务栏项工具提示的文本
        /// </summary>
        public string TaskbarItemDescription
        {
            get { return _taskbarItemDescription; }
            set { Set(ref _taskbarItemDescription, value); }
        }

        private System.Windows.Shell.TaskbarItemProgressState _taskbarItemProgressState;
        /// <summary>
        /// 获取或设置一个值，该值指示在任务栏按钮中显示进度指示器的方式
        /// </summary>
        public System.Windows.Shell.TaskbarItemProgressState TaskbarItemProgressState
        {
            get { return _taskbarItemProgressState; }
            set { Set(ref _taskbarItemProgressState, value); }
        }

        private double _taskbarItemProgressValue;
        /// <summary>
        /// 获取或设置一个值，该值指示在任务栏按钮中显示进度指示器的方式
        /// </summary>
        public double TaskbarItemProgressValue
        {
            get { return _taskbarItemProgressValue; }
            set { Set(ref _taskbarItemProgressValue, Math.Max(0, Math.Min(1.0d, value))); }
        }

        public VMWinBase()
        {
            IsEnabled = true;
        }

        //
        // 摘要:
        //     获取或设置对话框结果值，此值是从 System.Windows.Window.ShowDialog 方法返回的值。
        //
        // 返回结果:
        //     一个 System.Boolean 类型的 System.Nullable`1 值。默认值为 false。
        //
        // 异常:
        //   T:System.InvalidOperationException:
        //     System.Windows.Window.DialogResult 是在通过调用 System.Windows.Window.ShowDialog 打开窗口或通过调用
        //     System.Windows.Window.Show 打开窗口之前设置的。
        private bool? _dialogResult = false;
        public bool? DialogResult
        {
            get { return _win == null ? _dialogResult : _win.DialogResult; }
            set
            {
                if (_win != null)
                {
                    IsUserClosed = true;
                    _win.DialogResult = _dialogResult = value;
                }
            }
        }

        private void Win_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            OnFormClosing(e);
            __isClosed = !e.Cancel;
            IsUserClosed = false;
        }

        private void Win_Closed(object sender, EventArgs e)
        {
            IsUserClosed = false;
            __isClosed = true;
            if (_weakWin != null)
            {
                _winList.Remove(_weakWin);
                _weakWin.Target = null;
                _weakWin = null;
            }

            Microsoft.Win32.SystemEvents.DisplaySettingsChanged -= SystemEvents_DisplaySettingsChanged;
            if (_winHandle != IntPtr.Zero)
            {
                _hwndSource?.RemoveHook(WndProc);
                _hwndSource = null;
                _winHandle = IntPtr.Zero;
            }

            (sender as Window).Closed -= Win_Closed;
            (sender as Window).Closing -= Win_Closing;
            IsDisposed = true;
            OnFormClosed();
            Messenger.Default.Unregister(this);
            Messenger.Default.Unregister(sender);
            Cleanup();
            if (_win != null)
            {
                _win.Owner = null;
                _win = null;
            }
        }

        private RelayCommand<System.Windows.Window> _minWinCommand;

        /// <summary>
        /// 窗体的最小化命令
        /// </summary>
        public RelayCommand<System.Windows.Window> MinWinCommand
        {
            get
            {
                return _minWinCommand = _minWinCommand ?? new RelayCommand<Window>((win) =>
                {
                    if (win == null) return;
                    win.WindowState = WindowState.Minimized;
                    OnFormMinimize();
                });
            }
        }

        private RelayCommand<Window> _closeWinCommand;
        /// <summary>
        /// 窗体的关闭命令
        /// </summary>
        public RelayCommand<Window> CloseWinCommand
        {
            get
            {
                return _closeWinCommand = _closeWinCommand ?? new RelayCommand<Window>((win) =>
                {
                    if (win == null) return;
                    IsUserClosed = true;
                    __isClosed = true;
                    win.Close();
                });
            }
        }

        private RelayCommand _mouseLeftButtonCommand;
        public RelayCommand MouseLeftButtonCommand
        {
            get
            {
                return _mouseLeftButtonCommand = _mouseLeftButtonCommand ?? new RelayCommand(() =>
                {
                    if (_win != null)
                        _win.DragMove();
                });
            }
        }

        /// <summary>
        /// 窗体正在关闭时执行的回调函数
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnFormClosing(System.ComponentModel.CancelEventArgs e)
        {

        }

        /// <summary>
        /// 窗体最少化时执行的回调函数
        /// </summary>
        protected virtual void OnFormMinimize()
        {

        }

        /// <summary>
        /// 窗体关闭后执行的回调函数
        /// </summary>
        protected virtual void OnFormClosed()
        {

        }

        protected override void OnFrameworkLoadedHandle(FrameworkElement fe, IntPtr winHandler)
        {
            base.OnFrameworkLoadedHandle(fe, winHandler);

            var win = fe as Window;
            if (win == null) return;

            _winHandle = winHandler;
            Title = win.Title;

            win.ResizeMode = ResizeMode.CanMinimize;
            this.IsShowInTaskbar = win.ShowInTaskbar;

            if (IsFitToScreenSize)
            {
                _isMaximized = false;
                FitToScreenSize(win);
            }
            else
                _isMaximized = win.Width == SystemParameters.WorkArea.Width && win.Height == SystemParameters.WorkArea.Height;

            _initWinState = win.WindowState;

            win.Closing -= Win_Closing;
            win.Closing += Win_Closing;
            win.Closed -= Win_Closed;
            win.Closed += Win_Closed;
            _win = win;
            if (_winHandle != IntPtr.Zero && IsShowInTaskbar)
                (_hwndSource = HwndSource.FromHwnd(_winHandle))?.AddHook(WndProc);

            Microsoft.Win32.SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;

            _winList.Add(_weakWin = new WeakReference(this));
            _win.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, (Action)delegate
            {
                _win.Activate();
            });
        }

        private void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            if (IsDisposed || _win == null) return;

            _win.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, (Action)delegate
            {
                if (IsDisposed || _win == null) return;

                if (IsFitToScreenSize)
                {
                    FitToScreenSize(_win);
                }
                else if (_isMaximized || _isFullScreen)
                {
                    _win.Top = _win.Left = 0;
                    if (_isFullScreen)
                    {
                        _win.Width = SystemParameters.WorkArea.Width;
                        _win.Height = SystemParameters.WorkArea.Height;
                    }
                    else
                    {
                        _win.Width = SystemParameters.PrimaryScreenWidth;
                        _win.Height = SystemParameters.PrimaryScreenHeight;
                    }

                }
            });
        }

        protected virtual IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WindowsMessage.WM_SYSCOMMAND
                || msg == 641)
            {
                if (IsShowInTaskbar == false || __isClosed || _win?.Visibility != Visibility.Visible) return IntPtr.Zero;

                int count = _winList.Count;
                Window win = null;
                bool oldTopMost;
                WeakReference weakWin = null;
                VMWinBase vmWinBase = null;

                List<Window> lsbActivate = new List<Window>(Math.Max(0, count - 1));
                for (int i = 0; i < count; ++i)
                {
                    weakWin = _winList[i];
                    if (weakWin.IsAlive)
                    {
                        win = (vmWinBase = weakWin.Target as VMWinBase)?._win;

                        if (vmWinBase != null && vmWinBase.IsDisposed) continue;
                        if (win != null && win.Visibility == Visibility.Visible)
                            lsbActivate.Add(win);
                    }
                }

                count = lsbActivate.Count;
                if (count > 1)
                {
                    for (int i = 0; i < count; ++i)
                    {
                        win = lsbActivate[i];
                        if (win.IsActive == false)
                        {
                            oldTopMost = win.Topmost;
                            win.Topmost = true;
                            if (i == count - 1)
                                win.Activate();
                            win.Topmost = oldTopMost;
                        }
                    }
                }
                else if (msg == WindowsMessage.WM_SYSCOMMAND)
                {
                    win = _win;
                    if (win != null && (int)wParam != WindowsMessage.SC_MINIMIZE)
                    {
                        if (win.IsActive == false)
                            win.Activate();
                    }
                }


                lsbActivate.Clear();
            }

            return IntPtr.Zero;
        }

        public VMWinBase FitToScreenSize(Window win)
        {
            if (win == null) return this;

            IsFitToScreenSize = true;
            double w = SystemParameters.WorkArea.Width*0.83; //980;
            double h = SystemParameters.WorkArea.Height*0.83;//650;
            double top = Math.Max(20, (SystemParameters.WorkArea.Height - h) / 2d);
            double left = Math.Max(20, (SystemParameters.WorkArea.Width - w) / 2d);
            if (win.Top != top)
                win.Top = top;
            if (win.Left != left)
                win.Left = left;

            win.Width = w;
            win.Height = h;

            return this;
        }

        /// <summary>
        /// 获取窗体是否已被显示并且激活
        /// </summary>
        public bool ShowActivated
        {
            get
            {
                if (_win != null)
                    return _win.ShowActivated;
                return false;
            }
        }


        /// <summary>
        /// 关闭窗体
        /// </summary>
        public void Close()
        {
            Dispatcher.CurrentDispatcher.Invoke(delegate
            {
                if (_win != null)
                {
                    IsUserClosed = true;
                    _win.Close();
                }
            });
        }

        /// <summary>
        /// 隐藏窗体
        /// </summary>
        public void Hide()
        {
            Dispatcher.CurrentDispatcher.Invoke(delegate
            {
                if (_win != null)
                    _win.Hide();
            });
        }

        /// <summary>
        /// 显示窗体
        /// </summary>
        public void Show()
        {
            Dispatcher.CurrentDispatcher.Invoke(delegate
            {
                if (_win != null)
                {
                    _win.Show();
                    _win.Activate();
                }
            });
        }

        /// <summary>
        /// 模态显示窗体
        /// </summary>
        public bool? ShowDialog()
        {
            return _win?.ShowDialog();
        }

    }
}
