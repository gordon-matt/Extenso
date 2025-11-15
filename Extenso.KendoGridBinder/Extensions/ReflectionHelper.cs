using System.Reflection;

namespace Extenso.KendoGridBinder.Extensions;

/// <summary>
/// http://dotnetfollower.com/wordpress/2012/12/c-how-to-set-or-get-value-of-a-private-or-internal-property-through-the-reflection/
/// </summary>
public static class ReflectionHelper
{
    extension(object obj)
    {
        public object GetFieldValue(string fieldName)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            var objType = obj.GetType();
            var fieldInfo = GetFieldInfo(objType, fieldName);
            return fieldInfo == null
                ? throw new ArgumentOutOfRangeException(nameof(fieldName), $"Couldn't find field {fieldName} in type {objType.FullName}")
                : fieldInfo.GetValue(obj);
        }

        public void SetFieldValue(string fieldName, object val)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            var objType = obj.GetType();
            var fieldInfo = GetFieldInfo(objType, fieldName);

            if (fieldInfo == null)
                throw new ArgumentOutOfRangeException(nameof(fieldName), $"Couldn't find field {fieldName} in type {objType.FullName}");

            fieldInfo.SetValue(obj, val);
        }
    }

    private static FieldInfo GetFieldInfo(Type type, string fieldName)
    {
        FieldInfo fieldInfo;
        do
        {
            fieldInfo = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            type = type.GetTypeInfo().BaseType;
        }
        while (fieldInfo == null && type != null);

        return fieldInfo;
    }
}