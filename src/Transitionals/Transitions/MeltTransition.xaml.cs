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
    ///     Stores the XAML that defines the MeltTransition
    /// </summary>
    [ComVisible(false)]
    public partial class MeltTransitionFrameworkElement : FrameworkElement
    {
        public MeltTransitionFrameworkElement()
        {
            this.InitializeComponent();
        }
    }

    /// <summary>
    ///     Represents the MeltTransition
    /// </summary>
    [ComVisible(false)]
    public class MeltTransition : StoryboardTransition
    {
        private static MeltTransitionFrameworkElement _frameworkElement = new();

        public MeltTransition()
        {
            this.NewContentStyle = (Style) _frameworkElement.FindResource("MeltTransitionNewContentStyle");
            this.NewContentStoryboard = (Storyboard) _frameworkElement.FindResource("MeltTransitionNewContentStoryboard");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.NotSupportedException.#ctor(System.String)")]
        protected override void OnDurationChanged(Duration oldDuration, Duration newDuration)
        {
            throw new System.NotSupportedException("CTP1 does not support changing the duration of this transition");
        }
    }
}
