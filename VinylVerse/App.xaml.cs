using SkiaSharp;
using Svg.Skia;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace VinylVerse
{
    public partial class App : Application
    {
        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public int UserRank { get; set; }
            public string ProfilePicture { get; set; }
            public string PaymentMethod { get; set; }
            public string Language { get; set; }
            public DateTime RegistrationDate { get; set; }
        }
    }
}
