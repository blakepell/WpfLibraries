using System.Windows.Input;
using MvvmFoundation.Wpf;

namespace MvvmDemoApp.ViewModels
{
    public class NumberViewModel : ObservableObject
    {
        public bool IsEven => this.Value % 2 == 0;

        public bool IsNegative => this.Value < 0;

        public int Value
        {
            get => _value;
            set
            {
                if (value == _value)
                {
                    return;
                }

                bool wasEven = this.IsEven;
                bool wasNegative = this.IsNegative;

                _value = value;

                this.RaisePropertyChanged(nameof(this.Value));

                if (wasEven != this.IsEven)
                {
                    this.RaisePropertyChanged(nameof(this.IsEven));
                }

                if (wasNegative != this.IsNegative)
                {
                    this.RaisePropertyChanged(nameof(this.IsNegative));
                }
            }
        }

        public ICommand DecrementCommand
        {
            get
            {
                return _decrementCommand ??= new RelayCommand(() => --this.Value);
            }
        }

        public ICommand IncrementCommand
        {
            get
            {
                return _incrementCommand ??= new RelayCommand(() => ++this.Value);
            }
        }

        private RelayCommand? _decrementCommand;

        private RelayCommand? _incrementCommand;

        private int _value;
    }
}