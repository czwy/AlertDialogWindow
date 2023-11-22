namespace AlertDialogWindow.Helper
{

    /// <summary>
    /// 弹出提示窗体类型
    /// </summary>
    public enum AlertDialogType
    {
        /// <summary>
        /// 信息
        /// </summary>
        Info = 0,
        /// <summary>
        /// 询问
        /// </summary>
        Ask = 1,
        /// <summary>
        /// 警告
        /// </summary>
        Warning = 2,
        /// <summary>
        /// 成功
        /// </summary>
        Success = 3,
        /// <summary>
        /// 失败
        /// </summary>
        Fail = 4,
    }

    /// <summary>
    /// 弹出提示窗体样式
    /// </summary>
    public enum AlertDialogMode
    {
        /// <summary>
        /// 迷你窗体
        /// </summary>
        Mini = 0,
        /// <summary>
        /// 普通窗体
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 完整窗体
        /// </summary>
        Full = 2,
    }

    /// <summary>
    /// 弹出提示窗结果
    /// </summary>
    public enum AlertDialogResult
    {
        /// <summary>
        /// 关闭窗体
        /// </summary>
        Quit = 0,
        /// <summary>
        /// 取消
        /// </summary>
        Cancel = 1,
        /// <summary>
        /// 确认
        /// </summary>
        Confirm = 2
    }

}
