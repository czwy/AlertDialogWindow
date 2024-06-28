using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AlertDialogWindow.Toolkit
{
    /// <summary>
    /// html高亮文本快
    /// </summary>
    public class HtmlHighlightTextBlock : TextBlock
    {
        /// <summary>
        /// 最大的高亮匹配数
        /// </summary>
        public int MaxHighlightCount { get; set; } = 0;

        /// <summary>
        /// 高亮的内容
        /// </summary>
        public string Highlight
        {
            get { return (string)GetValue(HighlightProperty); }
            set { SetValue(HighlightProperty, value); }
        }

        public static readonly DependencyProperty HighlightProperty =
        DependencyProperty.Register("Highlight", typeof(string), typeof(HtmlHighlightTextBlock), new UIPropertyMetadata("",new PropertyChangedCallback(OnHeightlightChanged)));


        public static DependencyProperty HtmlProperty = DependencyProperty.Register("Html", typeof(string),
                typeof(HtmlHighlightTextBlock), new UIPropertyMetadata("", (s,e)=> s.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    OnHtmlChanged(s,e);
                }, System.Windows.Threading.DispatcherPriority.Background)));

        /// <summary>
        /// 高亮的颜色
        /// </summary>
        public Color HeightlightColor
        {
            get { return (Color)GetValue(HeightlightColorProperty); }
            set { SetValue(HeightlightColorProperty, value); }
        }

        public static readonly DependencyProperty HeightlightColorProperty =
            DependencyProperty.Register("HeightlightColor", typeof(Color), typeof(HtmlHighlightTextBlock), new UIPropertyMetadata(Colors.Transparent, new PropertyChangedCallback(OnHeightlightChanged)));
        
        /// <summary>
        /// html内容
        /// </summary>
        public string Html { get { return (string)GetValue(HtmlProperty); } set { SetValue(HtmlProperty, value); } }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Parse(Html);
        }

        private string ColorToHtmlStr(ref Color color)
        {
            return new StringBuilder(9).Append("#").Append(color.A.ToString("x2")).Append(color.R.ToString("x2")).Append(color.G.ToString("x2")).Append(color.B.ToString("x2")).ToString();
        }

        private void Parse(string html)
        {
            if (string.IsNullOrEmpty(html)) { Inlines.Clear(); return; }

            string highlight = Highlight;
            if (!String.IsNullOrEmpty(highlight))
            {
                int idx = html.IndexOf(highlight, StringComparison.InvariantCultureIgnoreCase);
                string formatStr = "{0}[b]{1}[/b]{2}";
                Color heightlightColor = HeightlightColor;
                int l =7;
                if (heightlightColor != Colors.Transparent)
                {
                    string format = new StringBuilder().Append("{0}[font color=").Append(ColorToHtmlStr(ref heightlightColor)).Append("][b]{1}[/b][/font]{2}").ToString();
                    l = l+ (format.Length - l);
                    formatStr = format;
                }
                int maxHighlightCount = Math.Max(0,MaxHighlightCount);
                if (maxHighlightCount < 1)
                {
                    while (idx != -1)
                    {
                        html = String.Format(formatStr,
                            html.Substring(0, idx), html.Substring(idx, highlight.Length), html.Substring(idx + highlight.Length));
                        idx = html.IndexOf(highlight, Math.Min(idx + l, html.Length), StringComparison.InvariantCultureIgnoreCase);
                    }
                }
                else
                {
                    int count = 1;
                    while (idx != -1)
                    {
                        html = String.Format(formatStr,
                            html.Substring(0, idx), html.Substring(idx, highlight.Length), html.Substring(idx + highlight.Length));
                        idx = html.IndexOf(highlight, Math.Min(idx + l, html.Length), StringComparison.InvariantCultureIgnoreCase);
                        ++count;
                        if (count >= maxHighlightCount) break;
                    }
                }
            }                

            Inlines.Clear();
            HtmlTagTree tree = new HtmlTagTree();
            HtmlParser parser = new HtmlParser(tree); //output
            parser.Parse(new StringReader(html));     //input

            HtmlUpdater updater = new HtmlUpdater(this); //output
            updater.Update(tree);
        }

        public static void OnHtmlChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            HtmlHighlightTextBlock sender = (HtmlHighlightTextBlock)s;
            sender.Parse((string)e.NewValue);
        }

        public static void OnHeightlightChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            HtmlHighlightTextBlock sender = (HtmlHighlightTextBlock)s;
            sender.Parse(sender.Html);
        }

        public HtmlHighlightTextBlock()
        {

        }

    }
}
