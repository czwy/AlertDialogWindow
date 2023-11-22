using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace AlertDialogWindow.Toolkit
{
    /// <summary>
    /// 可选择文本框控件
    /// </summary>
    public class SelectableTextBlock : TextBlock
    {
        private TextPointer startpoz;
        private TextPointer endpoz;
        private MenuItem copyMenu;
        private MenuItem selectAllMenu;

        public TextRange Selection { get; private set; }
        public bool HasSelection
        {
            get { return Selection != null && !Selection.IsEmpty; }
        }

        #region SelectionBrush

        public static readonly DependencyProperty SelectionBrushProperty =
            DependencyProperty.Register("SelectionBrush", typeof(Brush), typeof(SelectableTextBlock),
                new FrameworkPropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ccadd6ff"))));

        public Brush SelectionBrush
        {
            get { return (Brush)GetValue(SelectionBrushProperty); }
            set { SetValue(SelectionBrushProperty, value); }
        }

        #endregion

        public SelectableTextBlock()
        {
            Cursor = Cursors.IBeam;
            Focusable = true;
            var contextMenu = new ContextMenu();
            ContextMenu = contextMenu;

            copyMenu = new MenuItem();
            copyMenu.Header = "复制";
            copyMenu.InputGestureText = "Ctrl + C";
            copyMenu.Click += (ss, ee) =>
            {
                Copy();
            };
            contextMenu.Items.Add(copyMenu);

            selectAllMenu = new MenuItem();
            selectAllMenu.Header = "全选";
            selectAllMenu.InputGestureText = "Ctrl + A";
            selectAllMenu.Click += (ss, ee) =>
            {
                SelectAll();
            };
            contextMenu.Items.Add(selectAllMenu);

            ContextMenuOpening += contextMenu_ContextMenuOpening;
        }

        private void SelectableTextBlock_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ContextMenu != null && ContextMenu.IsOpen == false)
                ClearSelection();
        }

        private void contextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            copyMenu.IsEnabled = HasSelection;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            e.Handled = true;
            Keyboard.Focus(this);
            ReleaseMouseCapture();
            base.OnMouseLeftButtonUp(e);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            e.Handled = true;
            LostFocus -= SelectableTextBlock_LostFocus;

            if (e.ClickCount > 1)
            {
                SelectAll();
            }
            else
            {
                try
                {
                    var point = e.GetPosition(this);
                    startpoz = GetPositionFromPoint(point, true);
                    CaptureMouse();
                }
                catch { }
            }
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            LostFocus -= SelectableTextBlock_LostFocus;
            LostFocus += SelectableTextBlock_LostFocus;

            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            e.Handled = true;
            if (IsMouseCaptured)
            {
                var point = e.GetPosition(this);
                endpoz = GetPositionFromPoint(point, true);

                ClearSelection();
                Selection = new TextRange(startpoz, endpoz);
                Selection.ApplyPropertyValue(TextElement.BackgroundProperty, SelectionBrush);
                CommandManager.InvalidateRequerySuggested();

                OnSelectionChanged(EventArgs.Empty);
            }

            base.OnMouseMove(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (e.Key == Key.C)
                    Copy();
                else if (e.Key == Key.A)
                    SelectAll();
            }

            base.OnKeyDown(e);
        }

        public bool Copy()
        {
            if (HasSelection)
            {
                Clipboard.SetDataObject(Selection.Text);
                return true;
            }
            return false;
        }

        public void ClearSelection()
        {
            var contentRange = new TextRange(ContentStart, ContentEnd);
            contentRange.ApplyPropertyValue(TextElement.BackgroundProperty, null);
            Selection = null;
        }

        public void SelectAll()
        {
            Selection = new TextRange(ContentStart, ContentEnd);
            Selection.ApplyPropertyValue(TextElement.BackgroundProperty, SelectionBrush);
        }

        public event EventHandler SelectionChanged;

        protected virtual void OnSelectionChanged(EventArgs e)
        {
            var handler = this.SelectionChanged;
            if (handler != null)
                handler(this, e);
        }


    }
}
