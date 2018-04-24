using System;
using JsonOrg;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class UserFieldsTest
    {
        private static string[] RESERVED_KEY = new string[] {
            "loginName",
            "emailAddress",
            "phoneNumber",
            "displayName",
            "country",
            "userID",
            "internalUserID",
            "phoneNumberVerified",
            "emailAddressVerified"
        };

        #region Normal Cases
        [Test()]
        public void SetAllTypeTest()
        {
            UserFields uf = new UserFields();
            uf["field1"] = true;
            uf["field2"] = new byte[] {0x00};
            uf["field3"] = 10;
            uf["field4"] = 10L;
            uf["field5"] = 10.01;
            uf["field6"] = "hoge";
            uf["field7"] = new Uri("http://hoge.com/hoge");
            uf["field8"] = new JsonObject("{}");
            uf["field9"] = new JsonArray("[111,222]");
            uf.Displayname = "Displayname1";
            uf.Country = "US";

            Assert.AreEqual(true, uf["field1"]);
            Assert.AreEqual(Convert.ToBase64String(new byte[] {0x00}), uf["field2"]);
            Assert.AreEqual(10, uf["field3"]);
            Assert.AreEqual(10L, uf["field4"]);
            Assert.AreEqual(10.01, uf["field5"]);
            Assert.AreEqual("hoge", uf["field6"]);
            Assert.AreEqual(new Uri("http://hoge.com/hoge"), uf["field7"]);
            Assert.IsInstanceOfType(typeof(JsonObject), uf["field8"]);
            Assert.AreEqual("{}", uf["field8"].ToString());
            Assert.IsInstanceOfType(typeof(JsonArray), uf["field9"]);
            Assert.AreEqual("[111,222]", uf["field9"].ToString());
            Assert.AreEqual("Displayname1", uf.Displayname);
            Assert.AreEqual("US", uf.Country);
        }
        [Test()]
        public void RemoveAllTypeTest()
        {
            UserFields uf = new UserFields();
            uf["field1"] = true;
            uf["field2"] = new byte[] {0x00};
            uf["field3"] = 10;
            uf["field4"] = 10L;
            uf["field5"] = 10.01;
            uf["field6"] = "hoge";
            uf["field7"] = new Uri("http://hoge.com/hoge");
            uf["field8"] = new JsonObject("{}");
            uf["field9"] = new JsonArray("[111,222]");
            uf.Displayname = "Displayname1";
            uf.Country = "US";

            uf.Remove("field1");
            uf.Remove("field2");
            uf.Remove("field3");
            uf.Remove("field4");
            uf.Remove("field5");
            uf.Remove("field6");
            uf.Remove("field7");
            uf.Remove("field8");
            uf.Remove("field9");
            uf.RemoveDisplayname();
            uf.RemoveCountry();

            Assert.IsFalse(uf.Has("field1"));
            Assert.IsFalse(uf.Has("field2"));
            Assert.IsFalse(uf.Has("field3"));
            Assert.IsFalse(uf.Has("field4"));
            Assert.IsFalse(uf.Has("field5"));
            Assert.IsFalse(uf.Has("field6"));
            Assert.IsFalse(uf.Has("field7"));
            Assert.IsFalse(uf.Has("field8"));
            Assert.IsFalse(uf.Has("field9"));
            Assert.IsNull(uf.Displayname);
            Assert.IsNull(uf.Country);
        }
        [Test()]
        public void RemoveFromServerTest()
        {
            UserFields uf = new UserFields();
            uf.RemoveFromServer("field");
            Assert.AreEqual(1, uf.RemovedFields.Length);
            Assert.AreEqual("field", uf.RemovedFields[0]);
        }
        [Test()]
        public void CancelRemoveFromServerTest()
        {
            UserFields uf = new UserFields();
            uf.RemoveFromServer("field");
            uf.Remove("field");
            Assert.AreEqual(0, uf.RemovedFields.Length);
        }
        [Test()]
        public void SetAllTypeAfterRemoveFromServerTest()
        {
            UserFields uf = new UserFields();
            uf.RemoveFromServer("field1");
            uf.RemoveFromServer("field2");
            uf.RemoveFromServer("field3");
            uf.RemoveFromServer("field4");
            uf.RemoveFromServer("field5");
            uf.RemoveFromServer("field6");
            uf.RemoveFromServer("field7");
            uf.RemoveFromServer("field8");
            uf.RemoveFromServer("field9");

            uf["field1"] = true;
            uf["field2"] = new byte[] {0x00};
            uf["field3"] = 10;
            uf["field4"] = 10L;
            uf["field5"] = 10.01;
            uf["field6"] = "hoge";
            uf["field7"] = new Uri("http://hoge.com/hoge");
            uf["field8"] = new JsonObject("{}");
            uf["field9"] = new JsonArray("[111,222]");

            Assert.AreEqual(0, uf.RemovedFields.Length);
            Assert.AreEqual(true, uf["field1"]);
            Assert.AreEqual(Convert.ToBase64String(new byte[] {0x00}), uf["field2"]);
            Assert.AreEqual(10, uf["field3"]);
            Assert.AreEqual(10L, uf["field4"]);
            Assert.AreEqual(10.01, uf["field5"]);
            Assert.AreEqual("hoge", uf["field6"]);
            Assert.AreEqual(new Uri("http://hoge.com/hoge"), uf["field7"]);
            Assert.IsInstanceOfType(typeof(JsonObject), uf["field8"]);
            Assert.AreEqual("{}", uf["field8"].ToString());
            Assert.IsInstanceOfType(typeof(JsonArray), uf["field9"]);
            Assert.AreEqual("[111,222]", uf["field9"].ToString());
        }
        [Test()]
        public void GetExistsKeyValueTest()
        {
            UserFields uf = new UserFields();
            uf["test"] = "hoge";
            Assert.AreEqual("hoge", uf["test"]);
            Assert.AreEqual("hoge", uf.GetString("test", null));
        }
        [Test()]
        public void IsEmptyForKeyValuePairNotExistsTest()
        {
            UserFields uf = new UserFields();
            Assert.IsTrue(uf.IsEmpty);
        }
        [Test()]
        public void IsEmptyForKeyValuePairExistsTest()
        {
            UserFields uf = new UserFields();
            uf["test"] = "hoge";
            Assert.IsFalse(uf.IsEmpty);
        }
        #endregion

        #region Error Cases
//        [Test()]
//        public void SetBooleanWithReservedKeyTest()
//        {
//            UserFields uf = new UserFields();
//            foreach (string key in RESERVED_KEY)
//            {
//                try
//                {
//                    uf[key] = true;
//                    Assert.Fail("Exception is not thrown key=" + key);
//                }
//                catch (ArgumentException)
//                {
//                }
//            }
//        }
//        [Test()]
//        public void SetByteArrayWithReservedKeyTest()
//        {
//            UserFields uf = new UserFields();
//            foreach (string key in RESERVED_KEY)
//            {
//                try
//                {
//                    uf[key] = new byte[] {0x00};
//                    Assert.Fail("Exception is not thrown key=" + key);
//                }
//                catch (ArgumentException)
//                {
//                }
//            }
//        }
//        [Test()]
//        public void SetIntWithReservedKeyTest()
//        {
//            UserFields uf = new UserFields();
//            foreach (string key in RESERVED_KEY)
//            {
//                try
//                {
//                    uf[key] = 10;
//                    Assert.Fail("Exception is not thrown key=" + key);
//                }
//                catch (ArgumentException)
//                {
//                }
//            }
//        }
//        [Test()]
//        public void SetLongWithReservedKeyTest()
//        {
//            UserFields uf = new UserFields();
//            foreach (string key in RESERVED_KEY)
//            {
//                try
//                {
//                    uf[key] = 10L;
//                    Assert.Fail("Exception is not thrown key=" + key);
//                }
//                catch (ArgumentException)
//                {
//                }
//            }
//        }
//        [Test()]
//        public void SetDoubleWithReservedKeyTest()
//        {
//            UserFields uf = new UserFields();
//            foreach (string key in RESERVED_KEY)
//            {
//                try
//                {
//                    uf[key] = 10.01;
//                    Assert.Fail("Exception is not thrown key=" + key);
//                }
//                catch (ArgumentException)
//                {
//                }
//            }
//        }
//        [Test()]
//        public void SetStringWithReservedKeyTest()
//        {
//            UserFields uf = new UserFields();
//            foreach (string key in RESERVED_KEY)
//            {
//                try
//                {
//                    uf[key] = "hoge";
//                    Assert.Fail("Exception is not thrown key=" + key);
//                }
//                catch (ArgumentException)
//                {
//                }
//            }
//        }
//        [Test()]
//        public void SetUriWithReservedKeyTest()
//        {
//            UserFields uf = new UserFields();
//            foreach (string key in RESERVED_KEY)
//            {
//                try
//                {
//                    uf[key] = new Uri("http://hoge.com/hoge");
//                    Assert.Fail("Exception is not thrown key=" + key);
//                }
//                catch (ArgumentException)
//                {
//                }
//            }
//        }
//        [Test()]
//        public void SetJsonObjectWithReservedKeyTest()
//        {
//            UserFields uf = new UserFields();
//            foreach (string key in RESERVED_KEY)
//            {
//                try
//                {
//                    uf[key] = new JsonObject("{}");
//                    Assert.Fail("Exception is not thrown key=" + key);
//                }
//                catch (ArgumentException)
//                {
//                }
//            }
//        }
//        [Test()]
//        public void SetJsonArrayWithReservedKeyTest()
//        {
//            UserFields uf = new UserFields();
//            foreach (string key in RESERVED_KEY)
//            {
//                try
//                {
//                    uf[key] = new JsonArray("[111, 222]");
//                    Assert.Fail("Exception is not thrown key=" + key);
//                }
//                catch (ArgumentException)
//                {
//                }
//            }
//        }
        [Test()]
        public void RemoveWithReservedKeyTest()
        {
            UserFields uf = new UserFields();
            foreach (string key in RESERVED_KEY)
            {
                try
                {
                    uf.Remove(key);
                    Assert.Fail("Exception is not thrown key=" + key);
                }
                catch (ArgumentException)
                {
                }
            }
        }
        [Test()]
        public void RemoveFromServerWithReservedKeyTest()
        {
            UserFields uf = new UserFields();
            foreach (string key in RESERVED_KEY)
            {
                try
                {
                    uf.RemoveFromServer(key);
                    Assert.Fail("Exception is not thrown key=" + key);
                }
                catch (ArgumentException)
                {
                }
            }
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void SetNullDisplaynameTest()
        {
            UserFields uf = new UserFields();
            uf.Displayname = null;
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void SetNullCountryTest()
        {
            UserFields uf = new UserFields();
            uf.Country = null;
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void SetEmptyDisplaynameTest()
        {
            UserFields uf = new UserFields();
            uf.Displayname = "";
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void SetEmptyCountryTest()
        {
            UserFields uf = new UserFields();
            uf.Country = "";
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void SetInvalidCountryTest()
        {
            UserFields uf = new UserFields();
            uf.Country = "12";
        }
        [Test()]
        public void GetNotExistsKeyValueTest()
        {
            UserFields uf = new UserFields();
            uf["test"] = "hoge";
            try
            {
                string value = (string)uf["test2"];
                Assert.Fail("Should throw Exception");
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(IllegalKiiBaseObjectFormatException), e.GetType());
            }
            Assert.IsNull(uf.GetString("test2", null));
        }
        #endregion
    }
}

