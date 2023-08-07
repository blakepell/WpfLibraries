using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TransitionalsBasicDemoApp
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();
            TransitionBox.Transition = new Transitionals.Transitions.FadeAndBlurTransition();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TransitionBox.Content is Ellipse el && el.Height == 300)
            {
                TransitionBox.Content = new Ellipse
                {
                    Fill = new SolidColorBrush(Colors.Gray),
                    Height = 200,
                    Width = 200
                };
            }
            else
            {
                TransitionBox.Content = new Ellipse
                {
                    Fill = new SolidColorBrush(Colors.BlueViolet),
                    Height = 300,
                    Width = 300
                };
            }
        }
    }
}
