using System;
using UnityEngine;

namespace Stacker.Extensions.Attributes
{

    /// <summary>
    /// Shows the field if the conditional statement is true.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Struct, Inherited = true)]
    public sealed class EnumShowFieldAttribute : PropertyAttribute
    {

        /// <summary>
        /// The name of the field that will be in control.
        /// </summary>
        public string sourceField = "";

        /// <summary>
        /// Whenever the value of the field is equal to this, the property is shown.
        /// </summary>
        public int showInInspector;

        public EnumShowFieldAttribute(string sourceField, int showInInspector)
        {
            this.sourceField     = sourceField;
            this.showInInspector = showInInspector;
        }

        public bool IsFieldShown(int sourceFieldValue)
        {
            return (showInInspector & sourceFieldValue) == sourceFieldValue;
        }

    }

}
