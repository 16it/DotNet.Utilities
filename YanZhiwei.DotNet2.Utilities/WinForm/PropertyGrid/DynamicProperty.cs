using System;
using System.ComponentModel;

namespace YanZhiwei.DotNet2.Utilities.WinForm
{
    /// <summary>
    /// PropertyGrid动态属性
    /// </summary>
    /// <seealso cref="System.ComponentModel.PropertyDescriptor" />
    class DynamicProperty : PropertyDescriptor
    {
        private string propName;
        private object propValue;
        private string propDescription;
        private string propCategory;
        private Type propType;
        private bool isReadOnly;
        private bool isExpandable;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicProperty"/> class.
        /// </summary>
        /// <param name="pName">Name of the p.</param>
        /// <param name="pValue">The p value.</param>
        /// <param name="pDesc">The p desc.</param>
        /// <param name="pCat">The p cat.</param>
        /// <param name="pType">Type of the p.</param>
        /// <param name="readOnly">if set to <c>true</c> [read only].</param>
        /// <param name="expandable">if set to <c>true</c> [expandable].</param>
        public DynamicProperty(string pName, object pValue, string pDesc, string pCat, Type pType, bool readOnly, bool expandable) : base(pName, new Attribute[] { })
        {
            propName = pName;
            propValue = pValue;
            propDescription = pDesc;
            propCategory = pCat;
            propType = pType;
            isReadOnly = readOnly;
            isExpandable = expandable;
        }

        /// <summary>
        /// When overridden in a derived class, gets the type of the component this property is bound to.
        /// </summary>
        public override Type ComponentType
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the name of the category to which the member belongs, as specified in the <see cref="T:System.ComponentModel.CategoryAttribute" />.
        /// </summary>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
        /// </PermissionSet>
        public override string Category
        {
            get
            {
                return propCategory;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether this property is read-only.
        /// </summary>
        public override bool IsReadOnly
        {
            get
            {
                return isReadOnly;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets the type of the property.
        /// </summary>
        public override Type PropertyType
        {
            get
            {
                return propType;
            }
        }

        /// <summary>
        /// When overridden in a derived class, returns whether resetting an object changes its value.
        /// </summary>
        /// <param name="component">The component to test for reset capability.</param>
        /// <returns>
        /// true if resetting the component changes its value; otherwise, false.
        /// </returns>
        public override bool CanResetValue(object component)
        {
            return true;
        }

        /// <summary>
        /// When overridden in a derived class, gets the current value of the property on a component.
        /// </summary>
        /// <param name="component">The component with the property for which to retrieve the value.</param>
        /// <returns>
        /// The value of a property for a given component.
        /// </returns>
        public override object GetValue(object component)
        {
            return propValue;
        }

        /// <summary>
        /// When overridden in a derived class, sets the value of the component to a different value.
        /// </summary>
        /// <param name="component">The component with the property value that is to be set.</param>
        /// <param name="value">The new value.</param>
        public override void SetValue(object component, object value)
        {
            propValue = value;
        }

        /// <summary>
        /// When overridden in a derived class, resets the value for this property of the component to the default value.
        /// </summary>
        /// <param name="component">The component with the property value that is to be reset to the default value.</param>
        public override void ResetValue(object component)
        {
            propValue = null;
        }

        /// <summary>
        /// When overridden in a derived class, determines a value indicating whether the value of this property needs to be persisted.
        /// </summary>
        /// <param name="component">The component with the property to be examined for persistence.</param>
        /// <returns>
        /// true if the property should be persisted; otherwise, false.
        /// </returns>
        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        /// <summary>
        /// Gets the description of the member, as specified in the <see cref="T:System.ComponentModel.DescriptionAttribute" />.
        /// </summary>
        public override string Description
        {
            get
            {
                return propDescription;
            }
        }
    }
}