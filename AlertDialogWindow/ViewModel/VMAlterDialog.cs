using AlertDialogWindow.Helper;
using GalaSoft.MvvmLight.Command;
using System;
using System.Windows.Threading;

namespace AlertDialogWindow.ViewModel
{
    public class VMAlterDialog : VMWinBase
    {
        DispatcherTimer CountdownTimer = null;

        internal AlertDialogResult IsConfirm = AlertDialogResult.Quit;

        private int _countdown = -1;
        public int Countdown
        {
            get { return _countdown; }
            set
            {
                Set(ref _countdown, value);
                if (value >= 0)
                {
                    if (!IsCountdown)
                        IsCountdown = true;
                }
                else
                {
                    if (IsCountdown)
                        IsCountdown = false;
                }
            }
        }

        private bool _isCountdown = false;
        public bool IsCountdown
        {
            get { return _isCountdown; }
            set
            {
                Set(ref _isCountdown, value);
                if (value)
                {
                    CountdownTimer = new DispatcherTimer();
                    CountdownTimer.Interval = new TimeSpan(10000000);   //时间间隔为一秒
                    CountdownTimer.Tick += CountdownTimer_Tick;
                    CountdownTimer.Start();
                }
            }
        }

        private AlertDialogType _dialogType;
        /// <summary>
        /// 提示窗体的类型
        /// </summary>
        public AlertDialogType DialogType
        {
            get { return _dialogType; }
            set { Set(ref _dialogType, value, nameof(DialogType)); }
        }

        private AlertDialogMode _dialogMode;
        /// <summary>
        /// 提示弹窗的样式（完全模式、普通、迷你）
        /// </summary>
        public AlertDialogMode DialogMode
        {
            get { return _dialogMode; }
            set { Set(ref _dialogMode, value); }
        }

        private string _content;
        /// <summary>
        /// 窗体的内容
        /// </summary>
        public string Content
        {
            get { return _content; }
            set { Set(ref _content, value, nameof(Content)); }
        }

        private string _subContent;
        /// <summary>
        /// 窗体子内容
        /// </summary>
        public string SubContent
        {
            get { return _subContent; }
            set { Set(ref _subContent, value); }
        }

        private string _yesButtonText = "确定";
        /// <summary>
        /// 确定按钮文字内容
        /// </summary>
        public string YesButtonText
        {
            get { return _yesButtonText; }
            set { Set(ref _yesButtonText, value, nameof(YesButtonText)); }
        }

        private string _noButtonText = "取消";
        /// <summary>
        /// 确定按钮文字内容
        /// </summary>
        public string NoButtonText
        {
            get { return _noButtonText; }
            set { Set(ref _noButtonText, value, nameof(NoButtonText)); }
        }

        private RelayCommand _yesCommand;
        public RelayCommand YesCommand
        {
            get
            {
                return _yesCommand = _yesCommand ?? new RelayCommand(() =>
                {
                    if (CountdownTimer != null)
                    {
                        if (CountdownTimer.IsEnabled)
                            CountdownTimer.Stop();
                        CountdownTimer = null;
                    }
                    DialogResult = true;
                    IsConfirm = AlertDialogResult.Confirm;
                });
            }
        }

        private RelayCommand _noCommand;
        public RelayCommand NoCommand
        {
            get
            {
                return _noCommand = _noCommand ?? new RelayCommand(() =>
                {
                    if (CountdownTimer != null)
                    {
                        if (CountdownTimer.IsEnabled)
                            CountdownTimer.Stop();
                        CountdownTimer = null;
                    }
                    IsConfirm = AlertDialogResult.Cancel;
                    Close();
                });
            }
        }

        public VMAlterDialog()
        {

        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            if (--Countdown <= 0)
            {
                if (CountdownTimer != null)
                {
                    CountdownTimer.Stop();
                    CountdownTimer.Tick -= CountdownTimer_Tick;
                    DialogResult = true;
                }
            }
        }
    }
}
