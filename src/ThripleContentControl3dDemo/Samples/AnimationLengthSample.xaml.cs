namespace ThripleContentControl3dDemoApp.Samples
{
	public partial class AnimationLengthSample : ISample
	{
		public AnimationLengthSample()
		{
			InitializeComponent();
		}

		public string Description =>
            "This sample allows you to experiment with various values for the AnimationLength " +
            "property, which represents the duration of an animated rotation, in milliseconds.";
    }
}