using System;
using System.Globalization;
using NUnit.Framework;

namespace JsonOrg
{
    [TestFixture()]
    public class JSONObjectTest
    {
        [Test()]
        public void Test_0000_EmptyJSON ()
        {
            JsonObject json = new JsonObject();
            Assert.AreEqual("{}", json.ToString());
        }
        
        [Test()]
        public void Test_0010_StringValue()
        {
            JsonObject json = new JsonObject("{\"id\":\"abcd\"}");
            Assert.AreEqual("{\"id\":\"abcd\"}", json.ToString());
            Assert.AreEqual("abcd", json.GetString("id"));
        }
        
        [Test()]
        public void Test_0011_IntValue()
        {
            JsonObject json = new JsonObject("{\"id\":1234}");
            Assert.AreEqual("{\"id\":1234}", json.ToString());
            Assert.AreEqual((int)1234, json.GetInt("id"));
        }
        
        [Test()]
        public void Test_0012_LongValue()
        {
            JsonObject json = new JsonObject("{\"id\":1234567890123}");
            Assert.AreEqual("{\"id\":1234567890123}", json.ToString());
            Assert.AreEqual((long)1234567890123, json.GetLong("id"));
        }
        
        [Test()]
        public void Test_0012_LongValue_small()
        {
            JsonObject json = new JsonObject("{\"id\":1234}");
            Assert.AreEqual("{\"id\":1234}", json.ToString());
            Assert.AreEqual((long)1234, json.GetLong("id"));
        }
        
        [Test()]
        public void Test_0013_doubleValue()
        {
            JsonObject json = new JsonObject("{\"id\":12345.6789}");
            Assert.AreEqual("{\"id\":12345.6789}", json.ToString());
            Assert.AreEqual((double)12345.6789, json.GetDouble("id"));
        }
        
        [Test()]
        public void Test_0014_boolValue_true()
        {
            JsonObject json = new JsonObject("{\"id\":true}");
            Assert.AreEqual("{\"id\":true}", json.ToString());
            Assert.AreEqual(true, json.GetBoolean("id"));
        }
        
        [Test()]
        public void Test_0015_boolValue_false()
        {
            JsonObject json = new JsonObject("{\"id\":false}");
            Assert.AreEqual("{\"id\":false}", json.ToString());
            Assert.AreEqual(false, json.GetBoolean("id"));
        }
        
        [Test()]
        public void Test_0016_JSONObjectValue_empty()
        {
            JsonObject json = new JsonObject("{\"id\":{}}");
            Assert.AreEqual("{\"id\":{}}", json.ToString());
            Assert.AreEqual("{}", json.GetJsonObject("id").ToString());
        }
        
        [Test()]
        public void Test_0017_JSONObjectValue_hasEntry()
        {
            JsonObject json = new JsonObject("{\"user\":{\"name\":\"kii\"}}");
            Assert.AreEqual("{\"user\":{\"name\":\"kii\"}}", json.ToString());
            Assert.AreEqual("{\"name\":\"kii\"}", json.GetJsonObject("user").ToString());
            JsonObject inner = json.GetJsonObject("user");
            Assert.AreEqual("kii", inner.GetString("name"));
        }
        
        [Test()]
        public void Test_0018_JSONArrayValue_empty()
        {
            JsonObject json = new JsonObject("{\"ids\":[]}");
            Assert.AreEqual("{\"ids\":[]}", json.ToString());
            Assert.AreEqual("[]", json.GetJsonArray("ids").ToString());
        }
        
        [Test()]
        public void Test_0019_JSONArrayValue_hasValue()
        {
            JsonObject json = new JsonObject("{\"ids\":[1234]}");
            Assert.AreEqual("{\"ids\":[1234]}", json.ToString());
            Assert.AreEqual("[1234]", json.GetJsonArray("ids").ToString());
            JsonArray inner = json.GetJsonArray("ids");
            Assert.AreEqual(1234, inner.GetInt(0));
        }
        
        [Test()]
        public void Test_0030_set_String()
        {
            JsonObject json = new JsonObject();
            json.Put("id", "abcd");
            Assert.AreEqual("{\"id\":\"abcd\"}", json.ToString());
            Assert.AreEqual("abcd", json.GetString("id"));
        }
        
        [Test()]
        public void Test_0031_set_Int()
        {
            JsonObject json = new JsonObject();
            json.Put("id", (int)1234);
            Assert.AreEqual("{\"id\":1234}", json.ToString());
            Assert.AreEqual(1234, json.GetInt("id"));
        }
        
        [Test()]
        public void Test_0032_set_Long()
        {
            JsonObject json = new JsonObject();
            json.Put("id", (long)123456789012345);
            Assert.AreEqual("{\"id\":123456789012345}", json.ToString());
            Assert.AreEqual((long)123456789012345, json.GetLong("id"));
        }
        
        [Test()]
        public void Test_0033_set_Double()
        {
            JsonObject json = new JsonObject();
            json.Put("id", (double)12345.678901234);
            Assert.AreEqual("{\"id\":12345.678901234}", json.ToString());
            Assert.AreEqual((double)12345.678901234, json.GetDouble("id"));
        }
        
        [Test()]
        public void Test_0034_set_true()
        {
            JsonObject json = new JsonObject();
            json.Put("id", true);
            Assert.AreEqual("{\"id\":true}", json.ToString());
            Assert.AreEqual(true, json.GetBoolean("id"));
        }
        
        [Test()]
        public void Test_0035_set_false()
        {
            JsonObject json = new JsonObject();
            json.Put("id", false);
            Assert.AreEqual("{\"id\":false}", json.ToString());
            Assert.AreEqual(false, json.GetBoolean("id"));
        }
        
        [Test()]
        public void Test_0036_set_JSONObject_empty()
        {
            JsonObject json = new JsonObject();
            JsonObject inner = new JsonObject();
            json.Put("user", inner);
            Assert.AreEqual("{\"user\":{}}", json.ToString());
            Assert.AreEqual("{}", json.GetJsonObject("user").ToString());
        }
        
        [Test()]
        public void Test_0037_set_JSONObject_hasValue()
        {
            JsonObject json = new JsonObject();
            JsonObject inner = new JsonObject();
            inner.Put("name", "kii");
            json.Put("user", inner);
            Assert.AreEqual("{\"user\":{\"name\":\"kii\"}}", json.ToString());
            Assert.AreEqual("{\"name\":\"kii\"}", json.GetJsonObject("user").ToString());
        }
        
        [Test()]
        public void Test_0038_set_JSONArray_empty()
        {
            JsonObject json = new JsonObject();
            JsonArray inner = new JsonArray();
            json.Put("ids", inner);
            Assert.AreEqual("{\"ids\":[]}", json.ToString());
            Assert.AreEqual("[]", json.GetJsonArray("ids").ToString());
        }
        
        [Test()]
        public void Test_0039_set_JSONArray_hasValue()
        {
            JsonObject json = new JsonObject();
            JsonArray inner = new JsonArray();
            inner.Put(1234);
            json.Put("ids", inner);
            Assert.AreEqual("{\"ids\":[1234]}", json.ToString());
            Assert.AreEqual("[1234]", json.GetJsonArray("ids").ToString());
        }
        
        #region set_MAX_VALUE
        [Test()]
        public void Test_0100_IntValue_MAX()
        {
            JsonObject json = new JsonObject();
            json.Put("value", int.MaxValue);
            Assert.AreEqual("{\"value\":" + int.MaxValue + "}", json.ToString());
            Assert.AreEqual(int.MaxValue, json.GetInt("value"));
        }
        
        [Test()]
        public void Test_0101_LongValue_MAX()
        {
            JsonObject json = new JsonObject();
            json.Put("value", long.MaxValue);
            Assert.AreEqual("{\"value\":" + long.MaxValue + "}", json.ToString());
            Assert.AreEqual(long.MaxValue, json.GetLong("value"));
        }       
        
        [Test()]
        public void Test_0102_DoubleValue_MAX()
        {
            JsonObject json = new JsonObject();
            json.Put("value", double.MaxValue);
            Assert.AreEqual("{\"value\":1.7976931348623157E+308}", json.ToString());
            Assert.AreEqual(double.MaxValue, json.GetDouble("value"));
        }                    
        #endregion
        
        #region set_MIN_VALUE
        [Test()]
        public void Test_0110_IntValue_MIN()
        {
            JsonObject json = new JsonObject();
            json.Put("value", int.MinValue);
            Assert.AreEqual("{\"value\":" + int.MinValue + "}", json.ToString());
            Assert.AreEqual(int.MinValue, json.GetInt("value"));
        }
        
        [Test()]
        public void Test_0111_LongValue_MIN()
        {
            JsonObject json = new JsonObject();
            json.Put("value", long.MinValue);
            Assert.AreEqual("{\"value\":" + long.MinValue + "}", json.ToString());
            Assert.AreEqual(long.MinValue, json.GetLong("value"));
        }       
        
        [Test()]
        public void Test_0112_DoubleValue_MIN()
        {
            JsonObject json = new JsonObject();
            json.Put("value", double.MinValue);
            Assert.AreEqual("{\"value\":-1.7976931348623157E+308}", json.ToString());
            Assert.AreEqual(double.MinValue, json.GetDouble("value"));
        }              
        #endregion
        
        #region CAST
        [Test()]
        public void Test_0101_int_to_long()
        {
            JsonObject json = new JsonObject();
            json.Put("f", 10);
            Assert.AreEqual(10L, json.GetLong("f"));
        }
        [Test()]
        public void Test_0102_int_to_double()
        {
            JsonObject json = new JsonObject();
            json.Put("f", 10);
            Assert.AreEqual(10.00, json.GetDouble("f"));
        }
        [Test(), ExpectedException(typeof(JsonException))]
        public void Test_0103_int_to_bool()
        {
            JsonObject json = new JsonObject();
            json.Put("f", 10);
            json.GetBoolean("f");
        }
        [Test()]
        public void Test_0104_int_to_string()
        {
            JsonObject json = new JsonObject();
            json.Put("f", 10);
            Assert.AreEqual("10", json.GetString("f"));
        }
        [Test()]
        public void Test_0105_long_to_int()
        {
            JsonObject json = new JsonObject();
            json.Put("f", 10L);
            Assert.AreEqual(10, json.GetInt("f"));
        }
        [Test()]
        public void Test_0106_long_to_double()
        {
            JsonObject json = new JsonObject();
            json.Put("f", 10L);
            Assert.AreEqual(10.00, json.GetDouble("f"));
        }
        [Test(), ExpectedException(typeof(JsonException))]
        public void Test_0107_long_to_bool()
        {
            JsonObject json = new JsonObject();
            json.Put("f", 10L);
            json.GetBoolean("f");
        }
        [Test()]
        public void Test_0108_long_to_string()
        {
            JsonObject json = new JsonObject();
            json.Put("f", 10L);
            Assert.AreEqual("10", json.GetString("f"));
        }
        [Test()]
        public void Test_0109_double_to_int()
        {
            JsonObject json = new JsonObject();
            json.Put("f", 10.00);
            Assert.AreEqual(10, json.GetInt("f"));
        }
        [Test()]
        public void Test_0110_double_to_long()
        {
            JsonObject json = new JsonObject();
            json.Put("f", 10.00);
            Assert.AreEqual(10L, json.GetLong("f"));
        }
        [Test(), ExpectedException(typeof(JsonException))]
        public void Test_0111_double_to_bool()
        {
            JsonObject json = new JsonObject();
            json.Put("f", 10.00);
            json.GetBoolean("f");
        }
        [Test()]
        public void Test_0112_double_to_string()
        {
            JsonObject json = new JsonObject();
            json.Put("f", 10.01);
            Assert.AreEqual("10.01", json.GetString("f"));
        }
        [Test(), ExpectedException(typeof(JsonException))]
        public void Test_0113_bool_to_int()
        {
            JsonObject json = new JsonObject();
            json.Put("f", true);
            json.GetInt("f");
        }
        [Test(), ExpectedException(typeof(JsonException))]
        public void Test_0114_bool_to_long()
        {
            JsonObject json = new JsonObject();
            json.Put("f", true);
            json.GetLong("f");
        }
        [Test(), ExpectedException(typeof(JsonException))]
        public void Test_0115_bool_to_double()
        {
            JsonObject json = new JsonObject();
            json.Put("f", true);
            json.GetDouble("f");
        }
        [Test()]
        public void Test_0116_bool_to_string()
        {
            JsonObject json = new JsonObject();
            json.Put("f", true);
            Assert.AreEqual("true", json.GetString("f"));
        }
        [Test()]
        public void Test_0117_string_to_int()
        {
            JsonObject json = new JsonObject();
            json.Put("f", "10.01");
            Assert.AreEqual(10, json.GetInt("f"));
        }
        [Test()]
        public void Test_0118_string_to_long()
        {
            JsonObject json = new JsonObject();
            json.Put("f", "10.01");
            Assert.AreEqual(10L, json.GetLong("f"));
        }
        [Test()]
        public void Test_0119_string_to_double()
        {
            JsonObject json = new JsonObject();
            json.Put("f", "10.01");
            Assert.AreEqual(10.01, json.GetDouble("f"));
        }
        [Test(), ExpectedException(typeof(JsonException))]
        public void Test_0120_string_to_bool()
        {
            JsonObject json = new JsonObject();
            json.Put("f", "10.01");
            json.GetBoolean("f");
        }
        [Test()]
        public void Test_0121_invalid_json_object()
        {
            string value = "<xml>hoge</xml>";
            try
            {
                new JsonObject(value);
                Assert.Fail("JsonException was not thrown.");
            }
            catch (JsonException e)
            {
                Assert.IsTrue(e.Message.Contains(value));
            }
        }
        [Test()]
        public void Test_0122_invalid_json_array()
        {
            string value = "<xml>hoge</xml>";
            try
            {
                new JsonArray(value);
                Assert.Fail("JsonException was not thrown.");
            }
            catch (JsonException e)
            {
                Assert.IsTrue(e.Message.Contains(value));
            }
        }

        #endregion
    }
}
