using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VinylVerse.Controls;
using static VinylVerse.App;

namespace VinylVerse
{
    /// <summary>
    /// Логика взаимодействия для SignIn.xaml
    /// </summary>
    public partial class SignIn : Window
    {
        private string pattern = "^[a-zA-Z0-9\\s!@#\\$%\\^&\\*\\(\\)_\\+\\-=\\[\\]\\{\\};:'\",\\.<>\\?/\\\\|]+$";
        private bool isPasswordVisible = false;
        private string actualPassword = "";
        private bool isUpdatingText = false;

        public SignIn()
        {
            InitializeComponent();
        }

        private void username_textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, pattern);
        }

        private void password_textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, pattern);
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            string identifier = username_textbox.Text.Trim();
            string password = password_textbox.Text.Trim();

            if (string.IsNullOrEmpty(identifier) || string.IsNullOrEmpty(password))
            {
                CustomMessageBox.ShowNotification("Please fill in all fields.", 1500);
                return;
            }

            DatabaseHelper db = new DatabaseHelper();
            User user = db.LoginUser(identifier, password);

            if (user != null)
            {
                // Вход успешен
                CustomMessageBox.ShowNotification($"Welcome, {user.Name}!", 1500);

                // Логика после входа, например, переход на главное окно
                //MainWindow mainWindow = new MainWindow(user);
                //mainWindow.Show();
                //this.Close();
            }
            else
            {
                CustomMessageBox.ShowNotification("Invalid email/username or password.", 1500);
            }
        }
        private void SignUpButton_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            SignUp signUnWindow = new SignUp();
            Animator.AnimateWindowTransition(this, signUnWindow);
        }

        private void username_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(username_textbox.Text))
            {
                if (username_accept_circle.Visibility != System.Windows.Visibility.Visible)
                {
                    Animator.AnimateVisibility(username_deny_circle, false);
                    Animator.AnimateVisibility(username_accept_circle, true);

                    Animator.AnimateColorChange(
                        username_underline,
                        VinylVerse.Controls.Underline.LineColorProperty,
                        "#D7D7D7",
                        "#55D78B",
                        300);
                }
            }
            else
            {
                if (username_deny_circle.Visibility != System.Windows.Visibility.Visible)
                {
                    Animator.AnimateVisibility(username_deny_circle, true);
                    Animator.AnimateVisibility(username_accept_circle, false);

                    Animator.AnimateColorChange(
                        username_underline,
                        VinylVerse.Controls.Underline.LineColorProperty,
                        "#55D78B",
                        "#D7D7D7",
                        300);
                }
            }
        }

        private void password_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isUpdatingText) return;

            string newInput = password_textbox.Text;

            if (isPasswordVisible)
            {
                actualPassword = newInput;
            }
            else
            {
                if (newInput.Length > actualPassword.Length)
                {
                    string addedText = newInput.Replace("●", "");
                    actualPassword += addedText;
                }
                else if (newInput.Length < actualPassword.Length)
                {
                    int charsToRemove = actualPassword.Length - newInput.Length;
                    actualPassword = actualPassword.Substring(0, actualPassword.Length - charsToRemove);
                }

                isUpdatingText = true;
                password_textbox.Text = new string('●', actualPassword.Length);
                password_textbox.SelectionStart = password_textbox.Text.Length;
                isUpdatingText = false;
            }

            if (!string.IsNullOrEmpty(password_textbox.Text))
            {
                Animator.AnimateColorChange(
                        password_underline,
                        VinylVerse.Controls.Underline.LineColorProperty,
                        "#D7D7D7",
                        "#55D78B",
                        300);
            } else { 
                Animator.AnimateColorChange(
                        password_underline,
                        VinylVerse.Controls.Underline.LineColorProperty,
                        "#55D78B",
                        "#D7D7D7",
                        300);
            }
        }

        private void password_eye_closed_icon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Animator.AnimateVisibility(password_eye_closed_icon, false);
            Animator.AnimateVisibility(password_eye_icon, true);

            isPasswordVisible = true;

            isUpdatingText = true;
            password_textbox.Text = actualPassword;
            password_textbox.SelectionStart = password_textbox.Text.Length;
            isUpdatingText = false;
        }

        private void password_eye_icon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Animator.AnimateVisibility(password_eye_icon, false);
            Animator.AnimateVisibility(password_eye_closed_icon, true);

            isPasswordVisible = false;

            isUpdatingText = true;
            password_textbox.Text = new string('●', actualPassword.Length);
            password_textbox.SelectionStart = password_textbox.Text.Length;
            isUpdatingText = false;
        }
    }
}
