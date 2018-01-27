using System;
using System.ComponentModel;

namespace YanZhiwei.DotNet2.Utilities.WinForm
{
    /// <summary>
    /// PropertyGrid 读取配置Section节点包装器
    /// </summary>
    /// <seealso cref="System.ComponentModel.Component" />
    /// <seealso cref="System.ComponentModel.ICustomTypeDescriptor" />
    [TypeConverter(typeof(ExpandableObjectConverter))]
    class XmlSectionWrapper : Component, ICustomTypeDescriptor
    {
        private PropertyDescriptorCollection propertyCollection;

        private int maxLength;

        public int MaxLength
        {
            get
            {
                return maxLength;
            }
            set
            {
                if (value > maxLength)
                    maxLength = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSectionWrapper"/> class.
        /// </summary>
        public XmlSectionWrapper()
        {
            propertyCollection = new PropertyDescriptorCollection(new PropertyDescriptor[] { });
        }

        /// <summary>
        /// Adds the property.
        /// </summary>
        /// <param name="propName">Name of the property.</param>
        /// <param name="propValue">The property value.</param>
        /// <param name="propDesc">The property desc.</param>
        /// <param name="propCat">The property cat.</param>
        /// <param name="propType">Type of the property.</param>
        /// <param name="isReadOnly">if set to <c>true</c> [is read only].</param>
        /// <param name="isExpandable">if set to <c>true</c> [is expandable].</param>
        public void AddProperty(string propName, object propValue, string propDesc,
            string propCat, Type propType, bool isReadOnly, bool isExpandable)
        {
            DynamicProperty _property = new DynamicProperty(propName, propValue, propDesc, propCat,
                propType, isReadOnly, isExpandable);
            propertyCollection.Add(_property);
            this.MaxLength = propName.Length;
            this.MaxLength = propValue.ToString().Length;
        }

        /// <summary>
        /// Gets the <see cref="DynamicProperty"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="DynamicProperty"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public DynamicProperty this[int index]
        {
            get
            {
                return (DynamicProperty)propertyCollection[index];
            }
        }

        /// <summary>
        /// Gets the <see cref="DynamicProperty"/> with the specified name.
        /// </summary>
        /// <value>
        /// The <see cref="DynamicProperty"/>.
        /// </value>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public DynamicProperty this[string name]
        {
            get
            {
                return (DynamicProperty)propertyCollection[name];
            }
        }

        /// <summary>
        /// Returns the class name of this instance of a component.
        /// </summary>
        /// <returns>
        /// The class name of the object, or null if the class does not have a name.
        /// </returns>
        public string GetClassName()
        {
            return (TypeDescriptor.GetClassName(this, true));
        }

        /// <summary>
        /// Returns a collection of custom attributes for this instance of a component.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.ComponentModel.AttributeCollection" /> containing the attributes for this object.
        /// </returns>
        public AttributeCollection GetAttributes()
        {
            return (TypeDescriptor.GetAttributes(this, true));
        }

        /// <summary>
        /// Returns the name of this instance of a component.
        /// </summary>
        /// <returns>
        /// The name of the object, or null if the object does not have a name.
        /// </returns>
        public string GetComponentName()
        {
            return (TypeDescriptor.GetComponentName(this, true));
        }

        /// <summary>
        /// Returns a type converter for this instance of a component.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.TypeConverter" /> that is the converter for this object, or null if there is no <see cref="T:System.ComponentModel.TypeConverter" /> for this object.
        /// </returns>
        public TypeConverter GetConverter()
        {
            return (TypeDescriptor.GetConverter(this, true));
        }

        /// <summary>
        /// Returns the default event for this instance of a component.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.ComponentModel.EventDescriptor" /> that represents the default event for this object, or null if this object does not have events.
        /// </returns>
        public EventDescriptor GetDefaultEvent()
        {
            return (TypeDescriptor.GetDefaultEvent(this, true));
        }

        /// <summary>
        /// Returns the default property for this instance of a component.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.PropertyDescriptor" /> that represents the default property for this object, or null if this object does not have properties.
        /// </returns>
        public PropertyDescriptor GetDefaultProperty()
        {
            PropertyDescriptorCollection props = GetAllProperties();

            if (props.Count > 0)
                return (props[0]);
            else
                return (null);
        }

        /// <summary>
        /// Returns an editor of the specified type for this instance of a component.
        /// </summary>
        /// <param name="editorBaseType">A <see cref="T:System.Type" /> that represents the editor for this object.</param>
        /// <returns>
        /// An <see cref="T:System.Object" /> of the specified type that is the editor for this object, or null if the editor cannot be found.
        /// </returns>
        public object GetEditor(Type editorBaseType)
        {
            return (TypeDescriptor.GetEditor(this, editorBaseType, true));
        }

        /// <summary>
        /// Returns the events for this instance of a component using the specified attribute array as a filter.
        /// </summary>
        /// <param name="attributes">An array of type <see cref="T:System.Attribute" /> that is used as a filter.</param>
        /// <returns>
        /// An <see cref="T:System.ComponentModel.EventDescriptorCollection" /> that represents the filtered events for this component instance.
        /// </returns>
        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return (TypeDescriptor.GetEvents(this, attributes, true));
        }

        /// <summary>
        /// Returns the events for this instance of a component.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.ComponentModel.EventDescriptorCollection" /> that represents the events for this component instance.
        /// </returns>
        public EventDescriptorCollection GetEvents()
        {
            return (TypeDescriptor.GetEvents(this, true));
        }

        /// <summary>
        /// Returns the properties for this instance of a component using the attribute array as a filter.
        /// </summary>
        /// <param name="attributes">An array of type <see cref="T:System.Attribute" /> that is used as a filter.</param>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that represents the filtered properties for this component instance.
        /// </returns>
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return (GetAllProperties());
        }

        /// <summary>
        /// Returns the properties for this instance of a component.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that represents the properties for this component instance.
        /// </returns>
        public PropertyDescriptorCollection GetProperties()
        {
            return (GetAllProperties());
        }

        /// <summary>
        /// Returns an object that contains the property described by the specified property descriptor.
        /// </summary>
        /// <param name="pd">A <see cref="T:System.ComponentModel.PropertyDescriptor" /> that represents the property whose owner is to be found.</param>
        /// <returns>
        /// An <see cref="T:System.Object" /> that represents the owner of the specified property.
        /// </returns>
        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return (this);
        }

        /// <summary>
        /// Gets all properties.
        /// </summary>
        /// <returns></returns>
        private PropertyDescriptorCollection GetAllProperties()
        {
            return propertyCollection;
        }
    }
}