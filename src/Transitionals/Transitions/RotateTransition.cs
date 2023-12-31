/******************************************************************************
Copyright (c) Microsoft Corporation.  All rights reserved.


This file is licensed under the Microsoft Public License (Ms-PL). A copy of the Ms-PL should accompany this file. 
If it does not, you can obtain a copy from: 

http://www.microsoft.com/resources/sharedsource/licensingbasics/publiclicense.mspx
******************************************************************************/

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using Transitionals.Controls;

namespace Transitionals.Transitions
{
    public enum RotateDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    [System.Runtime.InteropServices.ComVisible(false)]
    public class RotateTransition : Transition3D
    {
        static RotateTransition()
        {
            AcceptsNullContentProperty.OverrideMetadata(typeof(RotateTransition), new FrameworkPropertyMetadata(NullContentSupport.Both));
        }

        public RotateTransition()
        {
            this.Duration = new Duration(TimeSpan.FromSeconds(0.75));
            this.Angle = 90;
            this.FieldOfView = 40;
        }

        public double Angle
        {
            get => (double)this.GetValue(AngleProperty);
            set => this.SetValue(AngleProperty, value);
        }

        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register(nameof(Angle), typeof(double), typeof(RotateTransition), new UIPropertyMetadata(90.0), IsAngleValid);

        private static bool IsAngleValid(object value)
        {
            double angle = (double)value;
            return angle >= 0 && angle < 180;
        }

        public RotateDirection Direction
        {
            get => (RotateDirection)this.GetValue(DirectionProperty);
            set => this.SetValue(DirectionProperty, value);
        }

        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register(nameof(Direction), typeof(RotateDirection), typeof(RotateTransition), new UIPropertyMetadata(RotateDirection.Left));

        public bool Contained
        {
            get => (bool)this.GetValue(ContainedProperty);
            set => this.SetValue(ContainedProperty, value);
        }

        public static readonly DependencyProperty ContainedProperty =
            DependencyProperty.Register(nameof(Contained), typeof(bool), typeof(RotateTransition), new UIPropertyMetadata(false));

        protected override void BeginTransition3D(TransitionElement transitionElement, ContentPresenter oldContent, ContentPresenter newContent, Viewport3D viewport)
        {
            var size = transitionElement.RenderSize;

            var origin = new Point3D(); // origin of 2nd face
            Vector3D u = new(), v = new(); // u & v vectors of 2nd face

            double angle = this.Angle;
            Point3D rotationCenter;
            Vector3D rotationAxis;
            var direction = this.Direction;

            TranslateTransform3D translation = null;
            double angleRads = this.Angle * Math.PI / 180;
            if (direction == RotateDirection.Left || direction == RotateDirection.Right)
            {
                if (this.Contained)
                {
                    rotationCenter = new Point3D(direction == RotateDirection.Left ? size.Width : 0, 0, 0);
                    translation = new TranslateTransform3D();
                    var x = new DoubleAnimation(direction == RotateDirection.Left ? -size.Width : size.Width, this.Duration);
                    translation.BeginAnimation(TranslateTransform3D.OffsetXProperty, x);
                }
                else
                {
                    rotationCenter = new Point3D(size.Width / 2, 0, size.Width / 2 * Math.Tan(angle / 2 * Math.PI / 180));
                }
                
                rotationAxis = new Vector3D(0, 1, 0);

                if (direction == RotateDirection.Left)
                {
                    u.X = -size.Width * Math.Cos(angleRads);
                    u.Z = size.Width * Math.Sin(angleRads);

                    origin.X = size.Width;
                }
                else
                {
                    u.X = -size.Width * Math.Cos(angleRads);
                    u.Z = -size.Width * Math.Sin(angleRads);

                    origin.X = -u.X;
                    origin.Z = -u.Z;
                }
                v.Y = size.Height;
            }
            else
            {
                if (this.Contained)
                {
                    rotationCenter = new Point3D(0, direction == RotateDirection.Up ? size.Height : 0, 0);
                    translation = new TranslateTransform3D();
                    var y = new DoubleAnimation(direction == RotateDirection.Up ? -size.Height : size.Height, this.Duration);
                    translation.BeginAnimation(TranslateTransform3D.OffsetYProperty, y);
                }
                else
                {
                    rotationCenter = new Point3D(0, size.Height / 2, size.Height / 2 * Math.Tan(angle / 2 * Math.PI / 180));
                }
                
                rotationAxis = new Vector3D(1, 0, 0);

                if (direction == RotateDirection.Up)
                {
                    v.Y = -size.Height * Math.Cos(angleRads);
                    v.Z = size.Height * Math.Sin(angleRads);

                    origin.Y = size.Height;
                }
                else
                {
                    v.Y = -size.Height * Math.Cos(angleRads);
                    v.Z = -size.Height * Math.Sin(angleRads);

                    origin.Y = -v.Y;
                    origin.Z = -v.Z;
                }
                u.X = size.Width;
            }

            double endAngle = 180 - angle;
            if (direction == RotateDirection.Right || direction == RotateDirection.Up)
            {
                endAngle = -endAngle;
            }

            ModelVisual3D m1, m2;
            viewport.Children.Add(m1 = this.MakeSide(oldContent, new Point3D(), new Vector3D(size.Width,0,0), new Vector3D(0,size.Height,0), endAngle, rotationCenter, rotationAxis, null));
            viewport.Children.Add(m2 = this.MakeSide(newContent, origin, u, v, endAngle, rotationCenter, rotationAxis, delegate
                {
                    this.EndTransition(transitionElement, oldContent, newContent);
                }));

            m1.Transform = m2.Transform = translation;

            // Replace old and new content in visual tree with new 3d models
            transitionElement.HideContent(oldContent);
            transitionElement.HideContent(newContent);
        }
        
        private ModelVisual3D MakeSide(ContentPresenter content, Point3D origin, Vector3D u, Vector3D v, double endAngle, Point3D rotationCenter, Vector3D rotationAxis, EventHandler onCompleted)
        {
            var sideMesh = CreateMesh(origin, u, v, 1, 1, new Rect(0, 0, 1, 1));

            var sideModel = new GeometryModel3D
            {
                Geometry = sideMesh
            };

            var clone = CreateBrush(content);
            sideModel.Material = new DiffuseMaterial(clone);

            var rotation = new AxisAngleRotation3D(rotationAxis, 0);
            sideModel.Transform = new RotateTransform3D(rotation, rotationCenter);


            var da = new DoubleAnimation(endAngle, this.Duration);
            if (onCompleted != null)
            {
                da.Completed += onCompleted;
            }

            rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);

            var side = new ModelVisual3D
            {
                Content = sideModel
            };
            return side;
        }
    }
}
