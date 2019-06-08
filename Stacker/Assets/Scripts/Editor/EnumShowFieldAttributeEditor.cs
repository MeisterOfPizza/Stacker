using Stacker.Extensions.Attributes;
using UnityEditor;
using UnityEngine;

namespace Stacker.Editor
{

    [CustomPropertyDrawer(typeof(EnumShowFieldAttribute))]
    public sealed class EnumShowFieldAttributeEditor : PropertyDrawer
    {

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            EnumShowFieldAttribute showField = (EnumShowFieldAttribute)attribute;
            bool show = GetEnumShowFieldAttributeResult(showField, property);

            // If the property should be shown, return the required height, else, undo the vertical spacing added as a standard before and after the property.
            return show ? EditorGUI.GetPropertyHeight(property, label) : -EditorGUIUtility.standardVerticalSpacing;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Get the attribute:
            EnumShowFieldAttribute showField = (EnumShowFieldAttribute)attribute;

            bool shown = GetEnumShowFieldAttributeResult(showField, property);

            // Check if the GUI was enabled.
            bool wasEnabled = GUI.enabled;

            // Hide/show the GUI.
            GUI.enabled = shown;

            if (shown)
            {
                // If it's not hidden, then draw the property.
                EditorGUI.PropertyField(position, property, label, true);
            }

            // Reenable/disable the GUI.
            GUI.enabled = wasEnabled;
        }

        private bool GetEnumShowFieldAttributeResult(EnumShowFieldAttribute showFieldAttribute, SerializedProperty property)
        {
            string propertyPath = property.propertyPath; // Get the path of the current property (with the attribute on).
            string conditionPath = propertyPath.Replace(property.name, showFieldAttribute.sourceField); // Find the path to the property that will act as the conditional statement.
            SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath); // Get the property.

            return showFieldAttribute.IsFieldShown(sourcePropertyValue.intValue); // Check if the value is correct or not.
        }

    }

}
