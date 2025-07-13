using ChessUI;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace YourNamespace
{
    public partial class StartGameWindow : Window
    {
        public StartGameWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Animate icon and title fade/slide in
            var fadeInIcon = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(500));
            var slideIcon = new DoubleAnimation(40, 0, TimeSpan.FromMilliseconds(500));
            IconText.BeginAnimation(OpacityProperty, fadeInIcon);
            ((TranslateTransform)IconText.RenderTransform).BeginAnimation(TranslateTransform.YProperty, slideIcon);

            var fadeInTitle = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(500)) { BeginTime = TimeSpan.FromMilliseconds(150) };
            var slideTitle = new DoubleAnimation(40, 0, TimeSpan.FromMilliseconds(500)) { BeginTime = TimeSpan.FromMilliseconds(150) };
            TitleText.BeginAnimation(OpacityProperty, fadeInTitle);
            ((TranslateTransform)TitleText.RenderTransform).BeginAnimation(TranslateTransform.YProperty, slideTitle);

            // Animate button pop-in
            var fadeInBtn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(400)) { BeginTime = TimeSpan.FromMilliseconds(350) };
            var scaleUpX = new DoubleAnimation(0.8, 1, TimeSpan.FromMilliseconds(400)) { BeginTime = TimeSpan.FromMilliseconds(350) };
            var scaleUpY = new DoubleAnimation(0.8, 1, TimeSpan.FromMilliseconds(400)) { BeginTime = TimeSpan.FromMilliseconds(350) };
            StartGameButton.BeginAnimation(OpacityProperty, fadeInBtn);
            ((ScaleTransform)StartGameButton.RenderTransform).BeginAnimation(ScaleTransform.ScaleXProperty, scaleUpX);
            ((ScaleTransform)StartGameButton.RenderTransform).BeginAnimation(ScaleTransform.ScaleYProperty, scaleUpY);
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            // Fade out animation
            var fadeOut = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromMilliseconds(600)));
            fadeOut.Completed += (s, _) =>
            {
                // Show the chess board window after fade out
                var boardWindow = new MainWindow();
                boardWindow.Show();
                this.Close();
            };
            this.BeginAnimation(Window.OpacityProperty, fadeOut);
        }
    }
}