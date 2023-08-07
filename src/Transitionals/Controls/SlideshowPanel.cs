/******************************************************************************
Copyright (c) Microsoft Corporation.  All rights reserved.


This file is licensed under the Microsoft Public License (Ms-PL). A copy of the Ms-PL should accompany this file. 
If it does not, you can obtain a copy from: 

http://www.microsoft.com/resources/sharedsource/licensingbasics/publiclicense.mspx
******************************************************************************/

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Transitionals.Controls
{
    /// <summary>
    /// A panel that can display only a single item. If the panel is associated with a selector, 
    /// only the selected item is displayed. When the active or selected item is replaced, a 
    /// transition occurs from the old item to the new one.
    /// </summary>
    internal class SlideshowPanel : VirtualizingPanel
    {
        /************************************************
		 * Member Variables
		 ***********************************************/
        private int _currentIndex = -1;
        private bool _panelAdded;
        private Selector _selector;
        private Slideshow _slideShow;
        private TransitionElement _transitionElement;

        // TODO: Need collection of transitions as a DependencyProperty.

        /************************************************
		 * Constructors
		 ***********************************************/
        /// <summary>
        /// Initializes a new <see cref="SingleItemPanel"/>.
        /// </summary>
        public SlideshowPanel()
        {
            this.Initialize();
        }

        /************************************************
		 * Internal Methods
		 ***********************************************/
        /// <summary>
        /// Handles the selector changing.
        /// </summary>
        private void HandleSelectorChange()
        {
            // If a previous selector exists, unsubscribe.
            if (_selector != null)
            {
                _selector.SelectionChanged -= this.selector_SelectionChanged;
            }

            // Try to get new selector from parent
            ItemsControl ic = ItemsControl.GetItemsOwner(this) as Selector;
            bool isHost = this.IsItemsHost;
            var presenter = this.TemplatedParent as ItemsPresenter;
            _selector = ItemsControl.GetItemsOwner(this) as Selector;

            // The selector should probably only be a Slideshow, but to keep the logic
            // separate we'll have a second variable.
            _slideShow = _selector as Slideshow;

            // If we have a new selector, subscribe to events
            if (_selector != null)
            {
                _selector.SelectionChanged += this.selector_SelectionChanged;
            }
        }

        /// <summary>
        /// Ensures that the transition panel has been created and added to the controls
        /// child collection.
        /// </summary>
        private void EnsureTransitionPanel()
        {
            // Add it as a visual child
            if (!_panelAdded)
            {
                try
                {
                    this.AddInternalChild(_transitionElement);
                    _panelAdded = true;
                }
                catch (Exception ex)
                {
                    string g = ex.Message;
                }
            }
        }

        /// <summary>
        /// Initializes the control and child controls.
        /// </summary>
        private void Initialize()
        {
            // Access internal children, which forces instantiation of the generator
            var children = this.InternalChildren;
            
            // We are an items host
            this.IsItemsHost = true; // TODO: Should do based on parent container or selector?

            // Create the transition element
            _transitionElement = new TransitionElement();
        }

        /************************************************
		 * Overrides / Event Handlers
		 ***********************************************/
        protected override Size ArrangeOverride(Size finalSize)
        {
            /*
            // We only have one child to arrange but it may not be created yet
            if (transitionElement != null)
            {
                transitionElement.Arrange(new Rect(new Point(), finalSize));
            }
             * */

            foreach (UIElement e in this.Children)
            {
                // TODO: Should we use e.DesiredSize instead and center it?
                e.Arrange(new Rect(new Point(0, 0), finalSize));
            }

            // Return the final size, which is the recommended size passed in
            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var resultSize = new Size(0, 0);

            foreach (UIElement child in this.Children)
            {
                child.Measure(availableSize);
                resultSize.Width = Math.Max(resultSize.Width, child.DesiredSize.Width);
                resultSize.Height = Math.Max(resultSize.Height, child.DesiredSize.Height);
            }

            resultSize.Width = double.IsPositiveInfinity(availableSize.Width) ? resultSize.Width : availableSize.Width;
            resultSize.Height = double.IsPositiveInfinity(availableSize.Height) ? resultSize.Height : availableSize.Height;

            return resultSize;
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            // Only allow our special child control
            if (visualAdded != _transitionElement)
            {
                // throw new NotSupportedException("This child contents of this control cannot be directly manipulated.");
            }
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            this.HandleSelectorChange();
        }

        private void selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If there is a new item, realize it and start a transition.
            // If there is an old item, see if it's realized. If it is, unrealize it.
            
            // The transition panel cannot be created any earlier or data binding 
            // may not behave as expected. We must ensure it is created here so that 
            // we can use it to transition content.
            this.EnsureTransitionPanel();

            // We must access the InternalChildren collection every time 
            // before we can access the generator. This may be a bug in 
            // the framework.
            var children = this.InternalChildren;

            // Get the generator
            var generator = this.ItemContainerGenerator;

            // If there is no generator we can't realize or virtualize so just bail
            if (generator == null) { return; }

            // If old item exists, mark it for virtualization
            if (_currentIndex > -1)
            {
                var currentPosition = generator.GeneratorPositionFromIndex(_currentIndex);
                generator.Remove(currentPosition, 1);
                _currentIndex = -1;
            }

            // Get the newly selected item index
            _currentIndex = _selector.SelectedIndex;
            
            // Only try to add new content if we have a selection
            if (_currentIndex > -1)
            {
                // Get the generator position for the index
                var newPosition = generator.GeneratorPositionFromIndex(_currentIndex);

                // Realize the new object
                DependencyObject newVisual = null;
                using (generator.StartAt(newPosition, GeneratorDirection.Forward))
                {
                    newVisual = generator.GenerateNext();
                }

                // Tell the selector to use the current list of transitions and duration
                if (_slideShow != null)
                {
                    _transitionElement.Transition = _slideShow.Transition;
                    _transitionElement.TransitionSelector = _slideShow.TransitionSelector;
                }
                else
                {
                    _transitionElement.Transition = null;
                    _transitionElement.TransitionSelector = null;
                }

                // Set content into the transition element
                _transitionElement.Content = newVisual;

                // Prepare the item for its container
                // Must be called after the element has been added to the visual tree, 
                // so that resource references and inherited properties work correctly.
                generator.PrepareItemContainer(newVisual);
            }

            // Changing the selected item may change the desired size so we 
            // must re-measure
            this.InvalidateMeasure();
            this.InvalidateArrange();
        }
    }
}
