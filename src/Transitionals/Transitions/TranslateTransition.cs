/******************************************************************************
Copyright (c) Microsoft Corporation.  All rights reserved.


This file is licensed under the Microsoft Public License (Ms-PL). A copy of the Ms-PL should accompany this file. 
If it does not, you can obtain a copy from: 

http://www.microsoft.com/resources/sharedsource/licensingbasics/publiclicense.mspx
******************************************************************************/

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Transitionals.Controls;

namespace Transitionals.Transitions
{
    // Applies a Translation to the content.  You can specify the starting point of the new 
    // content or the ending point of the old content using relative coordinates.
    // Set start point to (-1,0) to have the content slide from the left 
    [System.Runtime.InteropServices.ComVisible(false)]
    public class TranslateTransition : Transition
    {
        static TranslateTransition()
        {
            ClipToBoundsProperty.OverrideMetadata(typeof(TranslateTransition), new FrameworkPropertyMetadata(true));
        }

        public TranslateTransition()
        {
            this.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            this.StartPoint = new Point(-1, 0);
        }

        public Point StartPoint
        {
            get => (Point)this.GetValue(StartPointProperty);
            set => this.SetValue(StartPointProperty, value);
        }

        public static readonly DependencyProperty StartPointProperty =
            DependencyProperty.Register(nameof(StartPoint), typeof(Point), typeof(TranslateTransition), new UIPropertyMetadata(new Point()));

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "EndPoint")]
        public Point EndPoint
        {
            get => (Point)this.GetValue(EndPointProperty);
            set => this.SetValue(EndPointProperty, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "EndPoint")]
        public static readonly DependencyProperty EndPointProperty =
            DependencyProperty.Register(nameof(EndPoint), typeof(Point), typeof(TranslateTransition), new UIPropertyMetadata(new Point()));

        
        protected internal override void BeginTransition(TransitionElement transitionElement, ContentPresenter oldContent, ContentPresenter newContent)
        {
            var tt = new TranslateTransform(this.StartPoint.X * transitionElement.ActualWidth, this.StartPoint.Y * transitionElement.ActualHeight);

            if (this.IsNewContentTopmost)
            {
                newContent.RenderTransform = tt;
            }
            else
            {
                oldContent.RenderTransform = tt;
            }

            var da = new DoubleAnimation(this.EndPoint.X * transitionElement.ActualWidth, this.Duration);
            tt.BeginAnimation(TranslateTransform.XProperty, da);

            da.To = this.EndPoint.Y * transitionElement.ActualHeight;
            da.Completed += delegate
            {
                this.EndTransition(transitionElement, oldContent, newContent);
            };
            tt.BeginAnimation(TranslateTransform.YProperty, da);
        }

        protected override void OnTransitionEnded(TransitionElement transitionElement, ContentPresenter oldContent, ContentPresenter newContent)
        {
            newContent.ClearValue(UIElement.RenderTransformProperty);
            oldContent.ClearValue(UIElement.RenderTransformProperty);
        }
    }
}
