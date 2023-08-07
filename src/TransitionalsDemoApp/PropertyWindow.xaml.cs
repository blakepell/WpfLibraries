/******************************************************************************
Copyright (c) Microsoft Corporation.  All rights reserved.


This file is licensed under the Microsoft Public License (Ms-PL). A copy of the Ms-PL should accompany this file. 
If it does not, you can obtain a copy from: 

http://www.microsoft.com/resources/sharedsource/licensingbasics/publiclicense.mspx
******************************************************************************/

using System.ComponentModel;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Threading;

namespace TransitionalsDemoApp
{
    /// <summary>
    /// Interaction logic for PropertyWindow.xaml
    /// </summary>
    public partial class PropertyWindow : System.Windows.Window
    {
        private object _selectedObject;

        public PropertyWindow()
        {
            this.InitializeComponent();
        }

        private object CreateFilterWrapper(object value)
        {
            // Don't wrap null values
            if (value == null)
            {
                return null;
            }

            // Use the filter
            return new TransitionFilterDescriptor(value, (TransitionFilterLevel)FilterCombo.SelectedIndex);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // Don't let close, hide instead
            e.Cancel = true;
            
            // hideTimer.Start();
            this.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new ThreadStart(this.Hide));
        }

        /// <summary>
        /// Gets or sets the selected object of the <see cref="PropertyWindow"/>.
        /// </summary>
        /// <value>
        /// The selected object of the <c>PropertyWindow</c>.
        /// </value>
        public object SelectedObject
        {
            get => _selectedObject;
            set
            {
                // Store in local variable
                _selectedObject = value;

                // Create the wrapper and assign it to the grid
                propertyGrid.SelectedObject = this.CreateFilterWrapper(value);
            }
        }

        private void FilterChanged(object sender, SelectionChangedEventArgs e)
        {
            // Recreate the wrapper
            if (propertyGrid != null)
            {
                propertyGrid.SelectedObject = this.CreateFilterWrapper(_selectedObject);
            }
        }
    }
}
