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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VinylVerse.Controls
{
    public partial class Underline : UserControl
    {
        public Underline()
        {
            InitializeComponent();
        }

        public Brush LineColor
        {
            get => (Brush)GetValue(LineColorProperty);
            set => SetValue(LineColorProperty, value);
        }

        public static readonly DependencyProperty LineColorProperty =
            DependencyProperty.Register(nameof(LineColor), typeof(Brush), typeof(Underline),
                new PropertyMetadata(Brushes.Gray, OnLineColorChanged));

        private static void OnLineColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Underline underline)
            {
                underline.UnderlineBorder.Background = (Brush)e.NewValue;
            }
        }

        public double LineThickness
        {
            get => (double)GetValue(LineThicknessProperty);
            set => SetValue(LineThicknessProperty, value);
        }

        public static readonly DependencyProperty LineThicknessProperty =
            DependencyProperty.Register(nameof(LineThickness), typeof(double), typeof(Underline),
                new PropertyMetadata(1.0, OnLineThicknessChanged));

        private static void OnLineThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Underline underline)
            {
                underline.UnderlineBorder.Height = (double)e.NewValue;
            }
        }
    }
}