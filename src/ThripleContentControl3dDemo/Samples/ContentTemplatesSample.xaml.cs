namespace ThripleContentControl3dDemoApp.Samples
{
	public partial class ContentTemplatesSample : ISample
	{
		public ContentTemplatesSample()
		{
			InitializeComponent();
		}

		public string Description =>
            "This sample shows how to use data objects as the BackContent and Content " +
            "of ContentControl3D, and then provide DataTemplates to the BackContentTemplate " +
            "and ContentTemplate properties to specify how to display those data objects.";
    }
}