using System;
using System.Runtime.InteropServices;

namespace AlertDialogWindow.Toolkit
{
    internal class HtmlTextBlockProcessInfo
    {
        public IntPtr hProcess = IntPtr.Zero;
        public IntPtr hThread = IntPtr.Zero;
        public Int32 ProcessID =0;
        public Int32 ThreadID =0;
    }

    internal class HtmlTextBlockHeader
    {
#if CF
        const string user32 = "coredll.dll";
        const string kernel32 = "coredll.dll";
#else
        const string user32 = "user32.dll";
        const string kernel32 = "kernel32.dll";
#endif

        [DllImport(kernel32)]
        public static extern Int32 CreateProcess(string appName,
            string cmdLine, IntPtr lpProcessAttributes, IntPtr lpThreadAttributes,
            Int32 boolInheritHandles, Int32 dwCreationFlags, IntPtr lpEnvironment,
            IntPtr lpszCurrentDir, Byte[] si, HtmlTextBlockProcessInfo pi);

        [DllImport(kernel32)]
        public static extern Int32 WaitForSingleObject(IntPtr handle, Int32 wait);

        [DllImport(kernel32)]
        public static extern Int32 GetLastError();

        [DllImport(kernel32)]
        public static extern Int32 CloseHandle(IntPtr handle);


    }
}
