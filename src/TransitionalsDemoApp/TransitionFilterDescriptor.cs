/******************************************************************************
Copyright (c) Microsoft Corporation.  All rights reserved.


This file is licensed under the Microsoft Public License (Ms-PL). A copy of the Ms-PL should accompany this file. 
If it does not, you can obtain a copy from: 

http://www.microsoft.com/resources/sharedsource/licensingbasics/publiclicense.mspx
******************************************************************************/

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Transitionals;

namespace TransitionalsDemoApp
{
    /// <summary>
    /// Defines the filter level of properties to be displayed.
    /// </summary>
    public enum TransitionFilterLevel
    {
        /// <summary>
        /// The properties that are defined on the selected transition will be displayed 
        /// along with the properties defined on transition-related base classes (Transition, 
        /// Transition3D and any classes that inherit from them).
        /// </summary>
        TransitionPlusBase,

        /// <summary>
        /// Only properties that are defined on the selected transition will be displayed.
        /// </summary>
        TransitionSpecific,
        
        /// <summary>
        /// All properties will be displayed.
        /// </summary>
        All
    };

    /// <summary>
    /// A custom type descriptor that can be used to show or hide properties in a property grid based 
    /// on a filter level.
    /// </summary>
    public class TransitionFilterDescriptor : CustomTypeDescriptor
    {
        private object _component;

        public TransitionFilterDescriptor(object component, TransitionFilterLevel filter)
        {
            this._component = component;
            this.Filter = filter;
        }

        private bool MatchesFilter(PropertyDescriptor property)
        {
            var componentType = _component.GetType();
            var dProperty = DependencyPropertyDescriptor.FromProperty(property);
            
            switch (this.Filter)
            {
                case TransitionFilterLevel.All:
                    return true;
                
                case TransitionFilterLevel.TransitionPlusBase:
                    // If the property isn't defined on a type that inherits from Transition, drop it
                    if (!typeof(Transition).IsAssignableFrom(property.ComponentType))
                    {
                        return false;
                    }

                    // If the property is a dependency property, it must be owned by a type that inherits from Transition
                    if ((dProperty != null) && (!typeof(Transition).IsAssignableFrom(dProperty.DependencyProperty.OwnerType)))
                    {
                        return false;
                    }

                    // All conditions met
                    return true;
                
                case TransitionFilterLevel.TransitionSpecific:
                    // If the property isn't defined exactly on the component, drop it
                    if (property.ComponentType != componentType)
                    {
                        return false;
                    }

                    // If the property is a dependency property, it must be owned by the component
                    if ((dProperty != null) && (dProperty.DependencyProperty.OwnerType != componentType))
                    {
                        return false;
                    }

                    // All conditions met
                    return true;

                default:
                    return false;
            }
        }

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            // Placeholder
            var results = new Collection<PropertyDescriptor>();

            // Start with all
            var properties = TypeDescriptor.GetProperties(_component, attributes, true);

            // Add filtered
            foreach (PropertyDescriptor property in properties)
            {
                if (this.MatchesFilter(property))
                {
                    results.Add(property);
                }
            }
            
            return new PropertyDescriptorCollection(results.ToArray());
        }

        public override PropertyDescriptorCollection GetProperties()
        {
            var c = new PropertyDescriptorCollection(new PropertyDescriptor[] { TypeDescriptor.GetProperties(_component, true)[0] });
            return c;
        }

        public override AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(_component, true);
        }

        public override object GetPropertyOwner(PropertyDescriptor pd)
        {
            return _component;
        }

        /// <summary>
        /// Gets or sets the filter of the <see cref="TransitionFilterDescriptor"/>.
        /// </summary>
        /// <value>
        /// The filter of the <c>TransitionFilterDescriptor</c>.
        /// </value>
        public TransitionFilterLevel Filter { get; set; }
    }
}
