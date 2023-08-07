/******************************************************************************
Copyright (c) Microsoft Corporation.  All rights reserved.


This file is licensed under the Microsoft Public License (Ms-PL). A copy of the Ms-PL should accompany this file. 
If it does not, you can obtain a copy from: 

http://www.microsoft.com/resources/sharedsource/licensingbasics/publiclicense.mspx
******************************************************************************/

using System.Windows.Media.Animation;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.InteropServices;
using Transitionals.Controls;

namespace Transitionals.Transitions
{
    // Transition with storyboards for the old and new content presenters
    [StyleTypedProperty(Property="OldContentStyle", StyleTargetType=typeof(ContentPresenter))]
    [StyleTypedProperty(Property="NewContentStyle", StyleTargetType=typeof(ContentPresenter))]
    [ComVisible(false)]
    public abstract class StoryboardTransition : Transition
    {
        public Style OldContentStyle
        {
            get => (Style)this.GetValue(OldContentStyleProperty);
            set => this.SetValue(OldContentStyleProperty, value);
        }

        public static readonly DependencyProperty OldContentStyleProperty =
            DependencyProperty.Register(nameof(OldContentStyle), 
                typeof(Style), 
                typeof(StoryboardTransition), 
                new UIPropertyMetadata(null));
        

        public Storyboard OldContentStoryboard
        {
            get => (Storyboard)this.GetValue(OldContentStoryboardProperty);
            set => this.SetValue(OldContentStoryboardProperty, value);
        }

        public static readonly DependencyProperty OldContentStoryboardProperty =
           DependencyProperty.Register(nameof(OldContentStoryboard),
               typeof(Storyboard),
               typeof(StoryboardTransition),
               new UIPropertyMetadata(null));

        public Style NewContentStyle
        {
            get => (Style)this.GetValue(NewContentStyleProperty);
            set => this.SetValue(NewContentStyleProperty, value);
        }

        public static readonly DependencyProperty NewContentStyleProperty =
            DependencyProperty.Register(nameof(NewContentStyle),
                typeof(Style),
                typeof(StoryboardTransition),
                new UIPropertyMetadata(null));

        public Storyboard NewContentStoryboard
        {
            get => (Storyboard)this.GetValue(NewContentStoryboardProperty);
            set => this.SetValue(NewContentStoryboardProperty, value);
        }

        public static readonly DependencyProperty NewContentStoryboardProperty =
            DependencyProperty.Register(nameof(NewContentStoryboard), 
                typeof(Storyboard), 
                typeof(StoryboardTransition), 
                new UIPropertyMetadata(null));


        protected internal override void BeginTransition(TransitionElement transitionElement, ContentPresenter oldContent, ContentPresenter newContent)
        {
            var oldStoryboard = this.OldContentStoryboard;
            var newStoryboard = this.NewContentStoryboard;

            if (oldStoryboard != null || newStoryboard != null)
            {
                oldContent.Style = this.OldContentStyle;
                newContent.Style = this.NewContentStyle;

                // Flag to determine when both storyboards are done
                bool done = oldStoryboard == null || newStoryboard == null;

                if (oldStoryboard != null)
                {
                    oldStoryboard = oldStoryboard.Clone();
                    oldContent.SetValue(OldContentStoryboardProperty, oldStoryboard);
                    oldStoryboard.Completed += delegate
                    {
                        if (done)
                        {
                            this.EndTransition(transitionElement, oldContent, newContent);
                        }

                        done = true;
                    };
                    oldStoryboard.Begin(oldContent, true);
                }

                if (newStoryboard != null)
                {
                    newStoryboard = newStoryboard.Clone();
                    newContent.SetValue(NewContentStoryboardProperty, newStoryboard);
                    newStoryboard.Completed += delegate
                    {
                        if (done)
                        {
                            this.EndTransition(transitionElement, oldContent, newContent);
                        }

                        done = true;
                    };
                    newStoryboard.Begin(newContent, true);
                }
            }
            else
            {
                this.EndTransition(transitionElement, oldContent, newContent);
            }
        }

        protected override void OnTransitionEnded(TransitionElement transitionElement, ContentPresenter oldContent, ContentPresenter newContent)
        {
            var oldStoryboard = (Storyboard)oldContent.GetValue(OldContentStoryboardProperty);
            if (oldStoryboard != null)
            {
                oldStoryboard.Stop(oldContent);
            }

            oldContent.ClearValue(FrameworkElement.StyleProperty);

            var newStoryboard = (Storyboard)newContent.GetValue(NewContentStoryboardProperty);
            if (newStoryboard != null)
            {
                newStoryboard.Stop(newContent);
            }

            newContent.ClearValue(FrameworkElement.StyleProperty);
        }
    }
}
