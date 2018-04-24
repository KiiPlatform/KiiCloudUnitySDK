using System;

namespace KiiCorp.Cloud.Storage
{
    [AttributeUsage(AttributeTargets.Method)]
    public class KiiUTInfoAttribute : Attribute
    {
        public string action;
        public string expected;
        public KiiUTInfoAttribute ()
        {
        }
    }
}

