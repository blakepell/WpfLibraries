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
using System.Windows.Media.Media3D;
using Transitionals.Controls;

namespace Transitionals
{
    /// <summary>
    /// Base class for 3D transitions
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(false)]
    public abstract class Transition3D : Transition
    {
        static Transition3D()
        {
            var defaultLight = new Model3DGroup();

            var direction = new Vector3D(1,1,1);
            direction.Normalize();
            byte ambient = 108; // 108 is minimum for directional to be < 256 (for direction = [1,1,1])
            byte directional = (byte)Math.Min((255-ambient) / Vector3D.DotProduct(direction, new Vector3D(0,0,1)), 255);

            defaultLight.Children.Add(new AmbientLight(Color.FromRgb(ambient, ambient, ambient)));
            defaultLight.Children.Add(new DirectionalLight(Color.FromRgb(directional, directional, directional), direction));
            defaultLight.Freeze();
            LightProperty = DependencyProperty.Register("Light", typeof(Model3D), typeof(Transition3D), new UIPropertyMetadata(defaultLight));
        }

        public double FieldOfView
        {
            get => (double)this.GetValue(FieldOfViewProperty);
            set => this.SetValue(FieldOfViewProperty, value);
        }

        public static readonly DependencyProperty FieldOfViewProperty =
            DependencyProperty.Register(nameof(FieldOfView), typeof(double), typeof(Transition3D), new UIPropertyMetadata(20.0));

     
        public Model3D Light
        {
            get => (Model3D)this.GetValue(LightProperty);
            set => this.SetValue(LightProperty, value);
        }

        public static readonly DependencyProperty LightProperty;

        /// <summary>
        /// Setup the Viewport 3D
        /// </summary>
        /// <param name="transitionElement"></param>
        /// <param name="oldContent"></param>
        /// <param name="newContent"></param>
        protected internal sealed override void BeginTransition(TransitionElement transitionElement, ContentPresenter oldContent, ContentPresenter newContent)
        {
            var viewport = new Viewport3D
            {
                IsHitTestVisible = false,
                Camera = this.CreateCamera(transitionElement, this.FieldOfView),
                ClipToBounds = false
            };

            var light = new ModelVisual3D
            {
                Content = this.Light
            };
            viewport.Children.Add(light);

            transitionElement.Children.Add(viewport);
            this.BeginTransition3D(transitionElement, oldContent, newContent, viewport);
        }

        protected virtual Camera CreateCamera(UIElement uiElement, double fieldOfView)
        {
            var size = uiElement.RenderSize;
            return new PerspectiveCamera(new Point3D(size.Width / 2, size.Height / 2, -size.Width / Math.Tan(fieldOfView / 2 * Math.PI / 180) / 2),
                                         new Vector3D(0, 0, 1),
                                         new Vector3D(0, -1, 0),
                                         fieldOfView);
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "3#viewport")]
        protected virtual void BeginTransition3D(TransitionElement transitionElement, ContentPresenter oldContent, ContentPresenter newContent, Viewport3D viewport)
        {
            this.EndTransition(transitionElement, oldContent, newContent);
        }

        /// <summary>
        /// Generates a flat mesh starting at origin with sides equal to vector1 and vector2 vectors
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <param name="steps1"></param>
        /// <param name="steps2"></param>
        /// <param name="textureBounds"></param>
        /// <returns></returns>
        public static MeshGeometry3D CreateMesh(Point3D origin, Vector3D vector1, Vector3D vector2, int steps1, int steps2, Rect textureBounds)
        {
            vector1 = 1.0 / steps1 * vector1;
            vector2 = 1.0 / steps2 * vector2;

            var mesh = new MeshGeometry3D();

            for (int i = 0; i <= steps1; i++)
            {
                for (int j = 0; j <= steps2; j++)
                {
                    mesh.Positions.Add(origin + i * vector1 + j * vector2);

                    mesh.TextureCoordinates.Add(new Point(textureBounds.X + textureBounds.Width * i / steps1,
                                                          textureBounds.Y + textureBounds.Height * j / steps2));
                    if (i > 0 && j > 0)
                    {
                        mesh.TriangleIndices.Add((i - 1) * (steps2 + 1) + (j - 1));
                        mesh.TriangleIndices.Add((i - 0) * (steps2 + 1) + (j - 0));
                        mesh.TriangleIndices.Add((i - 0) * (steps2 + 1) + (j - 1));

                        mesh.TriangleIndices.Add((i - 1) * (steps2 + 1) + (j - 1));
                        mesh.TriangleIndices.Add((i - 1) * (steps2 + 1) + (j - 0));
                        mesh.TriangleIndices.Add((i - 0) * (steps2 + 1) + (j - 0));
                    }
                }
            }
            return mesh;
        }
    }
}
