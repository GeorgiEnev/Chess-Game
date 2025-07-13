using System.Configuration;
using System.Data;
using System.Windows;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.Resources = new ResourceDictionary
            {
                {
                    "ShadowEffect",
                    new System.Windows.Media.Effects.DropShadowEffect
                    {
                        BlurRadius = 15,
                        ShadowDepth = 5,
                        Color = System.Windows.Media.Colors.Black,
                        Opacity = 0.5
                    }
                }
            };
        }
    }
}
