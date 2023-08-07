/******************************************************************************
Copyright (c) Microsoft Corporation.  All rights reserved.


This file is licensed under the Microsoft Public License (Ms-PL). A copy of the Ms-PL should accompany this file. 
If it does not, you can obtain a copy from: 

http://www.microsoft.com/resources/sharedsource/licensingbasics/publiclicense.mspx
******************************************************************************/

using System.Windows;

namespace Transitionals.Transitions
{
    public enum TransitionDirection
    {
        Forward,
        Backward,
    }

    //Choose between a forward and backward transition based on the Direction property
    [System.Runtime.InteropServices.ComVisible(false)]
    public class TwoWayTransitionSelector : TransitionSelector
    {
        public Transition ForwardTransition
        {
            get => (Transition)this.GetValue(ForwardTransitionProperty);
            set => this.SetValue(ForwardTransitionProperty, value);
        }

        public static readonly DependencyProperty ForwardTransitionProperty =
            DependencyProperty.Register(nameof(ForwardTransition), typeof(Transition), typeof(TwoWayTransitionSelector), new UIPropertyMetadata(null));

        public Transition BackwardTransition
        {
            get => (Transition)this.GetValue(BackwardTransitionProperty);
            set => this.SetValue(BackwardTransitionProperty, value);
        }

        public static readonly DependencyProperty BackwardTransitionProperty =
            DependencyProperty.Register(nameof(BackwardTransition), typeof(Transition), typeof(TwoWayTransitionSelector), new UIPropertyMetadata(null));


        public TransitionDirection Direction
        {
            get => (TransitionDirection)this.GetValue(DirectionProperty);
            set => this.SetValue(DirectionProperty, value);
        }

        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register(nameof(Direction), typeof(TransitionDirection), typeof(TwoWayTransitionSelector), new UIPropertyMetadata(TransitionDirection.Forward));


        public override Transition SelectTransition(object oldContent, object newContent)
        {
            return this.Direction == TransitionDirection.Forward ? this.ForwardTransition : this.BackwardTransition;
        }
    }
}
