using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AlertDialogWindow.Toolkit
{
    /// <summary>
    /// 图片按钮
    /// </summary>
    public class ImageButton : Button
    {
        /// <summary>
        /// 正常时的图片
        /// </summary>
        public ImageSource NormalImage
        {
            get { return (ImageSource)GetValue(NormalImageProperty); }
            set { SetValue(NormalImageProperty, value); }
        }

        public static readonly DependencyProperty NormalImageProperty =
            DependencyProperty.Register("NormalImage", typeof(ImageSource), typeof(ImageButton), new UIPropertyMetadata(null, OnNormalImagePropertyChanged));


        private static void OnNormalImagePropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            //var btn = dp as ImageButton;
            //if (btn._sourceImage == null)
            //    btn._sourceImage = e.NewValue as ImageSource;

            //btn.SetEnable(btn.IsEnabled);
        }

        /// <summary>
        /// 鼠标在按钮上方时的图片
        /// </summary>
        public ImageSource HoverImage
        {
            get { return (ImageSource)GetValue(HoverImageProperty); }
            set { SetValue(HoverImageProperty, value); }
        }

        public static readonly DependencyProperty HoverImageProperty =
            DependencyProperty.Register("HoverImage", typeof(ImageSource), typeof(ImageButton), new UIPropertyMetadata());

        /// <summary>
        /// 鼠标在按钮按下时的图片
        /// </summary>
        public ImageSource DownImage
        {
            get { return (ImageSource)GetValue(DownImageProperty); }
            set { SetValue(DownImageProperty, value); }
        }

        public static readonly DependencyProperty DownImageProperty =
            DependencyProperty.Register("DownImage", typeof(ImageSource), typeof(ImageButton), new UIPropertyMetadata());

        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }

        public ImageButton()
        {
            //this.IsEnabledChanged += MyImageButton_IsEnabledChanged;
            //this.Loaded += ImageButton_Loaded;
            //this.Unloaded += ImageButton_Unloaded;
        }

        private void ImageButton_Unloaded(object sender, RoutedEventArgs e)
        {
            //ButtonClickLimitAttach.SetMinMilliseconds(this, 0);
        }

        private void ImageButton_Loaded(object sender, RoutedEventArgs e)
        {
            //ButtonClickLimitAttach.SetMinMilliseconds(this, 300);
        }

        //private BitmapSource _disableImageSource;
        //private ImageSource _sourceImage;
        private void SetEnable(bool isEnabled)
        {
            //if (DesignerProperties.GetIsInDesignMode(this)) return;

            //if (isEnabled == false)
            //{
            //    if (_disableImageSource == null && this.NormalImage != null)
            //    {
            //        _sourceImage = this.NormalImage;
            //        _disableImageSource = (_sourceImage as BitmapSource).ToGrayBitmap(); ;
            //    }
            //    if(_disableImageSource!=null)
            //        this.NormalImage = _disableImageSource;

            //}
            //else
            //{
            //    if (_sourceImage != null)
            //        this.NormalImage = _sourceImage;
            //}
        }

        private void MyImageButton_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool b = bool.Parse(e.NewValue.ToString());
            SetEnable(b);
        }

    }
}
