using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace VinylVerse
{
    public static class Animator
    {
        /// <summary>
        /// Animates the visibility of a UI element.
        /// </summary>
        /// <param name="element">The UI element to animate.</param>
        /// <param name="isVisible">True to show the element, False to hide it.</param>
        /// <param name="durationMs">The duration of the animation in milliseconds (default is 200).</param>
        public static void AnimateVisibility(UIElement element, bool isVisible, int durationMs = 200)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            DoubleAnimation animation = new DoubleAnimation
            {
                From = isVisible ? 0 : 1,
                To = isVisible ? 1 : 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(durationMs))
            };

            animation.Completed += (s, e) =>
            {
                element.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
            };

            element.BeginAnimation(UIElement.OpacityProperty, animation);

            if (isVisible)
            {
                element.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Animates the color change of a UI element using hex color codes.
        /// </summary>
        /// <param name="element">The element whose color should be animated (e.g., Border or TextBlock).</param>
        /// <param name="propertyPath">The property to animate (e.g., Background, Foreground).</param>
        /// <param name="fromHex">The starting color in hex format (e.g., "#FF0000").</param>
        /// <param name="toHex">The ending color in hex format (e.g., "#00FF00").</param>
        /// <param name="durationMs">The duration of the animation in milliseconds (default is 300).</param>
        public static void AnimateColorChange(FrameworkElement element, DependencyProperty propertyPath, string fromHex, string toHex, int durationMs = 300)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            Color fromColor = (Color)ColorConverter.ConvertFromString(fromHex);
            Color toColor = (Color)ColorConverter.ConvertFromString(toHex);

            ColorAnimation colorAnimation = new ColorAnimation
            {
                From = fromColor,
                To = toColor,
                Duration = new Duration(TimeSpan.FromMilliseconds(durationMs))
            };

            SolidColorBrush animatedBrush = new SolidColorBrush(fromColor);
            animatedBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);

            element.SetValue(propertyPath, animatedBrush);
        }

        /// <summary>
        /// Smoothly animates the width of a custom or standard control to a specified target width.
        /// </summary>
        /// <param name="element">The UIElement or custom control to animate.</param>
        /// <param name="targetWidth">The target width in pixels.</param>
        /// <param name="durationMs">The duration of the animation in milliseconds (default: 300).</param>
        public static void StretchWidthToRight(UIElement element, double targetWidth, int durationMs = 300)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            // If the element is a FrameworkElement, directly animate the Width property
            if (element is FrameworkElement frameworkElement)
            {
                AnimateFrameworkElementWidth(frameworkElement, targetWidth, durationMs);
                return;
            }

            // For custom controls, attempt to animate the Width property using reflection
            var widthProperty = element.GetType().GetProperty("Width");
            if (widthProperty == null || !widthProperty.CanWrite)
                throw new InvalidOperationException("The element does not have a writable Width property.");

            // Get the current width value
            double currentWidth = (double)(widthProperty.GetValue(element) ?? 0);

            // Ensure a valid starting width
            if (currentWidth <= 0)
                throw new InvalidOperationException("The element's Width is not initialized or is set to 0.");

            // Animate the custom Width property
            AnimateCustomControlWidth(element, widthProperty, currentWidth, targetWidth, durationMs);
        }

        /// <summary>
        /// Animates the Width property of a FrameworkElement.
        /// </summary>
        private static void AnimateFrameworkElementWidth(FrameworkElement element, double targetWidth, int durationMs)
        {
            // Ensure Width is initialized
            if (double.IsNaN(element.Width) || element.Width == 0)
                element.Width = element.ActualWidth > 0 ? element.ActualWidth : 1;

            // Create the animation
            var widthAnimation = new DoubleAnimation
            {
                From = element.Width,
                To = targetWidth,
                Duration = new Duration(TimeSpan.FromMilliseconds(durationMs)),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            };

            // Apply the animation
            element.BeginAnimation(FrameworkElement.WidthProperty, widthAnimation);
        }

        /// <summary>
        /// Animates the Width property of a custom control using reflection.
        /// </summary>
        private static void AnimateCustomControlWidth(UIElement element, System.Reflection.PropertyInfo widthProperty, double currentWidth, double targetWidth, int durationMs)
        {
            var animation = new DoubleAnimation
            {
                From = currentWidth,
                To = targetWidth,
                Duration = new Duration(TimeSpan.FromMilliseconds(durationMs)),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            };

            // Manually apply the animation by updating the property over time
            var storyboard = new Storyboard();
            Storyboard.SetTarget(animation, element);
            Storyboard.SetTargetProperty(animation, new PropertyPath(widthProperty.Name));
            storyboard.Children.Add(animation);

            storyboard.Begin();
        }

        /// <summary>
        /// Анимация перехода между окнами с появлением следующего окна на месте предыдущего.
        /// </summary>
        /// <param name="currentWindow">Текущее окно.</param>
        /// <param name="newWindow">Новое окно.</param>
        /// <param name="fadeOutDurationMs">Длительность анимации затухания текущего окна в миллисекундах (по умолчанию: 150).</param>
        /// <param name="fadeInDurationMs">Длительность анимации появления нового окна в миллисекундах (по умолчанию: 150).</param>
        public static void AnimateWindowTransition(Window currentWindow, Window newWindow, int fadeOutDurationMs = 150, int fadeInDurationMs = 150)
        {
            if (currentWindow == null)
                throw new ArgumentNullException(nameof(currentWindow));

            if (newWindow == null)
                throw new ArgumentNullException(nameof(newWindow));

            newWindow.Left = currentWindow.Left;
            newWindow.Top = currentWindow.Top;

            DoubleAnimation fadeOutAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = new Duration(TimeSpan.FromMilliseconds(fadeOutDurationMs)),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            };

            fadeOutAnimation.Completed += (s, e) =>
            {
                currentWindow.Close();

                newWindow.Opacity = 0;
                newWindow.Show();

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0.0,
                    To = 1.0,
                    Duration = new Duration(TimeSpan.FromMilliseconds(fadeInDurationMs)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
                };

                newWindow.BeginAnimation(Window.OpacityProperty, fadeInAnimation);
            };

            currentWindow.BeginAnimation(Window.OpacityProperty, fadeOutAnimation);
        }
    }
}