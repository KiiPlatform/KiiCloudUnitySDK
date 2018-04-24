using System;
using System.Reflection;

namespace KiiCorp.Cloud.Storage
{
    public static class SDKTestHack
    {
        public static void SetField(object instance, string fieldName, object newValue)
        {
            Type type = instance.GetType();
            FieldInfo fieldInfo = type.GetField(fieldName,  BindingFlags.Instance | BindingFlags.NonPublic);
            fieldInfo.SetValue(instance, newValue);
        }

        public static Object GetField (object instance, string fieldName)
        {
            Type type = instance.GetType ();
            FieldInfo fieldInfo = type.GetField (fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            return fieldInfo.GetValue (instance);
        }
    }
}
