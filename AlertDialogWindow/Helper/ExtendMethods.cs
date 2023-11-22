using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace AlertDialogWindow.Helper
{
    public static class ExtendMethods
    {
        public static IEnumerable<T> FindAllVisualChilds<T>(this DependencyObject reference)
            where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(reference);
            for (int i = 0; i < count; ++i)
            {
                DependencyObject child = VisualTreeHelper.GetChild(reference, i);
                if (child != null && child is T)
                {
                    yield return (T)child;
                }
                else
                {
                    if (child != null)
                    {
                        foreach (DependencyObject item in FindAllVisualChilds<T>(child))
                        {
                            yield return (T)item;
                        }
                    }
                }
            }
        }

        public static T FindFirstVisualChild<T>(this DependencyObject reference)
            where T : DependencyObject
        {
            if (reference == null) return default(T);

            int count = VisualTreeHelper.GetChildrenCount(reference);
            for (int i = 0; i < count; ++i)
            {
                DependencyObject child = VisualTreeHelper.GetChild(reference, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    child = FindFirstVisualChild<T>(child);
                    if (child != null)
                        return (T)child;
                }
            }

            return default(T);
        }

        public static T FindFirstVisualParent<T>(this DependencyObject reference)
            where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(reference);
            if (parent == null) return default(T);
            if (parent is T)
                return (T)parent;

            while ((parent = VisualTreeHelper.GetParent(parent)) != null)
            {
                if (parent is T)
                    return (T)parent;
            }

            return default(T);
        }
    }
}
