/******************************************************************************
Copyright (c) Microsoft Corporation.  All rights reserved.


This file is licensed under the Microsoft Public License (Ms-PL). A copy of the Ms-PL should accompany this file. 
If it does not, you can obtain a copy from: 

http://www.microsoft.com/resources/sharedsource/licensingbasics/publiclicense.mspx
******************************************************************************/

using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Animation;

namespace Transitionals.Transitions
{
    /// <summary>
    ///     Stores the XAML that defines the DiagonalWipeTransition
    /// </summary>
    [ComVisible(false)]
    public partial class DiagonalWipeTransitionFrameworkElement
    {
        public DiagonalWipeTransitionFrameworkElement()
        {
            this.InitializeComponent();
        }
    }

    /// <summary>
    ///     Represents the DiagonalWipeTransition
    /// </summary>
    [ComVisible(false)]
    public class DiagonalWipeTransition : StoryboardTransition
    {
        private static DiagonalWipeTransitionFrameworkElement _frameworkElement = new();

        public DiagonalWipeTransition()
        {
            this.NewContentStyle = (Style)_frameworkElement.FindResource("DiagonalWipeTransitionNewContentStyle");
            this.NewContentStoryboard = (Storyboard)_frameworkElement.FindResource("DiagonalWipeTransitionNewContentStoryboard");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.NotSupportedException.#ctor(System.String)")]
        protected override void OnDurationChanged(Duration oldDuration, Duration newDuration)
        {
            throw new System.NotSupportedException("CTP1 does not support changing the duration of this transition");
        }
    }
}
