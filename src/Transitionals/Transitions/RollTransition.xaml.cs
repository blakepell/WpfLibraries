/******************************************************************************
Copyright (c) Microsoft Corporation.  All rights reserved.


This file is licensed under the Microsoft Public License (Ms-PL). A copy of the Ms-PL should accompany this file. 
If it does not, you can obtain a copy from: 

http://www.microsoft.com/resources/sharedsource/licensingbasics/publiclicense.mspx
******************************************************************************/

using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Runtime.InteropServices;

namespace Transitionals.Transitions
{
    /// <summary>
    ///     Stores the XAML that defines the RollTransition
    /// </summary>
    [ComVisible(false)]
    public partial class RollTransitionFrameworkElement
    {
        public RollTransitionFrameworkElement()
        {
            this.InitializeComponent();
        }
    }

    /// <summary>
    ///     Represents the RollTransition
    /// </summary>
    [ComVisible(false)]
    public class RollTransition : StoryboardTransition
    {
        private static RollTransitionFrameworkElement _frameworkElement = new();

        static RollTransition()
        {
            AcceptsNullContentProperty.OverrideMetadata(typeof(RollTransition), new FrameworkPropertyMetadata(NullContentSupport.New));
            IsNewContentTopmostProperty.OverrideMetadata(typeof(RollTransition), new FrameworkPropertyMetadata(false));
            ClipToBoundsProperty.OverrideMetadata(typeof(RollTransition), new FrameworkPropertyMetadata(true));
        }

        public RollTransition()
        {
            this.OldContentStyle = (Style)_frameworkElement.FindResource("RollTransitionOldContentStyle");
            this.OldContentStoryboard = (Storyboard)_frameworkElement.FindResource("RollTransitionOldContentStoryboard");
            this.Duration = new Duration(TimeSpan.FromSeconds(0.5));
        }

        protected override void OnDurationChanged(Duration oldDuration, Duration newDuration)
        {
            if (this.OldContentStoryboard != null && this.OldContentStoryboard.Children.Count > 0)
            {
                this.OldContentStoryboard.Children[0].Duration = newDuration;
            }
        }
    }
}
