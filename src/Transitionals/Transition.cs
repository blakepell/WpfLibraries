/******************************************************************************
Copyright (c) Microsoft Corporation.  All rights reserved.


This file is licensed under the Microsoft Public License (Ms-PL). A copy of the Ms-PL should accompany this file. 
If it does not, you can obtain a copy from: 

http://www.microsoft.com/resources/sharedsource/licensingbasics/publiclicense.mspx
******************************************************************************/

using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using Transitionals.Controls;

namespace Transitionals
{
    /// <summary>
    /// Describes how null content is supported.
    /// </summary>
    public enum NullContentSupport
    {
        /// <summary>
        /// Transitioning to or from null is not supported.
        /// </summary>
        None,

        /// <summary>
        /// Transitioning from null to non-null is supported.
        /// </summary>
        Old,

        /// <summary>
        /// Transitioning from non-null to null is supported.
        /// </summary>
        New,

        /// <summary>
        /// Transitioning to or from null are both supported.
        /// </summary>
        Both
    }

    // Base class for all transitions.  
    [System.Runtime.InteropServices.ComVisible(false)]
    [System.ComponentModel.TypeConverter(typeof(TransitionConverter))]
    public abstract class Transition : DependencyObject
    {
        protected internal NullContentSupport NullContentSupport
        {
            get => (NullContentSupport)this.GetValue(AcceptsNullContentProperty);
            set => this.SetValue(AcceptsNullContentProperty, value);
        }

        public static readonly DependencyProperty AcceptsNullContentProperty =
            DependencyProperty.Register("AcceptsNullContent",
                typeof(NullContentSupport),
                typeof(Transition),
                new UIPropertyMetadata(NullContentSupport.Old));

        protected internal bool ClipToBounds
        {
            get => (bool)this.GetValue(ClipToBoundsProperty);
            set => this.SetValue(ClipToBoundsProperty, value);
        }

        public static readonly DependencyProperty ClipToBoundsProperty =
            DependencyProperty.Register(nameof(ClipToBounds),
                typeof(bool),
                typeof(Transition),
                new UIPropertyMetadata(false));

        public Duration Duration
        {
            get => (Duration)this.GetValue(DurationProperty);
            set => this.SetValue(DurationProperty, value);
        }

        private static void OnDurationChanged(object element, DependencyPropertyChangedEventArgs e)
        {
            if (element is Transition transition)
            {
                transition.OnDurationChanged((Duration)e.OldValue, (Duration)e.NewValue);
            }
        }

        protected virtual void OnDurationChanged(Duration oldDuration, Duration newDuration)
        {
        }

        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register(nameof(Duration), typeof(Duration), typeof(Transition), new UIPropertyMetadata(Duration.Automatic, OnDurationChanged));

        protected internal bool IsNewContentTopmost
        {
            get => (bool)this.GetValue(IsNewContentTopmostProperty);
            set => this.SetValue(IsNewContentTopmostProperty, value);
        }

        public static readonly DependencyProperty IsNewContentTopmostProperty =
            DependencyProperty.Register(nameof(IsNewContentTopmost),
                typeof(bool),
                typeof(Transition),
                new UIPropertyMetadata(true));

        // Called when an element is Removed from the TransitionPresenter's visual tree
        protected internal virtual void BeginTransition(TransitionElement transitionElement, ContentPresenter oldContent, ContentPresenter newContent)
        {
            this.EndTransition(transitionElement, oldContent, newContent);
        }

        //Transitions should call this method when they are done 
        protected void EndTransition(TransitionElement transitionElement, ContentPresenter oldContent, ContentPresenter newContent)
        {
            this.OnTransitionEnded(transitionElement, oldContent, newContent);

            if (transitionElement != null)
            {
                transitionElement.OnTransitionCompleted(this, oldContent, newContent);
            }
        }

        //Transitions can override this to perform cleanup at the end of the transition
        protected virtual void OnTransitionEnded(TransitionElement transitionElement, ContentPresenter oldContent, ContentPresenter newContent)
        {
        }

        // Returns a clone of the element
        protected static Brush CreateBrush(FrameworkElement frameworkElement)
        {
            // The code below originally hid the element from the tree.
            // We're no longer using Decorator as the parent and we're now 
            // leaving it up to the transition to determine if or when the 
            // original element should be hidden from the visual tree.

            //if (frameworkElement != null)
            //{
            //    Decorator decorator = frameworkElement.Parent as Decorator;
            //    if (decorator != null)
            //    {
            //        decorator.Visibility = Visibility.Hidden;
            //    }
            //}

            var brush = new VisualBrush(frameworkElement)
            {
                ViewportUnits = BrushMappingMode.Absolute
            };
            RenderOptions.SetCachingHint(brush, CachingHint.Cache);
            RenderOptions.SetCacheInvalidationThresholdMinimum(brush, 40);
            return brush;
        }
    }
}
