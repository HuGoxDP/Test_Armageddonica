using UnityEditor;

namespace _Project.Scripts.Editor
{
    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(RestrictedEnumAttribute))]
    public class RestrictedEnumDrawer : PropertyDrawer
    {
        public override void OnGUI(UnityEngine.Rect position, SerializedProperty property,
            UnityEngine.GUIContent label)
        {
            RestrictedEnumAttribute restrictedEnum = (RestrictedEnumAttribute)attribute;

            if (property.propertyType == SerializedPropertyType.Enum)
            {
                int currentValue = property.enumValueIndex;

                int currentIndex = -1;
                for (int i = 0; i < restrictedEnum.AllowedValues.Length; i++)
                {
                    if (property.enumNames[i] == restrictedEnum.AllowedValues[i])
                    {
                        currentIndex = i;
                        break;
                    }
                }
                
                if (currentIndex == -1)
                {
                    currentIndex = 0;
                    
                    for (int i = 0; i < property.enumNames.Length; i++)
                    {
                        if (property.enumNames[i] == restrictedEnum.AllowedValues[0])
                        {
                            property.enumValueIndex = i;
                            break;
                        }
                    }
                }
                
                int newIndex = EditorGUI.Popup(position, label.text, currentIndex, restrictedEnum.AllowedValues);

                if (newIndex != currentValue)
                {
                    for (int i = 0; i < property.enumNames.Length; i++)
                    {
                        if (property.enumNames[i] == restrictedEnum.AllowedValues[newIndex])
                        {
                            property.enumValueIndex = i;
                            break;
                        }
                    }
                }
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }

#endif
}