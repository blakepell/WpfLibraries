/******************************************************************************
Copyright (c) Microsoft Corporation.  All rights reserved.


This file is licensed under the Microsoft Public License (Ms-PL). A copy of the Ms-PL should accompany this file. 
If it does not, you can obtain a copy from: 

http://www.microsoft.com/resources/sharedsource/licensingbasics/publiclicense.mspx
******************************************************************************/

using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using Transitionals;

namespace TransitionalsDemoApp
{
	public partial class App: System.Windows.Application
	{
        /// <summary>
        /// Gets the typed instance of the current application.
        /// </summary>
        static public App CurrentApp => (App)Current;

        /************************************************
		 * Member Variables
		 ***********************************************/

        /************************************************
		 * Overrides / Event Handlers
		 ***********************************************/
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            // Can't assign the owner to the property window unless it has been created.
            if (this.PropertyWindow.Owner == null)
            {
                this.PropertyWindow.Owner = this.MainWindow;
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Load transitions
            this.LoadStockTransitions();
        }

        /************************************************
		 * Internal Methods
		 ***********************************************/
        /// <summary>
        /// Loads the transitions that are part of the framework.
        /// </summary>
        private void LoadStockTransitions()
        {
            this.LoadTransitions(Assembly.GetAssembly(typeof(Transition)));
        }

        /************************************************
		 * Public Methods
		 ***********************************************/
        /// <summary>
        /// Loads the transitions found in the specified assembly.
        /// </summary>
        /// <param name="assembly">
        /// The assembly to search for transitions in.
        /// </param>
        public void LoadTransitions(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                // Must not already exist
                if (this.TransitionTypes.Contains(type)) { continue; }

                // Must not be abstract.
                if ((typeof(Transition).IsAssignableFrom(type)) && (!type.IsAbstract))
                {
                    this.TransitionTypes.Add(type);
                }
            }
        }

        /// <summary>
        /// Loads the transitions found in the assembly at the specified path.
        /// </summary>
        /// <param name="assemblyPath">
        /// The path to the assembly to search for transitions in.
        /// </param>
        public void LoadTransitions(string assemblyPath)
        {
            // Load the assembly
            var assembly = Assembly.LoadFrom(assemblyPath);

            // Load transitions from the assembly
            this.LoadTransitions(assembly);
        }

        /// <summary>
        /// Toggles the visibility of the property window.
        /// </summary>
        public void TogglePropertyWindow()
        {
            if (this.PropertyWindow.IsVisible)
            {
                this.PropertyWindow.Hide();
            }
            else
            {
                this.PropertyWindow.Show();
            }
        }

        /************************************************
		 * Public Properties
		 ***********************************************/
        /// <summary>
        /// Gets the <see cref="PropertyWindow"/> used by the application.
        /// </summary>
        /// <value>
        /// The <see cref="PropertyWindow"/> used by the application.
        /// </value>
        public PropertyWindow PropertyWindow { get; } = new PropertyWindow();

        /// <summary>
        /// Gets the list of known transition types.
        /// </summary>
        /// <value>
        /// The list of known transition types.
        /// </value>
        public ObservableCollection<Type> TransitionTypes { get; } = new ObservableCollection<Type>();
    }
}
