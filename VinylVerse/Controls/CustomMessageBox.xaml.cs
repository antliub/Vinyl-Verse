using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace VinylVerse.Controls
{
    public partial class CustomMessageBox : Window
    {
        public CustomMessageBox(string message, int duration = 5000)
        {
            InitializeComponent();
            NotificationText.Text = message;

            Loaded += (s, e) =>
            {
                AnimateIn();
                StartAutoClose(duration);
            };
        }

        private void AnimateIn()
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.WorkArea.Height;

            Left = screenWidth - Width - 20;
            Top = screenHeight - Height - 20;

            var animation = new DoubleAnimation
            {
                From = screenHeight,
                To = Top,
                Duration = TimeSpan.FromSeconds(0.5),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            BeginAnimation(TopProperty, animation);
        }

        private void StartAutoClose(int duration)
        {
            var closeTimer = new System.Timers.Timer(duration);
            closeTimer.Elapsed += (s, e) =>
            {
                closeTimer.Stop();
                Dispatcher.Invoke(AnimateOut);
            };
            closeTimer.Start();
        }

        private void AnimateOut()
        {
            var animation = new DoubleAnimation
            {
                From = Opacity,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5)
            };
            animation.Completed += (s, e) => Close();
            BeginAnimation(OpacityProperty, animation);
        }

        public static void ShowNotification(string message, int duration = 5000)
        {
            var notification = new CustomMessageBox(message, duration);
            notification.Show();
        }
    }
}