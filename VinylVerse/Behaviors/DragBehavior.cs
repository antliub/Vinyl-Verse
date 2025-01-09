using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;

namespace VinylVerse.Behaviors
{
    public static class DragBehavior
    {
        public static readonly DependencyProperty IsDragEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsDragEnabled",
                typeof(bool),
                typeof(DragBehavior),
                new PropertyMetadata(false, OnIsDragEnabledChanged));

        public static bool GetIsDragEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDragEnabledProperty);
        }

        public static void SetIsDragEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDragEnabledProperty, value);
        }

        private static void OnIsDragEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window window)
            {
                if ((bool)e.NewValue)
                {
                    window.MouseDown += Window_MouseDown;
                }
                else
                {
                    window.MouseDown -= Window_MouseDown;
                }
            }
        }

        private static void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var window = sender as Window;
            if (window != null && e.ChangedButton == MouseButton.Left)
            {
                if (e.OriginalSource is Border border && border.Background != null)
                {
                    window.DragMove();
                }
            }
        }
    }
}