// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using NUnit.Framework;

using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiServerCodeEntry
    {
        [SetUp()]
        public void SetUp()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
        }

        #region Kii.ServerCodeEntry()
        [Test(), KiiUTInfo(
            action = "When we call Kii.ServerCodeEntry() with valid entryName, ",
            expected = "We can create KiiServerCodeEntry"
            )]
        public void Test_0000_ServerCodeEntry_create_OK()
        {
            KiiServerCodeEntry entry = Kii.ServerCodeEntry("myFunc");
            Assert.IsNotNull(entry);
            Assert.AreEqual("myFunc", entry.EntryName);
            Assert.AreEqual("current", entry.Version);
            Assert.IsFalse(entry.EnvironmentVersion.HasValue);
        }

        [Test(), ExpectedException(typeof(ArgumentNullException)), KiiUTInfo(
            action = "When we call Kii.ServerCodeEntry() with null entryName, ",
            expected = "ArgumentNullException must be thrown"
            )]
        public void Test_0010_ServerCodeEntry_create_null()
        {
            Kii.ServerCodeEntry(null);
        }

        [Test(), ExpectedException(typeof(ArgumentNullException)), KiiUTInfo(
            action = "When we call Kii.ServerCodeEntry() with empty entryName, ",
            expected = "ArgumentNullException must be thrown"
            )]
        public void Test_0011_ServerCodeEntry_create_empty()
        {
            Kii.ServerCodeEntry("");
        }

        [Test(), KiiUTInfo(
            action = "When we call Kii.ServerCodeEntry() with valid entryName and version, ",
            expected = "We can create KiiServerCodeEntry"
            )]
        public void Test_0100_ServerCodeEntry_create_OK()
        {
            KiiServerCodeEntry entry = Kii.ServerCodeEntry("myFunc", "version0001");
            Assert.IsNotNull(entry);
            Assert.AreEqual("myFunc", entry.EntryName);
            Assert.AreEqual("version0001", entry.Version);
        }

        [Test(), ExpectedException(typeof(ArgumentNullException)), KiiUTInfo(
            action = "When we call Kii.ServerCodeEntry() with null entryName, ",
            expected = "ArgumentNullException must be thrown"
            )]
        public void Test_0110_ServerCodeEntry_create_null_entryName()
        {
            Kii.ServerCodeEntry(null, "version0001");
        }

        [Test(), ExpectedException(typeof(ArgumentNullException)), KiiUTInfo(
            action = "When we call Kii.ServerCodeEntry() with empty entryName, ",
            expected = "ArgumentNullException must be thrown"
            )]
        public void Test_0111_ServerCodeEntry_create_empty_entryName()
        {
            Kii.ServerCodeEntry("", "version0001");
        }

        [Test(), ExpectedException(typeof(ArgumentNullException)), KiiUTInfo(
            action = "When we call Kii.ServerCodeEntry() with null version, ",
            expected = "ArgumentNullException must be thrown"
            )]
        public void Test_0112_ServerCodeEntry_create_null_version()
        {
            Kii.ServerCodeEntry("myFunc", (string)null);
        }

        [Test(), ExpectedException(typeof(ArgumentNullException)), KiiUTInfo(
            action = "When we call Kii.ServerCodeEntry() with empty version, ",
            expected = "ArgumentNullException must be thrown"
            )]
        public void Test_0113_ServerCodeEntry_create_empty_version()
        {
            Kii.ServerCodeEntry("myFunc", "");
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call Kii.ServerCodeEntry() with invalid entryName, ",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0114_ServerCodeEntry_create_invalid_entryName()
        {
            Kii.ServerCodeEntry("1MyFunc", "version0001");
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call Kii.ServerCodeEntry() with over 36 characters version, ",
            expected = "ArgumentException must be thrown"
        )]
        public void Test_0115_ServerCodeEntry_create_invalid_verson()
        {
            // over 36 characters
            Kii.ServerCodeEntry("myFunc", new String('0', 37));
        }
        #endregion

        #region KiiServerCodeEntryArgument.NewArgument
        [Test(), KiiUTInfo(
            action = "When we call KiiServerCodeEntryArgument.NewArgument() with valid JsonObject, ",
            expected = "We can get KiiServerCodeEntryArgument instance"
            )]
        public void Test_0200_ServerCodeEntryArgument_OK()
        {
            JsonObject rawArgs = new JsonObject();
            rawArgs.Put("name", "kii");
            rawArgs.Put("score", 100);
            KiiServerCodeEntryArgument args = KiiServerCodeEntryArgument.NewArgument(rawArgs);

            // Assertion
            Assert.IsNotNull(args);
            JsonObject argsJson = args.ToJson();
            Assert.AreEqual(2, argsJson.Length());
        }

        [Test(), ExpectedException(typeof(ArgumentNullException)), KiiUTInfo(
            action = "When we call KiiServerCodeEntryArgument.NewArgument() with null, ",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0210_ServerCodeEntryArgument_null()
        {
            KiiServerCodeEntryArgument.NewArgument(null);
        }
        #endregion

        #region KiiServerCodeEntry.Execute()
        [Test(), KiiUTInfo(
            action = "When we call Execute with valid args and server returns a response, ",
            expected = "We can get the response"
            )]
        public void Test_1000_ServerCodeEntry_Execute_OK()
        {
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set Response
            MockHttpClient client = factory.Client;
            client.AddResponse(200, "{\"returnedValue\":\"testResult\"}", 10);

            JsonObject rawArgs = new JsonObject();
            rawArgs.Put("name", "kii");
            rawArgs.Put("score", 100);
            KiiServerCodeEntryArgument args = KiiServerCodeEntryArgument.NewArgument(rawArgs);

            KiiServerCodeEntry entry = Kii.ServerCodeEntry("myFunc");
            KiiServerCodeExecResult result = entry.Execute(args);

            // Assertion
            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.ExecutedSteps);
            JsonObject json = result.ReturnedValue;
            Assert.AreEqual(1, json.Length());
            Assert.IsTrue(json.Has("returnedValue"));
            Assert.AreEqual("testResult", json.GetString("returnedValue"));

            // Assertion request
            Assert.AreEqual("https://api.kii.com/api/apps/appId/server-code/versions/current/myFunc", client.RequestUrl[0]);
            JsonObject requestJson = new JsonObject(client.RequestBody[0]);
            Assert.AreEqual("kii", requestJson.GetString("name"));
            Assert.AreEqual(100, requestJson.GetInt("score"));
        }

        [Test(), KiiUTInfo(
            action = "When we call Execute with null args and server returns a response, ",
            expected = "We can get the response"
            )]
        public void Test_1001_ServerCodeEntry_Execute_null()
        {
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set Response
            MockHttpClient client = factory.Client;
            client.AddResponse(200, "{\"returnedValue\":\"testResult\"}", 10);
            
            KiiServerCodeEntry entry = Kii.ServerCodeEntry("myFunc");
            KiiServerCodeExecResult result = entry.Execute(null);
            
            // Assertion
            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.ExecutedSteps);
            JsonObject json = result.ReturnedValue;
            Assert.AreEqual(1, json.Length());
            Assert.IsTrue(json.Has("returnedValue"));
            Assert.AreEqual("testResult", json.GetString("returnedValue"));
            
            // Assertion request
            Assert.AreEqual("https://api.kii.com/api/apps/appId/server-code/versions/current/myFunc", client.RequestUrl[0]);
            JsonObject requestJson = new JsonObject(client.RequestBody[0]);
            Assert.AreEqual(0, requestJson.Length());
        }

        [Test(), KiiUTInfo(
            action = "When we call Execute with valid args and server and environment version returns a response, ",
            expected = "We can get the response"
        )]
        public void Test_1002_ServerCodeEntry_Execute_With_EnvironmentVersion0_OK()
        {
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set Response
            MockHttpClient client = factory.Client;
            client.AddResponse(200, "{\"returnedValue\":\"testResult\"}", 10);

            JsonObject rawArgs = new JsonObject();
            rawArgs.Put("name", "kii");
            rawArgs.Put("score", 100);
            KiiServerCodeEntryArgument args = KiiServerCodeEntryArgument.NewArgument(rawArgs);

            KiiServerCodeEntry entry = Kii.ServerCodeEntry("myFunc", KiiServerCodeEnvironmentVersion.V0);
            KiiServerCodeExecResult result = entry.Execute(args);

            // Assertion
            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.ExecutedSteps);
            JsonObject json = result.ReturnedValue;
            Assert.AreEqual(1, json.Length());
            Assert.IsTrue(json.Has("returnedValue"));
            Assert.AreEqual("testResult", json.GetString("returnedValue"));

            // Assertion request
            Assert.AreEqual("https://api.kii.com/api/apps/appId/server-code/versions/current/myFunc?environment-version=0", client.RequestUrl[0]);
            JsonObject requestJson = new JsonObject(client.RequestBody[0]);
            Assert.AreEqual("kii", requestJson.GetString("name"));
            Assert.AreEqual(100, requestJson.GetInt("score"));
        }

        [Test(), KiiUTInfo(
            action = "When we call Execute with valid args and server and environment version returns a response, ",
            expected = "We can get the response"
        )]
        public void Test_1003_ServerCodeEntry_Execute_With_EnvironmentVersion6_OK()
        {
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;

            // set Response
            MockHttpClient client = factory.Client;
            client.AddResponse(200, "{\"returnedValue\":\"testResult\"}", 10);

            JsonObject rawArgs = new JsonObject();
            rawArgs.Put("name", "kii");
            rawArgs.Put("score", 100);
            KiiServerCodeEntryArgument args = KiiServerCodeEntryArgument.NewArgument(rawArgs);

            KiiServerCodeEntry entry = Kii.ServerCodeEntry("myFunc", KiiServerCodeEnvironmentVersion.V6);
            KiiServerCodeExecResult result = entry.Execute(args);

            // Assertion
            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.ExecutedSteps);
            JsonObject json = result.ReturnedValue;
            Assert.AreEqual(1, json.Length());
            Assert.IsTrue(json.Has("returnedValue"));
            Assert.AreEqual("testResult", json.GetString("returnedValue"));

            // Assertion request
            Assert.AreEqual("https://api.kii.com/api/apps/appId/server-code/versions/current/myFunc?environment-version=6", client.RequestUrl[0]);
            JsonObject requestJson = new JsonObject(client.RequestBody[0]);
            Assert.AreEqual("kii", requestJson.GetString("name"));
            Assert.AreEqual(100, requestJson.GetInt("score"));
        }

        [Test(), ExpectedException(typeof(BadRequestException)), KiiUTInfo(
            action = "When we call Execute with valid args and server returns HTTP 400, ",
            expected = "BadRequestException must be thrown"
            )]
        public void Test_1010_ServerCodeEntry_Execute_BadRequest()
        {
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set Response
            MockHttpClient client = factory.Client;
            client.AddResponse(new BadRequestException("", null, "{}", BadRequestException.Reason.__UNKNOWN__));
            
            JsonObject rawArgs = new JsonObject();
            rawArgs.Put("name", "kii");
            rawArgs.Put("score", 100);
            KiiServerCodeEntryArgument args = KiiServerCodeEntryArgument.NewArgument(rawArgs);
            
            KiiServerCodeEntry entry = Kii.ServerCodeEntry("myFunc");
            entry.Execute(args);
        }

        [Test(), ExpectedException(typeof(IllegalKiiBaseObjectFormatException)), KiiUTInfo(
            action = "When we call Execute with valid args and server returns a broken JSON, ",
            expected = "IllegalKiiBaseObjectFormatException must be thrown"
            )]
        public void Test_1011_ServerCodeEntry_Execute_broken_JSON()
        {
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            // set Response
            MockHttpClient client = factory.Client;
            client.AddResponse(200, "broken", 10);
            
            JsonObject rawArgs = new JsonObject();
            rawArgs.Put("name", "kii");
            rawArgs.Put("score", 100);
            KiiServerCodeEntryArgument args = KiiServerCodeEntryArgument.NewArgument(rawArgs);
            
            KiiServerCodeEntry entry = Kii.ServerCodeEntry("myFunc");
            entry.Execute(args);
        }
        #endregion

        #region KiiServerCodeEntry.Execute(callback)
        [Test(), KiiUTInfo(
            action = "When we call Execute with valid args and server returns a response, ",
            expected = "We can get the response"
            )]
        public void Test_1100_ServerCodeEntry_Execute_OK()
        {
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.AsyncHttpClientFactory = factory;
            
            // set Response
            MockHttpClient client = factory.Client;
            client.AddResponse(200, "{\"returnedValue\":\"testResult\"}", 10);
            
            JsonObject rawArgs = new JsonObject();
            rawArgs.Put("name", "kii");
            rawArgs.Put("score", 100);
            KiiServerCodeEntryArgument args = KiiServerCodeEntryArgument.NewArgument(rawArgs);
            
            KiiServerCodeEntry entry = Kii.ServerCodeEntry("myFunc");

            KiiServerCodeEntryArgument args2 = null;
            KiiServerCodeExecResult result = null;
            Exception exception = null;
            entry.Execute(args, (KiiServerCodeEntry entry2, KiiServerCodeEntryArgument outArgs, KiiServerCodeExecResult outResult, Exception e) =>
            {
                args2 = outArgs;
                result = outResult;
                exception = e;
            });

            // Assertion
            Assert.IsNotNull(args2);
            Assert.IsNotNull(result);
            Assert.IsNull(exception);

            Assert.AreEqual(10, result.ExecutedSteps);
            JsonObject json = result.ReturnedValue;
            Assert.AreEqual(1, json.Length());
            Assert.IsTrue(json.Has("returnedValue"));
            Assert.AreEqual("testResult", json.GetString("returnedValue"));
            
            // Assertion request
            Assert.AreEqual("https://api.kii.com/api/apps/appId/server-code/versions/current/myFunc", client.RequestUrl[0]);
            JsonObject requestJson = new JsonObject(client.RequestBody[0]);
            Assert.AreEqual("kii", requestJson.GetString("name"));
            Assert.AreEqual(100, requestJson.GetInt("score"));
        }
        
        [Test(), KiiUTInfo(
            action = "When we call Execute with null args and server returns a response, ",
            expected = "We can get the response"
            )]
        public void Test_1101_ServerCodeEntry_Execute_null()
        {
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.AsyncHttpClientFactory = factory;
            
            // set Response
            MockHttpClient client = factory.Client;
            client.AddResponse(200, "{\"returnedValue\":\"testResult\"}", 10);
            
            KiiServerCodeEntry entry = Kii.ServerCodeEntry("myFunc");

            KiiServerCodeEntryArgument args2 = null;
            KiiServerCodeExecResult result = null;
            Exception exception = null;
            entry.Execute(null, (KiiServerCodeEntry entry2, KiiServerCodeEntryArgument outArgs, KiiServerCodeExecResult outResult, Exception e) =>
            {
                args2 = outArgs;
                result = outResult;
                exception = e;
            });
            
            // Assertion
            Assert.IsNotNull(args2);
            Assert.IsNotNull(result);
            Assert.IsNull(exception);
            
            Assert.AreEqual(10, result.ExecutedSteps);
            JsonObject json = result.ReturnedValue;
            Assert.AreEqual(1, json.Length());
            Assert.IsTrue(json.Has("returnedValue"));
            Assert.AreEqual("testResult", json.GetString("returnedValue"));
            
            // Assertion request
            Assert.AreEqual("https://api.kii.com/api/apps/appId/server-code/versions/current/myFunc", client.RequestUrl[0]);
            JsonObject requestJson = new JsonObject(client.RequestBody[0]);
            Assert.AreEqual(0, requestJson.Length());
        }
        
        [Test(), KiiUTInfo(
            action = "When we call Execute with valid args and server returns HTTP 400, ",
            expected = "BadRequestException must be given"
            )]
        public void Test_1110_ServerCodeEntry_Execute_BadRequest()
        {
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.AsyncHttpClientFactory = factory;
            
            // set Response
            MockHttpClient client = factory.Client;
            client.AddResponse(new BadRequestException("", null, "{}", BadRequestException.Reason.__UNKNOWN__));
            
            JsonObject rawArgs = new JsonObject();
            rawArgs.Put("name", "kii");
            rawArgs.Put("score", 100);
            KiiServerCodeEntryArgument args = KiiServerCodeEntryArgument.NewArgument(rawArgs);
            
            KiiServerCodeEntry entry = Kii.ServerCodeEntry("myFunc");

            KiiServerCodeEntryArgument args2 = null;
            KiiServerCodeExecResult result = null;
            Exception exception = null;
            entry.Execute(args, (KiiServerCodeEntry entry2, KiiServerCodeEntryArgument outArgs, KiiServerCodeExecResult outResult, Exception e) =>
            {
                args2 = outArgs;
                result = outResult;
                exception = e;
            });
            
            // Assertion
            Assert.IsNotNull(args2);
            Assert.IsNull(result);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is BadRequestException);
        }
        
        [Test(), KiiUTInfo(
            action = "When we call Execute with valid args and server returns a broken JSON, ",
            expected = "IllegalKiiBaseObjectFormatException must be given"
            )]
        public void Test_1111_ServerCodeEntry_Execute_broken_JSON()
        {
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.AsyncHttpClientFactory = factory;
            
            // set Response
            MockHttpClient client = factory.Client;
            client.AddResponse(200, "broken", 10);
            
            JsonObject rawArgs = new JsonObject();
            rawArgs.Put("name", "kii");
            rawArgs.Put("score", 100);
            KiiServerCodeEntryArgument args = KiiServerCodeEntryArgument.NewArgument(rawArgs);
            
            KiiServerCodeEntry entry = Kii.ServerCodeEntry("myFunc");

            KiiServerCodeEntryArgument args2 = null;
            KiiServerCodeExecResult result = null;
            Exception exception = null;
            entry.Execute(args, (KiiServerCodeEntry entry2, KiiServerCodeEntryArgument outArgs, KiiServerCodeExecResult outResult, Exception e) =>
            {
                args2 = outArgs;
                result = outResult;
                exception = e;
            });
            
            // Assertion
            Assert.IsNotNull(args2);
            Assert.IsNull(result);
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception is IllegalKiiBaseObjectFormatException);
        }        
        #endregion
    }
}
