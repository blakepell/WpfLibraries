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
using System.Windows.Media.Media3D;
using Transitionals.Controls;

namespace Transitionals.Transitions
{
    [System.Runtime.InteropServices.ComVisible(false)]
    public class FlipTransition : Transition3D
    {
        static FlipTransition()
        {
            AcceptsNullContentProperty.OverrideMetadata(typeof(FlipTransition), new FrameworkPropertyMetadata(NullContentSupport.None));
        }

        public FlipTransition()
        {
            this.Duration = new Duration(TimeSpan.FromSeconds(0.5));
        }

        public RotateDirection Direction
        {
            get => (RotateDirection)this.GetValue(DirectionProperty);
            set => this.SetValue(DirectionProperty, value);
        }

        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register(nameof(Direction), typeof(RotateDirection), typeof(FlipTransition), new UIPropertyMetadata(RotateDirection.Left));


        protected override void BeginTransition3D(TransitionElement transitionElement, ContentPresenter oldContent, ContentPresenter newContent, Viewport3D viewport)
        {
            var size = transitionElement.RenderSize;

            // Create a rectangle
            var mesh = CreateMesh(new Point3D(),
                new Vector3D(size.Width, 0, 0),
                new Vector3D(0, size.Height, 0),
                1,
                1,
                new Rect(0, 0, 1, 1));
            
            var geometry = new GeometryModel3D
            {
                Geometry = mesh
            };
            var clone = new VisualBrush(oldContent);
            geometry.Material = new DiffuseMaterial(clone);

            var model = new ModelVisual3D
            {
                Content = geometry
            };

            // Replace old content in visual tree with new 3d model
            transitionElement.HideContent(oldContent);
            viewport.Children.Add(model);

            Vector3D axis;
            var center = new Point3D();
            switch (this.Direction)
            {
                case RotateDirection.Left:
                    axis = new Vector3D(0, 1, 0);
                    break;
                case RotateDirection.Right:
                    axis = new Vector3D(0, -1, 0);
                    center = new Point3D(size.Width, 0, 0);
                    break;
                case RotateDirection.Up:
                    axis = new Vector3D(-1, 0, 0);
                    break;
                default:
                    axis = new Vector3D(1, 0, 0);
                    center = new Point3D(0, size.Height, 0);
                    break;
            }
            var rotation = new AxisAngleRotation3D(axis, 0);
            model.Transform = new RotateTransform3D(rotation, center);

            var da = new DoubleAnimation(0, this.Duration);
            clone.BeginAnimation(Brush.OpacityProperty, da);

            da = new DoubleAnimation(90, this.Duration);
            da.Completed += delegate
            {
                this.EndTransition(transitionElement, oldContent, newContent);
            };
            rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);
        }
    }
}
