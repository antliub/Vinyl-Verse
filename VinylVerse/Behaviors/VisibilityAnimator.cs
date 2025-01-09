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
        /// Animates the stretching of a UIElement in a specified direction by pixel values.
        /// </summary>
        /// <param name="element">The UIElement to animate.</param>
        /// <param name="direction">The direction to stretch ("left", "right", "up", or "down").</param>
        /// <param name="durationMs">The duration of the animation in milliseconds (default: 300).</param>
        /// <param name="targetSize">The target size in pixels for the stretch.</param>
        /// <exception cref="ArgumentNullException">Thrown if the element is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the direction is invalid.</exception>
        public static void StretchElement(UIElement element, string direction, int durationMs = 300, double targetSize = 100)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            if (string.IsNullOrWhiteSpace(direction))
                throw new ArgumentException("Direction cannot be null or whitespace.", nameof(direction));

            element.RenderTransformOrigin = new Point(0.5, 0.5);

            DoubleAnimation sizeAnimation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(durationMs)),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut },
                From = null,
                To = targetSize
            };

            switch (direction.ToLower())
            {
                case "right":
                    if (element is FrameworkElement frameworkElementRight)
                    {
                        sizeAnimation.From = frameworkElementRight.Width;
                        frameworkElementRight.BeginAnimation(FrameworkElement.WidthProperty, sizeAnimation);
                    }
                    break;

                case "left":
                    if (element is FrameworkElement frameworkElementLeft)
                    {
                        sizeAnimation.From = frameworkElementLeft.Width;
                        frameworkElementLeft.BeginAnimation(FrameworkElement.WidthProperty, sizeAnimation);
                    }
                    break;

                case "up":
                    if (element is FrameworkElement frameworkElementUp)
                    {
                        sizeAnimation.From = frameworkElementUp.Height;
                        frameworkElementUp.BeginAnimation(FrameworkElement.HeightProperty, sizeAnimation);
                    }
                    break;

                case "down":
                    if (element is FrameworkElement frameworkElementDown)
                    {
                        sizeAnimation.From = frameworkElementDown.Height;
                        frameworkElementDown.BeginAnimation(FrameworkElement.HeightProperty, sizeAnimation);
                    }
                    break;

                default:
                    throw new ArgumentException("Invalid direction. Valid values are 'right', 'left', 'up', 'down'.");
            }
        }
    }
}