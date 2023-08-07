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
    public class DoorTransition : Transition3D
    {
        static DoorTransition()
        {
            AcceptsNullContentProperty.OverrideMetadata(typeof(DoorTransition), new FrameworkPropertyMetadata(NullContentSupport.New));
        }

        public DoorTransition()
        {
            this.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            this.FieldOfView = 40;
        }

        protected override void BeginTransition3D(TransitionElement transitionElement, ContentPresenter oldContent, ContentPresenter newContent, Viewport3D viewport)
        {
            var clone = CreateBrush(oldContent);

            var size = transitionElement.RenderSize;
            var leftDoor = CreateMesh(new Point3D(),
               new Vector3D(size.Width / 2, 0, 0),
               new Vector3D(0, size.Height, 0),
               1,
               1,
               new Rect(0, 0, 0.5, 1));

            var leftDoorGeometry = new GeometryModel3D
            {
                Geometry = leftDoor,
                Material = new DiffuseMaterial(clone)
            };

            var leftRotation = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0);
            leftDoorGeometry.Transform = new RotateTransform3D(leftRotation);

            var rightDoorGeometry = new GeometryModel3D();
            var rightDoor = CreateMesh(new Point3D(size.Width / 2, 0, 0),
                 new Vector3D(size.Width / 2, 0, 0),
                 new Vector3D(0, size.Height, 0),
                 1,
                 1,
                 new Rect(0.5, 0, 0.5, 1));

            rightDoorGeometry.Geometry = rightDoor;
            rightDoorGeometry.Material = new DiffuseMaterial(clone);

            var rightRotation = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0);
            rightDoorGeometry.Transform = new RotateTransform3D(rightRotation, size.Width, 0, 0);


            var doors = new Model3DGroup();
            doors.Children.Add(leftDoorGeometry);
            doors.Children.Add(rightDoorGeometry);

            var model = new ModelVisual3D
            {
                Content = doors
            };

            // Replace old content in visual tree with new 3d model
            transitionElement.HideContent(oldContent);
            viewport.Children.Add(model);

            var da = new DoubleAnimation(90 - 0.5 * this.FieldOfView, this.Duration);
            leftRotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);

            da = new DoubleAnimation(-(90 - 0.5 * this.FieldOfView), this.Duration);
            rightRotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);

            da = new DoubleAnimation(0, this.Duration);
            da.Completed += delegate
            {
                this.EndTransition(transitionElement, oldContent, newContent);
            };
            clone.BeginAnimation(Brush.OpacityProperty, da);
        }
    }
}
