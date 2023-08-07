/******************************************************************************
Copyright (c) Microsoft Corporation.  All rights reserved.


This file is licensed under the Microsoft Public License (Ms-PL). A copy of the Ms-PL should accompany this file. 
If it does not, you can obtain a copy from: 

http://www.microsoft.com/resources/sharedsource/licensingbasics/publiclicense.mspx
******************************************************************************/

using System.Windows;
using System.Windows.Media.Animation;
using System.Runtime.InteropServices;

namespace Transitionals.Transitions
{
    /// <summary>
    ///     Stores the XAML that defines the CheckerboardTransition
    /// </summary>
    [ComVisible(false)]
    public partial class CheckerboardTransitionFrameworkElement
    {
        public CheckerboardTransitionFrameworkElement()
        {
            this.InitializeComponent();
        }
    }

    /// <summary>
    ///     Represents the CheckerboardTransition
    /// </summary>
    [ComVisible(false)]
    public class CheckerboardTransition : StoryboardTransition
    {
        private static CheckerboardTransitionFrameworkElement _frameworkElement = new();

        public CheckerboardTransition()
        {
            this.NewContentStyle = (Style)_frameworkElement.FindResource("CheckerboardTransitionNewContentStyle");
            this.NewContentStoryboard = (Storyboard)_frameworkElement.FindResource("CheckerboardTransitionNewContentStoryboard");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.NotSupportedException.#ctor(System.String)")]
        protected override void OnDurationChanged(Duration oldDuration, Duration newDuration)
        {
            throw new System.NotSupportedException("CTP1 does not support changing the duration of this transition");
        }
    }
}
