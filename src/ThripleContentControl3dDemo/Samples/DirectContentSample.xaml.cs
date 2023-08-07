namespace ThripleContentControl3dDemoApp.Samples
{
	public partial class DirectContentSample : ISample
	{
		public DirectContentSample()
		{
			InitializeComponent();
		}

		public string Description =>
            "This sample shows how to add visual elements directly to both sides of ContentControl3D," +
            "via the Content and BackContent properties.";
    }
}