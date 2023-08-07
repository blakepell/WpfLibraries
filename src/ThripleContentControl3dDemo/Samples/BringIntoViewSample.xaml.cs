using System.Windows;
using System.Windows.Controls;

namespace ThripleContentControl3dDemoApp.Samples
{
	public partial class BringIntoViewSample : ISample
	{
		public BringIntoViewSample()
		{
			InitializeComponent();
		}

		private void OnBringFrontSideIntoView(object sender, RoutedEventArgs e)
		{
			// The front side TextBlock is in a DataTemplate, so we must use FindName() to locate it.
			ContentPresenter? frontContentPresenter =
				this.cntCtrl3D.Template.FindName("PART_FrontContentPresenter", this.cntCtrl3D)
				as ContentPresenter;

			TextBlock? frontSideTextBlock =
				frontContentPresenter?.ContentTemplate.FindName("frontSideTextBlock", frontContentPresenter)
				as TextBlock;

			frontSideTextBlock?.BringIntoView();
		}

		private void OnBringBackSideIntoView(object sender, RoutedEventArgs e)
		{
			this.backSideTextBlock.BringIntoView();
		}

		public string Description =>
            "This sample shows that calling BringIntoView on an element contained within the surface " +
            "causes the ContentControl3D to rotate, if that element is not currently on the side in view.";
    }
}