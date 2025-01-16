using ExCSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VinylVerse.Behaviors;
using System.Windows.Media.Animation;
using System.Drawing;

namespace VinylVerse
{
    /// <summary>
    /// Логика взаимодействия для SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        private bool isPasswordVisible = false;
        private string actualPassword = "";
        private bool isUpdatingText = false;

        private bool isRPasswordVisible = false;
        private string actualRPassword = "";
        private bool isUpdatingRText = false;
        public SignUp()
        {
            InitializeComponent();
        }
        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Sign Up button clicked!");
        }

        private void SignInButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var signInWindow = Application.Current.Windows.OfType<SignIn>().FirstOrDefault();
            Animator.AnimateWindowTransition(this, Application.Current.Windows.);
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

        private void email_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(email_textbox.Text))
            {
                if (email_accept_circle.Visibility != System.Windows.Visibility.Visible)
                {
                    Animator.AnimateVisibility(email_deny_circle, false);
                    Animator.AnimateVisibility(email_accept_circle, true);

                    Animator.AnimateColorChange(
                       email_underline,
                       VinylVerse.Controls.Underline.LineColorProperty,
                       "#D7D7D7",
                       "#55D78B",
                       300);
                }
            }
            else
            {
                if (email_deny_circle.Visibility != System.Windows.Visibility.Visible)
                {
                    Animator.AnimateVisibility(email_deny_circle, true);
                    Animator.AnimateVisibility(email_accept_circle, false);

                    Animator.AnimateColorChange(
                       email_underline,
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

            if (password_textbox.Text.Length >= 8 && (password_unchecked_characters.Visibility == System.Windows.Visibility.Visible))
            {
                Animator.AnimateVisibility(password_checked_characters, true);
                Animator.AnimateVisibility(password_unchecked_characters, false);

                Animator.AnimateColorChange(
                    password_check_characters_lable,
                    Label.ForegroundProperty,
                    "#D7D7D7",
                    "#55D78B",
                    300);
            }
            else if ((password_textbox.Text.Length < 8 || string.IsNullOrEmpty(password_textbox.Text)) && (password_unchecked_characters.Visibility != System.Windows.Visibility.Visible))
            {
                Animator.AnimateVisibility(password_checked_characters, false);
                Animator.AnimateVisibility(password_unchecked_characters, true);

                Animator.AnimateColorChange(
                    password_check_characters_lable,
                    Label.ForegroundProperty,
                    "#55D78B",
                    "#D7D7D7",
                    300);
            }

            bool containsDigitOrSpecial = actualPassword.Any(c =>
                char.IsDigit(c) || "!@#$%^&*()_+-=[]{}|;:'\",.<>?/`~".Contains(c));

            if (containsDigitOrSpecial && password_unchecked_numbers.Visibility == System.Windows.Visibility.Visible)
            {
                Animator.AnimateVisibility(password_checked_numbers, true);
                Animator.AnimateVisibility(password_unchecked_numbers, false);

                Animator.AnimateColorChange(
                    password_check_numbers_lable,
                    Label.ForegroundProperty,
                    "#D7D7D7",
                    "#55D78B",
                    300);
            }
            else if (!containsDigitOrSpecial && password_checked_numbers.Visibility == System.Windows.Visibility.Visible)
            {
                Animator.AnimateVisibility(password_checked_numbers, false);
                Animator.AnimateVisibility(password_unchecked_numbers, true);

                Animator.AnimateColorChange(
                    password_check_numbers_lable,
                    Label.ForegroundProperty,
                    "#55D78B",
                    "#D7D7D7",
                    300);
            }

            bool containsUpperOrLower = (actualPassword.Any(char.IsUpper) && actualPassword.Any(char.IsLower));

            if (containsUpperOrLower && password_unchecked_case.Visibility == System.Windows.Visibility.Visible)
            {
                Animator.AnimateVisibility(password_checked_case, true);
                Animator.AnimateVisibility(password_unchecked_case, false);

                Animator.AnimateColorChange(
                    password_check_case_lable,
                    Label.ForegroundProperty,
                    "#D7D7D7",
                    "#55D78B",
                    300);
            }
            else if (!containsUpperOrLower && password_checked_case.Visibility == System.Windows.Visibility.Visible)
            {
                Animator.AnimateVisibility(password_checked_case, false);
                Animator.AnimateVisibility(password_unchecked_case, true);

                Animator.AnimateColorChange(
                    password_check_case_lable,
                    Label.ForegroundProperty,
                    "#55D78B",
                    "#D7D7D7",
                    300);
            }

            UpdatePasswordStrengthAnimation(actualPassword);
        }

        private int GetPasswordStrength(string password)
        {
            int strength = 0;

            if (password.Length >= 8) strength++;
            if (password.Any(c => char.IsDigit(c) || char.IsPunctuation(c))) strength++;
            if (password.Any(char.IsUpper) && password.Any(char.IsLower)) strength++;

            return strength;
        }

        private void UpdatePasswordStrengthAnimation(string password)
        {
            int strength = GetPasswordStrength(password);

            switch (strength)
            {
                case 0:
                    p1.Visibility = System.Windows.Visibility.Hidden;
                    p2.Visibility = System.Windows.Visibility.Hidden;
                    p3.Visibility = System.Windows.Visibility.Hidden;
                    break;
                case 1:
                    if (p1.Visibility != System.Windows.Visibility.Visible)
                    {
                        Animator.AnimateVisibility(p1, true);
                        p2.Visibility = System.Windows.Visibility.Hidden;
                        p3.Visibility = System.Windows.Visibility.Hidden;
                    }
                    break;
                case 2:
                    if (p2.Visibility != System.Windows.Visibility.Visible)
                    {
                        Animator.AnimateVisibility(p2, true);
                        p1.Visibility = System.Windows.Visibility.Hidden;
                        p3.Visibility = System.Windows.Visibility.Hidden;
                    }
                    break;
                case 3:
                    if (p3.Visibility != System.Windows.Visibility.Visible)
                    {
                        Animator.AnimateVisibility(p3, true);
                        p1.Visibility = System.Windows.Visibility.Hidden;
                        p2.Visibility = System.Windows.Visibility.Hidden;
                    }
                    break;
                default:
                    p1.Visibility = System.Windows.Visibility.Hidden;
                    p2.Visibility = System.Windows.Visibility.Hidden;
                    p3.Visibility = System.Windows.Visibility.Hidden;
                    break;
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

        private void r_password_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isUpdatingRText) return;

            string newInput = r_password_textbox.Text;

            if (isRPasswordVisible)
            {
                actualRPassword = newInput;
            }
            else
            {
                if (newInput.Length > actualRPassword.Length)
                {
                    string addedText = newInput.Replace("●", "");
                    actualRPassword += addedText;
                }
                else if (newInput.Length < actualRPassword.Length)
                {
                    int charsToRemove = actualRPassword.Length - newInput.Length;
                    actualRPassword = actualRPassword.Substring(0, actualRPassword.Length - charsToRemove);
                }

                isUpdatingRText = true;
                r_password_textbox.Text = new string('●', actualRPassword.Length);
                r_password_textbox.SelectionStart = r_password_textbox.Text.Length;
                isUpdatingRText = false;
            }

            if (actualPassword == actualRPassword && (!string.IsNullOrEmpty(r_password_textbox.Text) || !string.IsNullOrEmpty(password_textbox.Text)))
            {
                Animator.AnimateColorChange(
                        r_password_underline,
                        VinylVerse.Controls.Underline.LineColorProperty,
                        "#D7D7D7",
                        "#55D78B",
                        300);
            } else if (actualPassword != actualRPassword && (!string.IsNullOrEmpty(r_password_textbox.Text) || !string.IsNullOrEmpty(password_textbox.Text)))
            {
                Animator.AnimateColorChange(
                        r_password_underline,
                        VinylVerse.Controls.Underline.LineColorProperty,
                        "#D7D7D7",
                        "#eb4d4b",
                        300);
            } else if (string.IsNullOrEmpty(r_password_textbox.Text)){
                Animator.AnimateColorChange(
                        r_password_underline,
                        VinylVerse.Controls.Underline.LineColorProperty,
                        "#55D78B",
                        "#D7D7D7",
                        300);
            }
        }

        private void r_eye_closed_icon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Animator.AnimateVisibility(r_eye_closed_icon, false);
            Animator.AnimateVisibility(r_eye_icon, true);

            isRPasswordVisible = true;

            isUpdatingRText = true;
            r_password_textbox.Text = actualRPassword;
            r_password_textbox.SelectionStart = r_password_textbox.Text.Length;
            isUpdatingRText = false;
        }

        private void r_eye_icon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Animator.AnimateVisibility(r_eye_icon, false);
            Animator.AnimateVisibility(r_eye_closed_icon, true);

            isRPasswordVisible = false;

            isUpdatingRText = true;
            r_password_textbox.Text = new string('●', actualRPassword.Length);
            r_password_textbox.SelectionStart = r_password_textbox.Text.Length;
            isUpdatingRText = false;
        }
    }
}
