using System;
using Thriple.Controls;

namespace ThripleContentControl3dDemoApp.Samples
{
	public partial class PropertyExplorerSample : ISample
	{
		public PropertyExplorerSample()
		{
			InitializeComponent();
			this.rotationDirectionSelector.ItemsSource = Enum.GetValues(typeof(RotationDirection));
			this.easingModeSelector.ItemsSource = Enum.GetValues(typeof(RotationEasingMode));
		}

		public string Description =>
            "This sample allows you to experiment with several properties of ContentControl3D " +
            "to see how the various values can work together to create different visual effects.";
    }
}