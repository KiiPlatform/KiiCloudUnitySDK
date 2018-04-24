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

    [AttributeUsage(AttributeTargets.Class)]
    public class TestSpec : Attribute
    {
        public string url;
        public TestSpec ()
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class TestCaseNumber : Attribute
    {
        string number;
        public TestCaseNumber (string number)
        {
            this.number = number;
        }
    }

}

