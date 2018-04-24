using System;
using System.Globalization;
using NUnit.Framework;

namespace JsonOrg
{
    [TestFixture()]
    public class TestJsonObject_constructor
    {
        [Test()]
        public void Test_0000_EmptyJSON ()
        {
            JsonObject json = new JsonObject("{}");
            Assert.AreEqual("{}", json.ToString());
        }
         
        [Test()]
        public void Test_0001_field_1 ()
        {
            JsonObject json = new JsonObject("{\"key\":\"value\"}");
            Assert.AreEqual("{\"key\":\"value\"}", json.ToString());
        }
        
        [Test(), ExpectedException(typeof(JsonException))]
        public void Test_0002_no_end_object ()
        {
            new JsonObject("{\"key\":\"value\"");
        }
         
        [Test(), ExpectedException(typeof(JsonException))]
        public void Test_0003_no_end_string ()
        {
            new JsonObject("{\"key\":\"value");
        }

        [Test(), ExpectedException(typeof(JsonException))]
        public void Test_0004_no_value ()
        {
            new JsonObject("{\"key\":");
        }
        
        [Test(), ExpectedException(typeof(JsonException))]
        public void Test_0005_no_value_separator ()
        {
            new JsonObject("{\"key\"");
        }
        
        [Test(), ExpectedException(typeof(JsonException))]
        public void Test_0006_no_end_key ()
        {
            new JsonObject("{\"key");
        }   
        
        [Test(), ExpectedException(typeof(JsonException))]
        public void Test_0007_no_key ()
        {
            new JsonObject("{\"");
        }      
        
        [Test(), ExpectedException(typeof(JsonException))]
        public void Test_0008_no_start_quot ()
        {
            new JsonObject("{");
        }              
        
        [Test(), ExpectedException(typeof(JsonException))]
        public void Test_0010_string ()
        {
            new JsonObject("\"abc\"");
        }          
        
        [Test(), ExpectedException(typeof(JsonException))]
        public void Test_0010_string_no_end ()
        {
            new JsonObject("\"abc");
        }    
        
        [Test(), ExpectedException(typeof(JsonException))]
        public void Test_0011_string_start ()
        {
            new JsonObject("\"");
        }    
        
        [Test(), ExpectedException(typeof(JsonException))]
        public void Test_0020_number ()
        {
            new JsonObject("123");
        }          
        
        [Test(), ExpectedException(typeof(JsonException))]
        public void Test_0030_empty_string ()
        {
            new JsonObject("");
        }          

    }
}

