/******************************************************************************
Copyright (c) Microsoft Corporation.  All rights reserved.


This file is licensed under the Microsoft Public License (Ms-PL). A copy of the Ms-PL should accompany this file. 
If it does not, you can obtain a copy from: 

http://www.microsoft.com/resources/sharedsource/licensingbasics/publiclicense.mspx
******************************************************************************/

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Win32;
using Transitionals;

namespace TransitionalsDemoApp
{
	public partial class TransitionWindow
	{
        /************************************************
		 * Constants
		 ***********************************************/
        private const string CellA = "CellA";
        private const string CellB = "CellB";

        /************************************************
		 * Member Variables
		 ***********************************************/
        private ICollectionView _view;

        /************************************************
		 * Constructors
		 ***********************************************/
        /// <summary>
        /// Initializes a new <see cref="TransitionWindow"/>.
        /// </summary>
        public TransitionWindow()
        {
            this.InitializeComponent();

            // Get the default view for the transition types
            _view = CollectionViewSource.GetDefaultView(App.CurrentApp.TransitionTypes);
            
            // Set the default sort
            _view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            
            // Handle changes in currency
            _view.CurrentChanged += new EventHandler(this.view_CurrentChanged);

            // Bind
            this.TransitionDs.ObjectType = null;
            this.TransitionTypesDs.ObjectType = null;
            this.TransitionTypesDs.ObjectInstance = App.CurrentApp.TransitionTypes;

            // Navigate to first item
            _view.MoveCurrentToFirst();
        }

        /************************************************
		 * Internal Methods
		 ***********************************************/
        /// <summary>
        /// Activates a transition and displays it.
        /// </summary>
        /// <param name="transitionType">
        /// The type of transition to activate.
        /// </param>
        private void ActivateTransition(Type transitionType)
        {
            // If no type, ignore
            if (transitionType == null)
            {
                return;
            }

            // Create the instance
            var transition = (Transition)Activator.CreateInstance(transitionType);

            // Bind
            this.TransitionDs.ObjectInstance = transition;
            App.CurrentApp.PropertyWindow.SelectedObject = transition;

            // Swap cells to show transition
            this.SwapCell();
        }

        /// <summary>
        /// Loads transitions by allowing the user to browse for a transition assembly.
        /// </summary>
        private void BrowseLoadTransitions()
        {
            // Create the browser
            var ofd = new OpenFileDialog
            {
                Filter = "Assemblies (*.dll, *.exe)|*.dll;*.exe|All files (*.*)|*.*",
                Multiselect = true
            };

            // Show the browse and if successful, try to load.
            if (ofd.ShowDialog(this) == true)
            {
                // Try to load each selected file
                foreach (string path in ofd.FileNames)
                {
                    try
                    {
                        App.CurrentApp.LoadTransitions(path);
                    }
                    catch (Exception ex)
                    {
                        // Build the message
                        string msg = string.Format("Error loading transitions:\r\n\r\n{0}\r\n\r\nContinue?", ex.Message);

                        // Show message and ask to continue
                        var result = MessageBox.Show(msg, "Error", MessageBoxButton.YesNo, MessageBoxImage.Error);

                        // If we shouldn't continue, break out of loop
                        if (result != MessageBoxResult.Yes) { break; }
                    }
                }
            }
        }

        /// <summary>
        /// Creates an animation cell for demonstrating a transition.
        /// </summary>
        /// <param name="style">
        /// The style used to create the cell.
        /// </param>
        /// <returns>
        /// A <see cref="ContentControl"/> that represents the cell.
        /// </returns>
        private ContentControl CreateCell(Style style)
        {
            var c = new ContentControl
            {
                Style = style
            };
            return c;
        }

        /// <summary>
        /// Displays the About dialog.
        /// </summary>
        private void ShowAbout()
        {
            var about = new AboutWindow
            {
                Owner = this
            };

            about.ShowDialog();
        }

        /// <summary>
        /// Swaps the current cell, from A to B or from B to A.
        /// </summary>
        private void SwapCell()
        {
            var currentCell = (ContentControl)TransitionBox.Content;

            if ((currentCell == null) || (currentCell.Style == this.Resources[CellB]))
            {
                TransitionBox.Content = this.CreateCell((Style)this.Resources[CellA]);
            }
            else
            {
                TransitionBox.Content = this.CreateCell((Style)this.Resources[CellB]);
            }
        }

        /************************************************
		 * Overrides / Event Handlers
		 ***********************************************/
        private void About_Executed(object sender, RoutedEventArgs e)
        {
            this.ShowAbout();    
        }

        private void ABButton_Click(object sender, RoutedEventArgs e)
        {
            this.SwapCell();
        }

        private void AButton_Click(object sender, RoutedEventArgs e)
        {
            TransitionBox.Content = this.CreateCell((Style)this.Resources[CellA]);
        }

        private void BButton_Click(object sender, RoutedEventArgs e)
        {
            TransitionBox.Content = this.CreateCell((Style)this.Resources[CellB]);
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            TransitionBox.Content = null;
        }

        private void Exit_Executed(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Open_Executed(object sender, RoutedEventArgs e)
        {
            this.BrowseLoadTransitions();
        }

        private void PropertyButton_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentApp.TogglePropertyWindow();
        }

        private void view_CurrentChanged(object sender, EventArgs e)
        {
            this.ActivateTransition((Type)_view.CurrentItem);
        }

        /************************************************
		 * Internal Properties
		 ***********************************************/
        private ObjectDataProvider TransitionDs => (ObjectDataProvider)this.Resources["TransitionDS"];

        private ObjectDataProvider TransitionTypesDs => (ObjectDataProvider)this.Resources["TransitionTypesDS"];
    }
}