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
    ///     Stores the XAML that defines the DotsTransition
    /// </summary>
    [ComVisible(false)]
    public partial class DotsTransitionFrameworkElement
    {
        public DotsTransitionFrameworkElement()
        {
            this.InitializeComponent();
        }
    }

    /// <summary>
    ///     Represents the DotsTransition
    /// </summary>
    [ComVisible(false)]
    public class DotsTransition : StoryboardTransition
    {
        private static DotsTransitionFrameworkElement _frameworkElement = new();

        public DotsTransition()
        {
            this.NewContentStyle = (Style)_frameworkElement.FindResource("DotsTransitionNewContentStyle");
            this.NewContentStoryboard = (Storyboard)_frameworkElement.FindResource("DotsTransitionNewContentStoryboard");
            this.Duration = new Duration(TimeSpan.FromSeconds(0.5));
        }

        protected override void OnDurationChanged(Duration oldDuration, Duration newDuration)
        {
            if (this.NewContentStoryboard != null && this.NewContentStoryboard.Children.Count > 0)
            {
                this.NewContentStoryboard.Children[0].Duration = newDuration;
            }
        }
    }
}
