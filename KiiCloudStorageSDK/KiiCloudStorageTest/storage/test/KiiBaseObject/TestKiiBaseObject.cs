using System;
using NUnit.Framework;
using System.Collections.Generic;

using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiBaseObject
    {
        #region int value
        [Test(), KiiUTInfo(
            action = "When we set int value to KiiObject,",
            expected = "We can get it as int value"
            )]
        public void Test_0000_int ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["score"] = 100;

            Assert.AreEqual("{\"score\":100}", obj.mJSON.ToString());
            Assert.AreEqual("{\"score\":100}", obj.mJSONPatch.ToString());

            Assert.AreEqual(100, obj["score"]);
            Assert.AreEqual(100, obj.GetInt("score"));
        }

        [Test(), ExpectedException(typeof(ArgumentNullException)), KiiUTInfo(
            action = "When we call GetInt() with null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0001_int_null ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["score"] = 100;

            obj.GetInt(null);
        }

        [Test(), KiiUTInfo(
            action = "When we call GetInt() with unknown key and fallback,",
            expected = "fallback value must be returned"
            )]
        public void Test_0010_int_fallback ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["score"] = 100;

            Assert.AreEqual(100, obj.GetInt("score", -1));
            Assert.AreEqual(-1, obj.GetInt("scoreDummy", -1));
        }

        [Test(), KiiUTInfo(
            action = "When we call GetInt() with null key and fallback,",
            expected = "fallback value must be returned"
            )]
        public void Test_0011_int_fallback_null ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["score"] = 100;

            Assert.AreEqual(100, obj.GetInt("score", -1));
            Assert.AreEqual(-1, obj.GetInt(null, -1));
        }


        #endregion

        #region long value

        [Test(), KiiUTInfo(
            action = "When we set long value to KiiObject,",
            expected = "We can get it as long value"
            )]
        public void Test_0100_long ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["score"] = (long)1234567890123;

            Assert.AreEqual("{\"score\":1234567890123}", obj.mJSON.ToString());
            Assert.AreEqual("{\"score\":1234567890123}", obj.mJSONPatch.ToString());

            Assert.AreEqual((long)1234567890123, obj["score"]);
            Assert.AreEqual((long)1234567890123, obj.GetLong("score"));
        }

        [Test(), ExpectedException(typeof(ArgumentNullException)), KiiUTInfo(
            action = "When we call GetLong() with null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0101_long_null ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["score"] = (long)1234567890123;

            obj.GetLong(null);
        }

        [Test(), KiiUTInfo(
            action = "When we call GetLong() with unknown key and fallback,",
            expected = "fallback value must be returned"
            )]
        public void Test_0110_long_fallback ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["score"] = (long)1234567890123;

            Assert.AreEqual((long)1234567890123, obj.GetLong("score", -1));
            Assert.AreEqual(-1, obj.GetLong("scoreDummy", -1));
        }

        [Test(), KiiUTInfo(
            action = "When we call GetLong() with null key and fallback,",
            expected = "fallback value must be returned"
            )]
        public void Test_0111_long_fallback_null ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["score"] = (long)1234567890123;

            Assert.AreEqual((long)1234567890123, obj.GetLong("score", -1));
            Assert.AreEqual(-1, obj.GetLong(null, -1));
        }

        #endregion

        #region double value
        [Test(), KiiUTInfo(
            action = "When we set double value to KiiObject,",
            expected = "We can get it as double value"
            )]
        public void Test_0200_double ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["score"] = 12.345;

            Assert.AreEqual("{\"score\":12.345}", obj.mJSON.ToString());
            Assert.AreEqual("{\"score\":12.345}", obj.mJSONPatch.ToString());

            Assert.AreEqual((double)12.345, obj["score"]);
            Assert.AreEqual((double)12.345, obj.GetDouble("score"));
        }

        [Test(), ExpectedException(typeof(ArgumentNullException)), KiiUTInfo(
            action = "When we call GetDouble() with null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0201_double_null ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["score"] = 12.345;

            obj.GetDouble(null);
        }

        [Test(), KiiUTInfo(
            action = "When we call GetDouble() with unknown key and fallback,",
            expected = "fallback value must be returned"
            )]
        public void Test_0210_double_fallback ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["score"] = 12.345;

            Assert.AreEqual((double)12.345, obj.GetDouble("score", 45.6789));
            Assert.AreEqual((double)45.6789, obj.GetDouble("scoreDummy", 45.6789));
        }

        [Test(), ExpectedException(typeof(ArgumentNullException)), KiiUTInfo(
            action = "When we call GetDouble() with null key and fallback,",
            expected = "ArgumentNullException must be thrown"
            )]
        public void Test_0211_double_fallback_null ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["score"] = 12.345;

            Assert.AreEqual((double)12.345, obj.GetDouble("score", 45.6789));
            Assert.AreEqual((double)45.6789, obj.GetDouble(null, 45.6789));
        }

        #endregion

        #region bool value

        [Test(), KiiUTInfo(
            action = "When we set boolean value(true) to KiiObject,",
            expected = "We can get it as boolean value"
            )]
        public void Test_0300_bool_true ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["enable"] = true;

            Assert.AreEqual("{\"enable\":true}", obj.mJSON.ToString());
            Assert.AreEqual("{\"enable\":true}", obj.mJSONPatch.ToString());

            Assert.AreEqual(true, obj["enable"]);
            Assert.AreEqual(true, obj.GetBoolean("enable"));
        }

        [Test(), KiiUTInfo(
            action = "When we set boolean value(false) to KiiObject,",
            expected = "We can get it as boolean value"
            )]
        public void Test_0301_bool_false ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["enable"] = false;

            Assert.AreEqual("{\"enable\":false}", obj.mJSON.ToString());
            Assert.AreEqual("{\"enable\":false}", obj.mJSONPatch.ToString());

            Assert.AreEqual(false, obj["enable"]);
            Assert.AreEqual(false, obj.GetBoolean("enable"));
        }

        [Test(), ExpectedException(typeof(ArgumentNullException)), KiiUTInfo(
            action = "When we call GetBoolean() with null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0302_bool_null ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["enable"] = true;

            obj.GetBoolean(null);
        }

        [Test(), KiiUTInfo(
            action = "When we call GetBoolean() with unknown key and fallback,",
            expected = "fallback value must be returned"
            )]
        public void Test_0310_bool_fallback ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["enable"] = true;

            Assert.AreEqual(true, obj.GetBoolean("enable", false));
            Assert.AreEqual(false, obj.GetBoolean("enableDymmy", false));
        }

        [Test(), ExpectedException(typeof(ArgumentNullException)), KiiUTInfo(
            action = "When we call GetBoolean() with null key and fallback,",
            expected = "fallback value must be returned"
            )]
        public void Test_0311_bool_fallback_null ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["enable"] = true;

            Assert.AreEqual(true, obj.GetBoolean("enable", false));
            Assert.AreEqual(false, obj.GetBoolean(null, false));
        }

        #endregion

        #region string value

        [Test(), KiiUTInfo(
            action = "When we set string value to KiiObject,",
            expected = "We can get it as string value"
            )]
        public void Test_0400_string ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "kii";

            Assert.AreEqual("{\"name\":\"kii\"}", obj.mJSON.ToString());
            Assert.AreEqual("{\"name\":\"kii\"}", obj.mJSONPatch.ToString());

            Assert.AreEqual("kii", obj["name"]);
            Assert.AreEqual("kii", obj.GetString("name"));
        }

        [Test(), ExpectedException(typeof(ArgumentNullException)), KiiUTInfo(
            action = "When we call GetString() with null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0401_string_null ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "kii";

            obj.GetString(null);
        }

        [Test(), KiiUTInfo(
            action = "When we call GetString() with unknown key and fallback,",
            expected = "fallback value must be returned"
            )]
        public void Test_0410_string_fallback ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "kii";

            Assert.AreEqual("kii", obj.GetString("name", "fallback"));
            Assert.AreEqual("fallback", obj.GetString("nameDymmy", "fallback"));
        }

        [Test(), ExpectedException(typeof(ArgumentNullException)), KiiUTInfo(
            action = "When we call GetString() with null key and fallback,",
            expected = "fallback value must be returned"
            )]
        public void Test_0411_string_fallback_null ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "kii";

            Assert.AreEqual("kii", obj.GetString("name", "fallback"));
            Assert.AreEqual("fallback", obj.GetString(null, "fallback"));
        }

        #endregion

        #region JsonObject value
        [Test(), KiiUTInfo(
            action = "When we set JsonObject value to KiiObject,",
            expected = "We can get it as JsonObject value"
            )]
        public void Test_0500_JsonObject ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            JsonObject json = new JsonObject();
            json.Put("name", "kii");
            obj["attr"] = json;

            Assert.AreEqual("{\"attr\":{\"name\":\"kii\"}}", obj.mJSON.ToString());
            Assert.AreEqual("{\"attr\":{\"name\":\"kii\"}}", obj.mJSONPatch.ToString());

            Assert.AreEqual("kii", ((JsonObject)obj["attr"]).GetString("name"));
            Assert.AreEqual("kii", obj.GetJsonObject("attr").GetString("name"));
        }

        #endregion

        #region JsonArray value

        [Test(), KiiUTInfo(
            action = "When we set JsonArray value to KiiObject,",
            expected = "We can get it as JsonArray value"
            )]
        public void Test_0600_JsonArray ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            JsonArray array = new JsonArray();
            array.Put("Kaa");
            array.Put("Kii");
            obj["names"] = array;

            Assert.AreEqual("{\"names\":[\"Kaa\",\"Kii\"]}", obj.mJSON.ToString());
            Assert.AreEqual("{\"names\":[\"Kaa\",\"Kii\"]}", obj.mJSONPatch.ToString());

            Assert.AreEqual("Kaa", ((JsonArray)obj["names"]).GetString(0));
            Assert.AreEqual("Kii", obj.GetJsonArray("names").GetString(1));
        }

        #endregion

        #region byteArray value

        [Test(), KiiUTInfo(
            action = "When we set ByteArray value to KiiObject,",
            expected = "We can get it as ByteArray value"
            )]
        public void Test_0700_ByteArray ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            byte[] array = new byte[] {1, 2, 3, 4};
            obj["data"] = array;

            Assert.AreEqual("{\"data\":\"AQIDBA==\"}", obj.mJSON.ToString());
            Assert.AreEqual("{\"data\":\"AQIDBA==\"}", obj.mJSONPatch.ToString());

            Assert.AreEqual("AQIDBA==", (string)obj["data"]);
            byte[] outArray = obj.GetByteArray("data");
            Assert.AreEqual(array, outArray);
        }

        #endregion

        #region Uri value

        [Test(), KiiUTInfo(
            action = "When we set Uri value to KiiObject,",
            expected = "We can get it as Uri value"
            )]
        public void Test_0800_Uri ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            Uri uri = new Uri("kiicloud://users/abcd");
            obj["id"] = uri;

            Assert.AreEqual("{\"id\":\"kiicloud:\\/\\/users\\/abcd\"}", obj.mJSON.ToString());
            Assert.AreEqual("{\"id\":\"kiicloud:\\/\\/users\\/abcd\"}", obj.mJSONPatch.ToString());

            Assert.AreEqual("kiicloud://users/abcd", (string)obj["id"]);
            Uri outUri = obj.GetUri("id");
            Assert.AreEqual(uri, outUri);
        }

        #endregion

        #region Has

        [Test(), KiiUTInfo(
            action = "When we call Has(),",
            expected = "If key is known, return true / otherwise, return false"
            )]
        public void Test_0900_Has ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["score"] = 100;

            Assert.IsTrue(obj.Has ("score"));
            Assert.IsFalse(obj.Has ("name"));
        }

        [Test(), ExpectedException(typeof(ArgumentNullException)), KiiUTInfo(
            action = "When we call Has() with null,",
            expected = "ArgumentNullException must be thrown"
            )]
        public void Test_0901_Has_null ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["score"] = 100;

            obj.Has (null);
        }

        #endregion

        #region Remove

        [Test(), KiiUTInfo(
            action = "When we call Remove() with known key,",
            expected = "It must be removed from the Object and Has() must return false"
            )]
        public void Test_1000_Remove ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "kii";
            obj["score"] = 100;

            obj.Remove("score");
            Assert.IsTrue(obj.Has("name"));
            Assert.IsFalse(obj.Has("score"));
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call Remove() with null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_1001_Remove_null ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "kii";
            obj["score"] = 100;

            obj.Remove(null);
        }

        #endregion

        #region Keys

        [Test(), KiiUTInfo(
            action = "When we call Keys(),",
            expected = "We can get enumerable object and get all keys"
            )]
        public void Test_1100_Keys ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();
            obj["name"] = "kii";
            obj["score"] = 100;

            IEnumerable<string> keys = obj.Keys();
            List<string> list = new List<string>();
            foreach (string k in keys)
            {
                list.Add(k);
            }
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("name", list[0]);
            Assert.AreEqual("score", list[1]);
        }

        [Test(), KiiUTInfo(
            action = "When we call Keys() to empty KiiObject,",
            expected = "We can get enumerable object which has no entry"
            )]
        public void Test_1101_Keys_empty ()
        {
            KiiObject obj = Kii.Bucket("test").NewKiiObject();

            IEnumerable<string> keys = obj.Keys();
            List<string> list = new List<string>();
            foreach (string k in keys)
            {
                list.Add(k);
            }
            Assert.AreEqual(0, list.Count);
        }

        #endregion

        #region Reserved Field


        [Test(), KiiUTInfo(
            action = "When we set the reserved field to KiiUser instance,",
            expected = "ArgumentException must be thrown"
        )]
        public void Test_Reserved_Field()
        {
            string[] KiiUserReservedFields = new string[] {
                "loginName",
                "emailAddress",
                "phoneNumber",
                "displayName",
                "country",
                "locale",
                "userID",
                "password",
                "internalUserID",
                "phoneNumberVerified",
                "emailAddressVerified",
                "_hasPassword",
                "_disabled"};

            KiiUser user = KiiUser.UserWithID("user-id-0001");

            foreach (string reservedField in KiiUserReservedFields)
            {
                try
                {
                    user [reservedField] = "value";
                    Assert.Fail("ArgumentException must be thrown");
                }
                catch (ArgumentException expected)
                {
                }
            }

        }
        #endregion
    }
}

