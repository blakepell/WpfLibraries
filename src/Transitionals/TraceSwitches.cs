/******************************************************************************
Copyright (c) Microsoft Corporation.  All rights reserved.


This file is licensed under the Microsoft Public License (Ms-PL). A copy of the Ms-PL should accompany this file. 
If it does not, you can obtain a copy from: 

http://www.microsoft.com/resources/sharedsource/licensingbasics/publiclicense.mspx
******************************************************************************/

using System.Diagnostics;

namespace Transitionals
{
    /// <summary>
    /// Provides trace level switches for various components of the framework.
    /// </summary>
    internal static class TraceSwitches
    {
        private static TraceSwitch? _transitionsSw;

        /// <summary>
        /// Defines a trace switch for the transitions themselves.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static TraceSwitch? Transitions => _transitionsSw ?? (_transitionsSw = new TraceSwitch("Transitions", "Transition operations trace switch"));
    }
}
