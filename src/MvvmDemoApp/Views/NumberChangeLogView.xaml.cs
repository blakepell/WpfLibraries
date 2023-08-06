using System;
using System.Windows.Controls;

namespace MvvmDemoApp.Views
{
    public partial class NumberChangeLogView : UserControl
    {
        public NumberChangeLogView()
        {
            this.InitializeComponent();

            App.Messenger.Register(
                App.MSG_LOG_APPENDED,
                new Action(() => _scrollViewer.ScrollToBottom()));
        }
    }
}