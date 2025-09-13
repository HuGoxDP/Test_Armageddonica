#if UNITY_EDITOR
using UnityEngine;

namespace _Project.Scripts.Editor
{
    public class RestrictedEnumAttribute : PropertyAttribute
    {
        public System.Type EnumType { get; private set; }
        public string[] AllowedValues { get; private set; }
    
        public RestrictedEnumAttribute(System.Type enumType, params string[] allowedValues)
        {
            EnumType = enumType;
            AllowedValues = allowedValues;
        }
    }
}
#endif