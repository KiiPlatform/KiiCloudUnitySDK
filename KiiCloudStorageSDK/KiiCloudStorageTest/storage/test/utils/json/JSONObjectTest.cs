using System;
using NUnit.Framework;

using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class JsonObjectTest
    {
        [Test(), KiiUTInfo(
            action = "When we create empty JsonObject,",
            expected = "We can get {} by calling ToString()"
            )]
        public void Test_0000_EmptyJSON ()
        {
            JsonObject json = new JsonObject();
            Assert.AreEqual("{}", json.ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we create JsonObject with string value,",
            expected = "We can get string value"
            )]
        public void Test_0010_StringValue()
        {
            JsonObject json = new JsonObject("{\"id\":\"abcd\"}");
            Assert.AreEqual("{\"id\":\"abcd\"}", json.ToString());
            Assert.AreEqual("abcd", json.GetString("id"));
        }

        [Test(), KiiUTInfo(
            action = "When we create JsonObject with int value,",
            expected = "We can get int value"
            )]
        public void Test_0011_IntValue()
        {
            JsonObject json = new JsonObject("{\"id\":1234}");
            Assert.AreEqual("{\"id\":1234}", json.ToString());
            Assert.AreEqual((int)1234, json.GetInt("id"));
        }

        [Test(), KiiUTInfo(
            action = "When we create JsonObject with long value,",
            expected = "We can get long value"
            )]
        public void Test_0012_LongValue()
        {
            JsonObject json = new JsonObject("{\"id\":1234567890123}");
            Assert.AreEqual("{\"id\":1234567890123}", json.ToString());
            Assert.AreEqual((long)1234567890123, json.GetLong("id"));
        }

        [Test(), KiiUTInfo(
            action = "When we create JsonObject with small long value,",
            expected = "We can get long value"
            )]
        public void Test_0012_LongValue_small()
        {
            JsonObject json = new JsonObject("{\"id\":1234}");
            Assert.AreEqual("{\"id\":1234}", json.ToString());
            Assert.AreEqual((long)1234, json.GetLong("id"));
        }

        [Test(), KiiUTInfo(
            action = "When we create JsonObject with double value,",
            expected = "We can get double value"
            )]
        public void Test_0013_doubleValue()
        {
            JsonObject json = new JsonObject("{\"id\":12345.6789}");
            Assert.AreEqual("{\"id\":12345.6789}", json.ToString());
            Assert.AreEqual((double)12345.6789, json.GetDouble("id"));
        }

        [Test(), KiiUTInfo(
            action = "When we create JsonObject with bool(true) value,",
            expected = "We can get bool(true) value"
            )]
        public void Test_0014_boolValue_true()
        {
            JsonObject json = new JsonObject("{\"id\":true}");
            Assert.AreEqual("{\"id\":true}", json.ToString());
            Assert.AreEqual(true, json.GetBoolean("id"));
        }

        [Test(), KiiUTInfo(
            action = "When we create JsonObject with bool(false) value,",
            expected = "We can get bool(false) value"
            )]
        public void Test_0015_boolValue_false()
        {
            JsonObject json = new JsonObject("{\"id\":false}");
            Assert.AreEqual("{\"id\":false}", json.ToString());
            Assert.AreEqual(false, json.GetBoolean("id"));
        }

        [Test(), KiiUTInfo(
            action = "When we create JsonObject with empty JsonObject value,",
            expected = "We can get empty JsonObject value"
            )]
        public void Test_0016_JsonObjectValue_empty()
        {
            JsonObject json = new JsonObject("{\"id\":{}}");
            Assert.AreEqual("{\"id\":{}}", json.ToString());
            Assert.AreEqual("{}", json.GetJsonObject("id").ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we create JsonObject with JsonObject value,",
            expected = "We can get JsonObject value"
            )]
        public void Test_0017_JsonObjectValue_hasEntry()
        {
            JsonObject json = new JsonObject("{\"user\":{\"name\":\"kii\"}}");
            Assert.AreEqual("{\"user\":{\"name\":\"kii\"}}", json.ToString());
            Assert.AreEqual("{\"name\":\"kii\"}", json.GetJsonObject("user").ToString());
            JsonObject inner = json.GetJsonObject("user");
            Assert.AreEqual("kii", inner.GetString("name"));
        }

        [Test(), KiiUTInfo(
            action = "When we create JsonObject with empty JsonArray value,",
            expected = "We can get empty JsonArray value"
            )]
        public void Test_0018_JsonArrayValue_empty()
        {
            JsonObject json = new JsonObject("{\"ids\":[]}");
            Assert.AreEqual("{\"ids\":[]}", json.ToString());
            Assert.AreEqual("[]", json.GetJsonArray("ids").ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we create JsonObject with JsonArray value,",
            expected = "We can get JsonArray value"
            )]
        public void Test_0019_JsonArrayValue_hasValue()
        {
            JsonObject json = new JsonObject("{\"ids\":[1234]}");
            Assert.AreEqual("{\"ids\":[1234]}", json.ToString());
            Assert.AreEqual("[1234]", json.GetJsonArray("ids").ToString());
            JsonArray inner = json.GetJsonArray("ids");
            Assert.AreEqual(1234, inner.GetInt(0));
        }

        [Test(), KiiUTInfo(
            action = "When we set string value,",
            expected = "We can get string value"
            )]
        public void Test_0030_set_String()
        {
            JsonObject json = new JsonObject();
            json.Put("id", "abcd");
            Assert.AreEqual("{\"id\":\"abcd\"}", json.ToString());
            Assert.AreEqual("abcd", json.GetString("id"));
        }

        [Test(), KiiUTInfo(
            action = "When we set int value,",
            expected = "We can get int value"
            )]
        public void Test_0031_set_Int()
        {
            JsonObject json = new JsonObject();
            json.Put("id", (int)1234);
            Assert.AreEqual("{\"id\":1234}", json.ToString());
            Assert.AreEqual(1234, json.GetInt("id"));
        }

        [Test(), KiiUTInfo(
            action = "When we set long value,",
            expected = "We can get long value"
            )]
        public void Test_0032_set_Long()
        {
            JsonObject json = new JsonObject();
            json.Put("id", (long)123456789012345);
            Assert.AreEqual("{\"id\":123456789012345}", json.ToString());
            Assert.AreEqual((long)123456789012345, json.GetLong("id"));
        }

        [Test(), KiiUTInfo(
            action = "When we set double value,",
            expected = "We can get double value"
            )]
        public void Test_0033_set_Double()
        {
            JsonObject json = new JsonObject();
            json.Put("id", (double)12345.678901234);
            Assert.AreEqual("{\"id\":12345.678901234}", json.ToString());
            Assert.AreEqual((double)12345.678901234, json.GetDouble("id"));
        }

        [Test(), KiiUTInfo(
            action = "When we set bool(true) value,",
            expected = "We can get bool(true) value"
            )]
        public void Test_0034_set_true()
        {
            JsonObject json = new JsonObject();
            json.Put("id", true);
            Assert.AreEqual("{\"id\":true}", json.ToString());
            Assert.AreEqual(true, json.GetBoolean("id"));
        }

        [Test(), KiiUTInfo(
            action = "When we set bool(false) value,",
            expected = "We can get bool(false) value"
            )]
        public void Test_0035_set_false()
        {
            JsonObject json = new JsonObject();
            json.Put("id", false);
            Assert.AreEqual("{\"id\":false}", json.ToString());
            Assert.AreEqual(false, json.GetBoolean("id"));
        }

        [Test(), KiiUTInfo(
            action = "When we set empty JsonObject value,",
            expected = "We can get empty JsonObject value"
            )]
        public void Test_0036_set_JsonObject_empty()
        {
            JsonObject json = new JsonObject();
            JsonObject inner = new JsonObject();
            json.Put("user", inner);
            Assert.AreEqual("{\"user\":{}}", json.ToString());
            Assert.AreEqual("{}", json.GetJsonObject("user").ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we set JsonObject value,",
            expected = "We can get JsonObject value"
            )]
        public void Test_0037_set_JsonObject_hasValue()
        {
            JsonObject json = new JsonObject();
            JsonObject inner = new JsonObject();
            inner.Put("name", "kii");
            json.Put("user", inner);
            Assert.AreEqual("{\"user\":{\"name\":\"kii\"}}", json.ToString());
            Assert.AreEqual("{\"name\":\"kii\"}", json.GetJsonObject("user").ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we set empty JsonArray value,",
            expected = "We can get empty JsonArray value"
            )]
        public void Test_0038_set_JsonArray_empty()
        {
            JsonObject json = new JsonObject();
            JsonArray inner = new JsonArray();
            json.Put("ids", inner);
            Assert.AreEqual("{\"ids\":[]}", json.ToString());
            Assert.AreEqual("[]", json.GetJsonArray("ids").ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we set JsonArray value,",
            expected = "We can get JsonArray value"
            )]
        public void Test_0039_set_JsonArray_hasValue()
        {
            JsonObject json = new JsonObject();
            JsonArray inner = new JsonArray();
            inner.Put(1234);
            json.Put("ids", inner);
            Assert.AreEqual("{\"ids\":[1234]}", json.ToString());
            Assert.AreEqual("[1234]", json.GetJsonArray("ids").ToString());
        }

    }
}

