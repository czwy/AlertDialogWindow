using System;
using System.Runtime.InteropServices;

namespace AlertDialogWindow.Helper
{
    public class WindowNativeMethods
    {
        /// <summary>   
        /// 带有外边框和标题的windows的样式   
        /// </summary>   
        public const long WS_CAPTION = 0x00C00000L;
        public const long WS_CAPTION_2 = 0X00C0000L;

        // public const long WS_BORDER = 0X0080000L;   
        /// <summary>   
        /// window 扩展样式 分层显示   
        /// </summary>   
        public const long WS_EX_LAYERED = 0x00080000L;
        public const long WS_CHILD = 0x40000000L;

        /// <summary>   
        /// 带有alpha的样式   
        /// </summary>   
        public const long LWA_ALPHA = 0x00000002L;

        /// <summary>   
        /// 颜色设置   
        /// </summary>   
        public const long LWA_COLORKEY = 0x00000001L;

        /// <summary>   
        /// window的基本样式   
        /// </summary>   
        public const int GWL_STYLE = -16;

        /// <summary>   
        /// window的扩展样式   
        /// </summary>   
        public const int GWL_EXSTYLE = -20;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        /// <summary>   
        /// 设置窗体的样式   
        /// </summary>   
        /// <param name="handle">操作窗体的句柄</param>   
        /// <param name="oldStyle">进行设置窗体的样式类型.</param>   
        /// <param name="newStyle">新样式</param>
        [DllImport("User32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void SetWindowLong(IntPtr handle, int oldStyle, int newStyle);

        /// <summary>   
        /// 获取窗体指定的样式.   
        /// </summary>   
        /// <param name="handle">操作窗体的句柄</param>   
        /// <param name="style">要进行返回的样式</param>   
        /// <returns>当前window的样式</returns>   
        [DllImport("User32.dll")]
        public static extern long GetWindowLong(IntPtr handle, int style);

        /// <summary>   
        /// 设置窗体的工作区域.   
        /// </summary>   
        /// <param name="handle">操作窗体的句柄.</param>   
        /// <param name="handleRegion">操作窗体区域的句柄.</param>   
        /// <param name="regraw">if set to <c>true</c> [regraw].</param>   
        /// <returns>返回值</returns>   
        [DllImport("User32.dll")]
        public static extern int SetWindowRgn(IntPtr handle, IntPtr handleRegion, bool regraw);

        /// <summary>   
        /// 创建带有圆角的区域.   
        /// </summary>   
        /// <param name="x1">左上角坐标的X值.</param>   
        /// <param name="y1">左上角坐标的Y值.</param>   
        /// <param name="x2">右下角坐标的X值.</param>   
        /// <param name="y2">右下角坐标的Y值.</param>   
        /// <param name="width">圆角椭圆的width.</param>   
        /// <param name="height">圆角椭圆的height.</param>   
        /// <returns>hRgn的句柄</returns>   
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateRoundRectRgn(int x1, int y1, int x2, int y2, int width, int height);

        /// <summary>   
        /// Sets the layered window attributes.   
        /// </summary>   
        /// <param name="handle">要进行操作的窗口句柄</param>   
        /// <param name="colorKey">RGB的值</param>   
        /// <param name="alpha">Alpha的值，透明度</param>   
        /// <param name="flags">附带参数</param>   
        /// <returns>true or false</returns>   
        [DllImport("User32.dll")]
        public static extern bool SetLayeredWindowAttributes(IntPtr handle, ulong colorKey, byte alpha, long flags);
        //=================================================================================  
        /// <summary>  
        /// 设置窗体为无边框风格  
        /// </summary>  
        /// <param name="hWnd"></param>  
        public static void SetWindowNoBorder(IntPtr hWnd)
        {
            long oldstyle = GetWindowLong(hWnd, GWL_STYLE);
            SetWindowLong(hWnd, GWL_STYLE, (int)(oldstyle & (~(WS_CAPTION | WS_CAPTION_2))));
        }

        /// <summary>
        /// 隐藏任务栏
        /// </summary>
        /// <param name="isHide"></param>
        public static void HideTask(bool isHide)
        {
            try
            {

                IntPtr trayHwnd = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Shell_TrayWnd", null);
                IntPtr hStar = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Button", null);
                IntPtr h360Button = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "XTaskBarBtn", null);

                if (isHide)
                {
                    ShowWindow(trayHwnd, 0);
                    ShowWindow(hStar, 0);
                    ShowWindow(h360Button, 0);
                }
                else
                {
                    ShowWindow(trayHwnd, 1);
                    ShowWindow(hStar, 1);
                    ShowWindow(h360Button, 1);
                }

            }
            catch { }
        }

        /// <summary>
        /// <para>该函数将一个消息放入（寄送）到与指定窗口创建的线程相联系消息队列里，不等待线程</para>
        /// <para>处理消息就返回，是异步消息模式。消息队列里的消息通过调用GetMessage和PeekMessage取得。</para>
        /// </summary>
        /// <param name="hWnd"><para>接收消息的那个窗口的句柄。</para>
        /// <para>如设为HWND_BROADCAST，表示投递给系统中的所有顶级窗口。</para>
        /// <para>如设为零，表示投递一条线程消息（参考PostThreadMessage）</para></param>
        /// <param name="Msg">消息标识符</param>
        /// <param name="wParam">具体由消息决定</param>
        /// <param name="lParam">具体由消息决定</param>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
        public static extern bool SendMessage(IntPtr hwnd, int msg, int wParam, ref COPYDATASTRUCT cds);
        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
        public static extern bool SendMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
        public static extern bool SendMessage(IntPtr hwnd, int msg, int wParam, int lParam);
        [DllImport("user32.dll", EntryPoint = "IsWindow")]
        public static extern bool IsWindow(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref Win32Point pt);

        [DllImport("user32.dll")]
        public static extern bool ScreenToClient(IntPtr hwnd, ref Win32Point pt);

        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

        [DllImport("user32.dll")]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(int hWnd);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("User32.dll")]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32")]
        public static extern bool IsIconic(IntPtr hWnd);
        [DllImport("user32")]
        public static extern bool OpenIcon(IntPtr hWnd);

        /// <summary>
        /// 该函数将焦点切换指定的窗口，并将其带到前台。
        /// </summary>
        /// <param name="hWnd">要切换到的窗口的句柄</param>
        /// <param name="fAltTab">TRUE 表示使用 Alt/Ctl+Tab 键的先后次序来切换窗口，否则设为 FALSE</param>
        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        [DllImport("imm32.dll")]
        public static extern IntPtr ImmGetContext(IntPtr hWnd);
        [DllImport("Imm32.dll")]
        public static extern bool ImmReleaseContext(IntPtr hWnd, IntPtr hIMC);
        [DllImport("Imm32.dll")]
        public static extern bool ImmGetConversionStatus(IntPtr himc, ref int lpdw, ref int lpdw2);
        [DllImport("imm32.dll")]
        public static extern bool ImmGetOpenStatus(IntPtr himc);
        [DllImport("imm32.dll")]
        public static extern bool ImmSetOpenStatus(IntPtr himc, bool b);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public POINT(double x, double y)
        {
            X = Convert.ToInt32(x);
            Y = Convert.ToInt32(y);
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct Win32Point
    {
        public Int32 X;
        public Int32 Y;
    };

    //定义数据传输的结构
    [StructLayout(LayoutKind.Sequential)]
    public struct COPYDATASTRUCT
    {
        public int dwData; //可以是任意值
        public int cbData;    //指定lpData内存区域的字节数
        [MarshalAs(UnmanagedType.LPStr)]
        public string lpData; //发送给目录窗口所在进程的数据
    }

    #region Window消息列表
    /// <summary>
    /// Window消息列表
    /// </summary>
    public static class WindowsMessage
    {
        /// <summary>
        /// 
        /// </summary>
        public const int WM_NULL = 0x0000;
        /// <summary>
        /// 应用程序创建一个窗口 
        /// </summary>
        public const int WM_CREATE = 0x0001;
        /// <summary>
        /// 一个窗口被销毁 
        /// </summary>
        public const int WM_DESTROY = 0x0002;
        /// <summary>
        /// 移动一个窗口
        /// </summary>
        public const int WM_MOVE = 0x0003;
        /// <summary>
        /// 改变一个窗口的大小
        /// </summary>
        public const int WM_SIZE = 0x0005;
        /// <summary>
        /// 一个窗口被激活或失去激活状态；
        /// </summary>
        public const int WM_ACTIVATE = 0x0006;
        /// <summary>
        /// 获得焦点后 
        /// </summary>
        public const int WM_SETFOCUS = 0x0007;
        /// <summary>
        /// 失去焦点
        /// </summary>
        public const int WM_KILLFOCUS = 0x0008;
        /// <summary>
        /// 改变enable状态
        /// </summary>
        public const int WM_ENABLE = 0x000A;
        /// <summary>
        /// 移动鼠标
        /// </summary>
        public const int WM_MOUSEMOVE = 0x0200;
        /// <summary>
        /// 按下鼠标左键
        /// </summary>
        public const int WM_LBUTTONDOWN = 0x0201;
        /// <summary>
        /// 释放鼠标左键 
        /// </summary>
        public const int WM_LBUTTONUP = 0x0202;
        /// <summary>
        /// 双击鼠标左键 
        /// </summary>
        public const int WM_LBUTTONDBLCLK = 0x0203;
        /// <summary>
        /// 按下鼠标右键 
        /// </summary>
        public const int WM_RBUTTONDOWN = 0x0204;
        /// <summary>
        /// 释放鼠标右键
        /// </summary>
        public const int WM_RBUTTONUP = 0x0205;
        /// <summary>
        /// 双击鼠标右键
        /// </summary>
        public const int WM_RBUTTONDBLCLK = 0x0206;
        /// <summary>
        /// 按下鼠标中键
        /// </summary>
        public const int WM_MBUTTONDOWN = 0x0207;
        /// <summary>
        /// 释放鼠标中键 
        /// </summary>
        public const int WM_MBUTTONUP = 0x0208;
        /// <summary>
        /// 双击鼠标中键 
        /// </summary>
        public const int WM_MBUTTONDBLCLK = 0x0209;
        /// <summary>
        /// 当鼠标轮子转动时发送此消息给当前有焦点的控件 
        /// </summary>
        public const int WM_MOUSEWHEEL = 0x020A;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_MOUSELAST = 0x020A;
        public const int WM_SYSCOMMAND = 0x112;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_RECOVERSIZE = 0xF020;
        public const int SC_CLOSE = 0xF060;
        /// <summary>
        /// 用户消息
        /// </summary>
        public const int WM_USER = 0x0400;
        /// <summary>
        /// 传送字符串
        /// </summary>
        public const int WM_COPYDATA = 0x004A;
    }
    #endregion
}
