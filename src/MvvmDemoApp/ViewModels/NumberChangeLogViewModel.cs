using System.Collections.ObjectModel;
using MvvmFoundation.Wpf;

namespace MvvmDemoApp.ViewModels
{
    public class NumberChangeLogViewModel
    {
        public NumberChangeLogViewModel()
        {
            this.Number = new NumberViewModel();
            this.ChangeLog = new ObservableCollection<string>();

            _observer =
                new PropertyObserver<NumberViewModel>(this.Number)
                   .RegisterHandler(n => n.Value, n => this.Log($"Value: {n.Value}"))
                   .RegisterHandler(n => n.IsNegative, this.AppendIsNegative)
                   .RegisterHandler(n => n.IsEven, this.AppendIsEven);
        }

        private void AppendIsNegative(NumberViewModel number)
        {
            if (number.IsNegative)
            {
                this.Log("\tNumber is now negative");
            }
            else
            {
                this.Log("\tNumber is now positive");
            }
        }

        private void AppendIsEven(NumberViewModel number)
        {
            if (number.IsEven)
            {
                this.Log("\tNumber is now even");
            }
            else
            {
                this.Log("\tNumber is now odd");
            }
        }

        private void Log(string item)
        {
            this.ChangeLog.Add(item);
            App.Messenger.NotifyColleagues(App.MSG_LOG_APPENDED);
        }

        public ObservableCollection<string> ChangeLog { get; private set; }
        public NumberViewModel Number { get; private set; }

        readonly PropertyObserver<NumberViewModel> _observer;
    }
}