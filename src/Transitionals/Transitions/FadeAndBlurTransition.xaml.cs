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
    ///     Stores the XAML that defines the FadeAndBlurTransition
    /// </summary>
    [ComVisible(false)]
    public partial class FadeAndBlurTransitionFrameworkElement
    {
        public FadeAndBlurTransitionFrameworkElement()
        {
            this.InitializeComponent();
        }
    }

    /// <summary>
    ///     Represents the FadeAndBlurTransition
    /// </summary>
    [ComVisible(false)]
    public class FadeAndBlurTransition : StoryboardTransition
    {
        private static FadeAndBlurTransitionFrameworkElement _frameworkElement = new();

        static FadeAndBlurTransition()
        {
            AcceptsNullContentProperty.OverrideMetadata(typeof(FadeAndBlurTransition), new FrameworkPropertyMetadata(NullContentSupport.New));
            IsNewContentTopmostProperty.OverrideMetadata(typeof(FadeAndBlurTransition), new FrameworkPropertyMetadata(false));
        }

        public FadeAndBlurTransition()
        {
            this.OldContentStyle = (Style)_frameworkElement.FindResource("FadeAndBlurTransitionOldContentStyle");
            this.OldContentStoryboard = (Storyboard)_frameworkElement.FindResource("FadeAndBlurTransitionOldContentStoryboard");
            this.NewContentStyle = (Style)_frameworkElement.FindResource("FadeAndBlurTransitionNewContentStyle");
            this.NewContentStoryboard = (Storyboard)_frameworkElement.FindResource("FadeAndBlurTransitionNewContentStoryboard");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.NotSupportedException.#ctor(System.String)")]
        protected override void OnDurationChanged(Duration oldDuration, Duration newDuration)
        {
            throw new System.NotSupportedException("CTP1 does not support changing the duration of this transition");
        }
    }
}
