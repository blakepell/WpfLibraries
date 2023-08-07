/******************************************************************************
Copyright (c) Microsoft Corporation.  All rights reserved.


This file is licensed under the Microsoft Public License (Ms-PL). A copy of the Ms-PL should accompany this file. 
If it does not, you can obtain a copy from: 

http://www.microsoft.com/resources/sharedsource/licensingbasics/publiclicense.mspx
******************************************************************************/

using System.Windows.Input;

namespace TransitionalsDemoApp
{
    /// <summary>
    /// Commands for the TransitionTester application not provided by the framework.
    /// </summary>
    static public class TransitionTesterCommands
    {
        private static RoutedUICommand _about = null;
        private static RoutedUICommand _exit = null;

        /// <summary>
        /// Provides a routed command for application about.
        /// </summary>
        public static RoutedUICommand About
        {
            get
            {
                if (_about == null)
                {
                    _about = new RoutedUICommand("About", "About", typeof(TransitionTesterCommands));
                }

                return _about;
            }
        }

        /// <summary>
        /// Provides a routed command for application exit.
        /// </summary>
        public static RoutedUICommand Exit
        {
            get
            {
                if (_exit == null)
                {
                    _exit = new RoutedUICommand("Exit", "Exit", typeof(TransitionTesterCommands));
                    _exit.InputGestures.Add(new KeyGesture(Key.F4, ModifierKeys.Alt));
                }

                return _exit;
            }
        }
    }
}
