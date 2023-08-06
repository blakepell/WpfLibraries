using MvvmFoundation.Wpf;

namespace MvvmDemoApp
{
    public partial class App
    {
        internal static Messenger Messenger => _messenger;

        private static readonly Messenger _messenger = new();

        internal const string MSG_LOG_APPENDED = "Log Appended";
    }
}
