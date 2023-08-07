using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace ThripleContentControl3dDemoApp.Samples
{
	public partial class BindingToViewModelSample : UserControl, ISample
	{
		public BindingToViewModelSample()
		{
			InitializeComponent();

			base.DataContext = new PersonViewModel(
				"Josh",
				"Smith",
				"Images/JoshSmith.jpg",
				28,
				"Johann Sebastian Bach");
		}

		public string Description =>
            "This sample shows how to bind both sides of ContentControl3D to a ViewModel object. " +
            "It disables the rotation button by binding the CanRotate property to the PersonViewModel's " +
            "IsValid property.  The back side of the surface is an edit form, with input validation.";
    }

	public class PersonViewModel : INotifyPropertyChanged, IDataErrorInfo
	{
        string _age;
		string _favoriteComposer;
		string _firstName;
		bool _isValidating;
		string _lastName;

		static readonly string[] ValidatedProperties = new string[] 
		{
			"Age",
			"FirstName", 
			"LastName"			
		};

        public PersonViewModel(string firstName, string lastName, string photoPath, int age, string favoriteComposer)
		{
			this.FirstName = firstName;
			this.LastName = lastName;
			this.PhotoUri = new Uri(photoPath, UriKind.Relative);
			this.Age = age.ToString();
			this.FavoriteComposer = favoriteComposer;
		}

        public string Age
		{
			get => _age;
            set
			{
				if (value == _age)
                {
                    return;
                }

                _age = value;

				this.OnPropertyChanged(nameof(Age));
			}
		}

		public string FavoriteComposer
		{
			get => _favoriteComposer;
            set
			{
				if (value == _favoriteComposer)
                {
                    return;
                }

                _favoriteComposer = value;

				this.OnPropertyChanged(nameof(FavoriteComposer));
			}
		}

		public string FirstName
		{
			get => _firstName;
            set
			{
				if (value == _firstName)
                {
                    return;
                }

                _firstName = value;

				this.OnPropertyChanged(nameof(FirstName));
				this.OnPropertyChanged(nameof(FullName));
			}
		}

		public bool IsValid
		{
			get
			{
				try
				{
					_isValidating = true;

					foreach (string propertyName in ValidatedProperties)
                    {
                        if ((this as IDataErrorInfo)[propertyName] != null)
                        {
                            return false;
                        }
                    }
                }
				finally
				{
					_isValidating = false;
				}

				return true;
			}
		}

		public string LastName
		{
			get => _lastName;
            set
			{
				if (value == _lastName)
                {
                    return;
                }

                _lastName = value;

				this.OnPropertyChanged(nameof(LastName));
				this.OnPropertyChanged(nameof(FullName));
			}
		}

		public string FullName => $"{this.LastName}, {this.FirstName}";

        public Uri PhotoUri { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

		void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler handler = this.PropertyChanged;
			if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string Error => null;

        public string this[string propertyName]
		{
			get
			{
				string error = null;
				switch (propertyName)
				{
					case "FirstName":
						if (this.FirstName == null || this.FirstName.Trim().Length == 0)
                        {
                            error = "First name is missing.";
                        }

                        break;

					case "LastName":
						if (this.LastName == null || this.LastName.Trim().Length == 0)
                        {
                            error = "Last name is missing.";
                        }

                        break;

					case "Age":
						int age;
						if (int.TryParse(this.Age, out age))
						{
							if (age < 0 || 120 < age)
                            {
                                error = "Age must be between 0 and 120.";
                            }
                        }
						else
						{
							error = "Age must be a number.";
						}
						break;
				}

				if (!_isValidating)
                {
                    this.OnPropertyChanged(nameof(IsValid));
                }

                return error;
			}
		}
    }
}