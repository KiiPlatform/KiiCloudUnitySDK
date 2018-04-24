using System;
using System.Collections.Generic;
using JsonOrg;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class PseudoUserTest
    {
        private MockHttpClient client;

        [SetUp()]
        public void SetUp()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            Kii.AsyncHttpClientFactory = factory;
            client = (MockHttpClient)factory.Client;
        }
        private void LogIn(string userId)
        {
            // set Response
            client.AddResponse(200, "{" +
                "\"id\" : \"" + userId + "\"," +
                "\"access_token\" : \"cdef\"," +
                "\"expires_in\" : 9223372036854775}");
            KiiUser.LogIn(userId, "pass1234");
            client.Clear();
        }

        #region IsPseudoUser
        [Test()]
        public void Test_IsPseudoUser()
        {
            // For register user
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""displayName"" : ""disp"",
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            KiiUser user;
            KiiUser.RegisterAsPseudoUser(null, (KiiUser pseudoUser, Exception e)=>
            {
                Assert.IsNull(e);
                Assert.IsTrue(pseudoUser.IsPseudoUser);
                pseudoUser.Displayname = "disp";
                pseudoUser.Country = "JP";
                Assert.IsTrue(pseudoUser.IsPseudoUser);
                pseudoUser.SetReserveProperty(KiiUser.PROPERTY_USERNAME, "name");
                Assert.IsFalse(pseudoUser.IsPseudoUser);
            });

            user = KiiUser.BuilderWithName("name").Build();
            Assert.IsFalse(user.IsPseudoUser);

            user = KiiUser.BuilderWithEmail("hoge@example.com").Build();
            Assert.IsFalse(user.IsPseudoUser);

            user = KiiUser.BuilderWithPhone("+819011112222").Build();
            Assert.IsFalse(user.IsPseudoUser);

            user = KiiUser.CreateByUri(new Uri("kiicloud://users/abcd"));
            Assert.IsTrue(user.IsPseudoUser);
        }
        #endregion

        #region RegisterAsPseudoUser
        [Test(), ExpectedException(typeof(ArgumentNullException))]
        public void Test_RegisterAsPseudoUserWithNullCallback()
        {
            KiiUser.RegisterAsPseudoUser(null, null);
        }
        [Test()]
        public void Test_RegisterAsPseudoUser()
        {
            // For register user
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""displayName"" : ""disp"",
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            UserFields userFields = new UserFields();
            userFields.Displayname = "disp";

            KiiUser user = KiiUser.RegisterAsPseudoUser(userFields);
            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users", client.RequestUrl [0]);
            Assert.AreEqual("{\"displayName\":\"disp\"}", client.RequestBody[0]);
            Assert.AreEqual("1234567890abcdefghijklmnopqrstuvwxyz", KiiUser.AccessToken);

            // verify Access token dictionary
            Dictionary<string, object> tokenBundle = user.GetAccessTokenDictionary();
            Assert.AreEqual("1234567890abcdefghijklmnopqrstuvwxyz", tokenBundle["access_token"]);
            DateTime expiresAt = (DateTime)tokenBundle["expires_at"];
            Assert.AreEqual( DateTime.MaxValue, expiresAt);

            // verify Access token dictionary for current user
            tokenBundle = KiiUser.CurrentUser.GetAccessTokenDictionary();
            Assert.AreEqual("1234567890abcdefghijklmnopqrstuvwxyz", tokenBundle["access_token"]);
            expiresAt = (DateTime)tokenBundle["expires_at"];
            Assert.AreEqual( DateTime.MaxValue, expiresAt);
        }
        [Test()]
        public void Test_RegisterAsPseudoUser_Async()
        {
            // For register user
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""displayName"" : ""disp"",
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            UserFields userFields = new UserFields();
            userFields.Displayname = "disp";

            KiiUser.RegisterAsPseudoUser(userFields, (KiiUser user, Exception e)=>{
                Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
                Assert.AreEqual ("https://api.kii.com/api/apps/appId/users", client.RequestUrl [0]);
                Assert.AreEqual("{\"displayName\":\"disp\"}", client.RequestBody[0]);
                Assert.AreEqual("1234567890abcdefghijklmnopqrstuvwxyz", KiiUser.AccessToken);

                // verify Access token dictionary
                Dictionary<string, object> tokenBundle = user.GetAccessTokenDictionary();
                Assert.AreEqual("1234567890abcdefghijklmnopqrstuvwxyz", tokenBundle["access_token"]);
                DateTime expiresAt = (DateTime)tokenBundle["expires_at"];
                Assert.AreEqual( DateTime.MaxValue, expiresAt);
                
                // verify Access token dictionary for current user
                tokenBundle = KiiUser.CurrentUser.GetAccessTokenDictionary();
                Assert.AreEqual("1234567890abcdefghijklmnopqrstuvwxyz", tokenBundle["access_token"]);
                expiresAt = (DateTime)tokenBundle["expires_at"];
                Assert.AreEqual( DateTime.MaxValue, expiresAt);
            });
        }
        [Test()]
        public void Test_RegisterAsPseudoUserWithNull()
        {
            // For register user
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""displayName"" : ""disp"",
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            KiiUser user = KiiUser.RegisterAsPseudoUser(null);
            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users", client.RequestUrl [0]);
            Assert.AreEqual("{}", client.RequestBody[0]);
            Assert.AreEqual("1234567890abcdefghijklmnopqrstuvwxyz", KiiUser.AccessToken);

            // verify Access token dictionary
            Dictionary<string, object> tokenBundle = user.GetAccessTokenDictionary();
            Assert.AreEqual("1234567890abcdefghijklmnopqrstuvwxyz", tokenBundle["access_token"]);
            DateTime expiresAt = (DateTime)tokenBundle["expires_at"];
            Assert.AreEqual( DateTime.MaxValue, expiresAt);
            
            // verify Access token dictionary for current user
            tokenBundle = KiiUser.CurrentUser.GetAccessTokenDictionary();
            Assert.AreEqual("1234567890abcdefghijklmnopqrstuvwxyz", tokenBundle["access_token"]);
            expiresAt = (DateTime)tokenBundle["expires_at"];
            Assert.AreEqual( DateTime.MaxValue, expiresAt);
        }
        [Test()]
        public void Test_RegisterAsPseudoUserWithNull_Async()
        {
            // For register user
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""displayName"" : ""disp"",
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            KiiUser.RegisterAsPseudoUser(null, (KiiUser user, Exception e)=>{
                Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
                Assert.AreEqual ("https://api.kii.com/api/apps/appId/users", client.RequestUrl [0]);
                Assert.AreEqual("{}", client.RequestBody[0]);
                Assert.AreEqual("1234567890abcdefghijklmnopqrstuvwxyz", KiiUser.AccessToken);

                // verify Access token dictionary
                Dictionary<string, object> tokenBundle = user.GetAccessTokenDictionary();
                Assert.AreEqual("1234567890abcdefghijklmnopqrstuvwxyz", tokenBundle["access_token"]);
                DateTime expiresAt = (DateTime)tokenBundle["expires_at"];
                Assert.AreEqual( DateTime.MaxValue, expiresAt);
                
                // verify Access token dictionary for current user
                tokenBundle = KiiUser.CurrentUser.GetAccessTokenDictionary();
                Assert.AreEqual("1234567890abcdefghijklmnopqrstuvwxyz", tokenBundle["access_token"]);
                expiresAt = (DateTime)tokenBundle["expires_at"];
                Assert.AreEqual( DateTime.MaxValue, expiresAt);
            });
        }
        #endregion

        #region PutIdentity
        [Test()]
        public void Test_PutIdentity()
        {
            // for RegisterAsPseudoUser
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            KiiUser.RegisterAsPseudoUser(null, (KiiUser pseudoUser, Exception e)=>{
                IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
                builder.WithEmail("kii_user01@gmail.com");
                builder.WithPhone("+819011112222");
                IdentityData identityData = builder.Build();

                UserFields userFields = new UserFields();
                userFields.Country = "JP";
                userFields["age"] = 30;

                // For refresh
                string response2 = @"
                {
                     ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81""
                }";
                client.AddResponse (200, response2);
                // for PutIdentity
                string response3 = @"
                {
                    ""modifiedAt"":123456789
                }";
                client.AddResponse (200, response3);

                pseudoUser.PutIdentity(identityData, userFields, "123ABC");

                Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [2]);
                Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/me", client.RequestUrl [2]);
                JsonObject requestBody = new JsonObject(client.RequestBody[2]);
                Assert.AreEqual(identityData.UserName, requestBody.GetString("loginName"));
                Assert.AreEqual(identityData.Email, requestBody.GetString("emailAddress"));
                Assert.AreEqual(identityData.Phone, requestBody.GetString("phoneNumber"));
                Assert.AreEqual("123ABC", requestBody.GetString("password"));
                Assert.AreEqual(identityData.UserName, KiiUser.CurrentUser.Username);
                Assert.AreEqual(identityData.Email, KiiUser.CurrentUser.Email);
                Assert.AreEqual(identityData.Phone, KiiUser.CurrentUser.Phone);
            });
        }
        [Test()]
        public void Test_PutIdentity_Async()
        {
            // for RegisterAsPseudoUser
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            KiiUser.RegisterAsPseudoUser(null, (KiiUser pseudoUser, Exception e1)=>{
                IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
                builder.WithEmail("kii_user01@gmail.com");
                builder.WithPhone("+819011112222");
                IdentityData identityData = builder.Build();

                UserFields userFields = new UserFields();
                userFields.Country = "JP";
                userFields["age"] = 30;

                // For refresh
                string response4 = @"
                {
                     ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81""
                }";
                client.AddResponse (200, response4);
                // for PutIdentity
                string response5 = @"
                {
                    ""modifiedAt"":123456789
                }";
                client.AddResponse (200, response5);

                pseudoUser.PutIdentity(identityData, userFields, "123ABC", (KiiUser user, Exception e2)=>
                {
                    Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [2]);
                    Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/me", client.RequestUrl [2]);
                    JsonObject requestBody = new JsonObject(client.RequestBody[2]);
                    Assert.AreEqual(identityData.UserName, requestBody.GetString("loginName"));
                    Assert.AreEqual(identityData.Email, requestBody.GetString("emailAddress"));
                    Assert.AreEqual(identityData.Phone, requestBody.GetString("phoneNumber"));
                    Assert.AreEqual("123ABC", requestBody.GetString("password"));
                    Assert.AreEqual(identityData.UserName, KiiUser.CurrentUser.Username);
                    Assert.AreEqual(identityData.Email, KiiUser.CurrentUser.Email);
                    Assert.AreEqual(identityData.Phone, KiiUser.CurrentUser.Phone);
                });
            });
        }
        [Test()]
        public void Test_PutIdentity_Without_UserFields()
        {
            // for RegisterAsPseudoUser
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            KiiUser.RegisterAsPseudoUser(null, (KiiUser pseudoUser, Exception e)=>{
                IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
                builder.WithEmail("kii_user01@gmail.com");
                builder.WithPhone("+819011112222");
                IdentityData identityData = builder.Build();

                // For refresh
                string response2 = @"
                {
                     ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81""
                }";
                client.AddResponse (200, response2);
                // for PutIdentity
                string response3 = @"
                {
                    ""modifiedAt"":123456789
                }";
                client.AddResponse (200, response3);

                pseudoUser.PutIdentity(identityData, null, "123ABC");

                Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [2]);
                Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/me", client.RequestUrl [2]);
                JsonObject requestBody = new JsonObject(client.RequestBody[2]);
                Assert.AreEqual(identityData.UserName, requestBody.GetString("loginName"));
                Assert.AreEqual(identityData.Email, requestBody.GetString("emailAddress"));
                Assert.AreEqual(identityData.Phone, requestBody.GetString("phoneNumber"));
                Assert.AreEqual("123ABC", requestBody.GetString("password"));
                Assert.AreEqual(identityData.UserName, KiiUser.CurrentUser.Username);
                Assert.AreEqual(identityData.Email, KiiUser.CurrentUser.Email);
                Assert.AreEqual(identityData.Phone, KiiUser.CurrentUser.Phone);
            });
        }
        [Test()]
        public void Test_PutIdentity_Async_Without_UserFields()
        {
            // for RegisterAsPseudoUser
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            KiiUser.RegisterAsPseudoUser(null, (KiiUser pseudoUser, Exception e1)=>{
                IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
                builder.WithEmail("kii_user01@gmail.com");
                builder.WithPhone("+819011112222");
                IdentityData identityData = builder.Build();

                // For refresh
                string response4 = @"
                {
                     ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81""
                }";
                client.AddResponse (200, response4);
                // for PutIdentity
                string response5 = @"
                {
                    ""modifiedAt"":123456789
                }";
                client.AddResponse (200, response5);

                pseudoUser.PutIdentity(identityData, null, "123ABC", (KiiUser user, Exception e2)=>
                {
                    Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [2]);
                    Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/me", client.RequestUrl [2]);
                    JsonObject requestBody = new JsonObject(client.RequestBody[2]);
                    Assert.AreEqual(identityData.UserName, requestBody.GetString("loginName"));
                    Assert.AreEqual(identityData.Email, requestBody.GetString("emailAddress"));
                    Assert.AreEqual(identityData.Phone, requestBody.GetString("phoneNumber"));
                    Assert.AreEqual("123ABC", requestBody.GetString("password"));
                    Assert.AreEqual(identityData.UserName, KiiUser.CurrentUser.Username);
                    Assert.AreEqual(identityData.Email, KiiUser.CurrentUser.Email);
                    Assert.AreEqual(identityData.Phone, KiiUser.CurrentUser.Phone);
                });
            });
        }
        [Test(), ExpectedException(typeof(AlreadyHasIdentityException))]
        public void Test_PutIdentity_With_NormalUser()
        {
            // for Refresh
            string response3 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""loginName"" : ""kii_user01""
            }
            ";

            client.AddResponse (200, response3);
            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
            builder.WithEmail("kii_user01@gmail.com");
            builder.WithPhone("+819011112222");
            IdentityData identityData = builder.Build();

            UserFields userFields = new UserFields();
            userFields.Country = "JP";
            userFields["age"] = 30;

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/0398e67a-818d-47ee-83fb-3519a6197b81"));
            user.PutIdentity(identityData, userFields, "password");
        }
        [Test()]
        public void Test_PutIdentity_With_NormalUser_Async()
        {
            // for Refresh
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""loginName"" : ""kii_user01""
            }
            ";

            client.AddResponse (200, response1);
            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
            builder.WithEmail("kii_user01@gmail.com");
            builder.WithPhone("+819011112222");
            IdentityData identityData = builder.Build();

            UserFields userFields = new UserFields();
            userFields.Country = "JP";
            userFields["age"] = 30;

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/0398e67a-818d-47ee-83fb-3519a6197b81"));
            user.PutIdentity(identityData, userFields, "password", (KiiUser u, Exception e)=>
            {
                Assert.IsNotNull(e);
                Assert.IsInstanceOfType(typeof(AlreadyHasIdentityException), e);
            });
        }
        [Test()]
        public void Test_PutIdentity_With_Local_Phone_Without_Country()
        {
            // for RegisterAsPseudoUser
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            KiiUser.RegisterAsPseudoUser(null, (KiiUser pseudoUser, Exception e)=>{
                IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
                builder.WithEmail("kii_user01@gmail.com");
                builder.WithPhone("09011112222");
                IdentityData identityData = builder.Build();

                UserFields userFields = new UserFields();
                userFields["age"] = 30;

                try
                {
                    pseudoUser.PutIdentity(identityData, userFields, "123ABC");
                    Assert.Fail("Exception is not thrown");
                } catch (ArgumentException)
                {
                }
                catch
                {
                    Assert.Fail("Unexpected Exception");
                }
            });
        }
        [Test()]
        public void Test_PutIdentity_With_Local_Phone_Without_Country_Async()
        {
            // for RegisterAsPseudoUser
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            KiiUser.RegisterAsPseudoUser(null, (KiiUser pseudoUser, Exception e1)=>{
                IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
                builder.WithEmail("kii_user01@gmail.com");
                builder.WithPhone("09011112222");
                IdentityData identityData = builder.Build();

                UserFields userFields = new UserFields();
                userFields["age"] = 30;

                pseudoUser.PutIdentity(identityData, userFields, "123ABC", (KiiUser user, Exception e2)=>
                {
                    Assert.IsNotNull(e2);
                    Assert.IsInstanceOfType(typeof(ArgumentException), e2);
                });
            });
        }
        [Test()]
        public void Test_PutIdentity_With_Local_Phone_Without_UserFileds()
        {
            // for RegisterAsPseudoUser
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            KiiUser.RegisterAsPseudoUser(null, (KiiUser pseudoUser, Exception e)=>{
                IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
                builder.WithEmail("kii_user01@gmail.com");
                builder.WithPhone("09011112222");
                IdentityData identityData = builder.Build();

                try
                {
                    pseudoUser.PutIdentity(identityData, null, "123ABC");
                    Assert.Fail("Exception is not thrown");
                } catch (ArgumentException)
                {
                }
                catch
                {
                    Assert.Fail("Unexpected Exception");
                }
            });
        }
        [Test()]
        public void Test_PutIdentity_With_Local_Phone_Without_UserFileds_Async()
        {
            // for RegisterAsPseudoUser
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            KiiUser.RegisterAsPseudoUser(null, (KiiUser pseudoUser, Exception e1)=>{
                IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
                builder.WithEmail("kii_user01@gmail.com");
                builder.WithPhone("09011112222");
                IdentityData identityData = builder.Build();

                pseudoUser.PutIdentity(identityData, null, "123ABC", (KiiUser user, Exception e2)=>
                {
                    Assert.IsNotNull(e2);
                    Assert.IsInstanceOfType(typeof(ArgumentException), e2);
                });
            });
        }
        [Test()]
        public void Test_PutIdentity_With_3Length_Password()
        {
            // for RegisterAsPseudoUser
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            KiiUser.RegisterAsPseudoUser(null, (KiiUser pseudoUser, Exception e1)=>{
                IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
                builder.WithEmail("kii_user01@gmail.com");
                builder.WithPhone("+819011112222");
                IdentityData identityData = builder.Build();

                UserFields userFields = new UserFields();
                userFields.Country = "JP";
                userFields["age"] = 30;

                try
                {
                    pseudoUser.PutIdentity(identityData, userFields, "abc");
                    Assert.Fail("Exception is not thrown");
                } catch (ArgumentException)
                {
                    // Pass
                }
                catch
                {
                    Assert.Fail("Unexpected Exception");
                }
            });
        }
        [Test()]
        public void Test_PutIdentity_With_3Length_Password_Async()
        {
            // for RegisterAsPseudoUser
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            KiiUser.RegisterAsPseudoUser(null, (KiiUser pseudoUser, Exception e1)=>{
                IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
                builder.WithEmail("kii_user01@gmail.com");
                builder.WithPhone("+819011112222");
                IdentityData identityData = builder.Build();

                UserFields userFields = new UserFields();
                userFields.Country = "JP";
                userFields["age"] = 30;

                pseudoUser.PutIdentity(identityData, userFields, "abc", (KiiUser user, Exception e2)=>
                {
                    Assert.IsNotNull(e2);
                    Assert.IsInstanceOfType(typeof(ArgumentException), e2);
                });
            });
        }
        [Test()]
        public void Test_PutIdentity_With_4Length_Password()
        {
            // for RegisterAsPseudoUser
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            KiiUser.RegisterAsPseudoUser(null, (KiiUser pseudoUser, Exception e1)=>{
                IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
                builder.WithEmail("kii_user01@gmail.com");
                builder.WithPhone("+819011112222");
                IdentityData identityData = builder.Build();

                UserFields userFields = new UserFields();
                userFields.Country = "JP";
                userFields["age"] = 30;

                try
                {
                    pseudoUser.PutIdentity(identityData, userFields, "abcd");
                } catch (Exception)
                {
                    Assert.Fail("Exception is thrown");
                }
                catch
                {
                    Assert.Fail("Unexpected Exception");
                }
            });
        }
        [Test()]
        public void Test_PutIdentity_With_4Length_Password_Async()
        {
            // for RegisterAsPseudoUser
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            KiiUser.RegisterAsPseudoUser(null, (KiiUser pseudoUser, Exception e1)=>{
                IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
                builder.WithEmail("kii_user01@gmail.com");
                builder.WithPhone("+819011112222");
                IdentityData identityData = builder.Build();

                UserFields userFields = new UserFields();
                userFields.Country = "JP";
                userFields["age"] = 30;

                pseudoUser.PutIdentity(identityData, userFields, "abcd", (KiiUser user, Exception e2)=>
                {
                    Assert.IsNull(e2);
                });
            });
        }
        [Test()]
        public void Test_PutIdentity_With_50Length_Password()
        {
            // for RegisterAsPseudoUser
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            KiiUser.RegisterAsPseudoUser(null, (KiiUser pseudoUser, Exception e1)=>{
                IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
                builder.WithEmail("kii_user01@gmail.com");
                builder.WithPhone("+819011112222");
                IdentityData identityData = builder.Build();

                UserFields userFields = new UserFields();
                userFields.Country = "JP";
                userFields["age"] = 30;

                try
                {
                    pseudoUser.PutIdentity(identityData, userFields, TextUtils.randomAlphaNumeric(50));
                } catch (Exception)
                {
                    Assert.Fail("Exception is thrown");
                }
                catch
                {
                    Assert.Fail("Unexpected Exception");
                }
            });
        }
        [Test()]
        public void Test_PutIdentity_With_50Length_Password_Async()
        {
            // for RegisterAsPseudoUser
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            KiiUser.RegisterAsPseudoUser(null, (KiiUser pseudoUser, Exception e1)=>{
                IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
                builder.WithEmail("kii_user01@gmail.com");
                builder.WithPhone("+819011112222");
                IdentityData identityData = builder.Build();

                UserFields userFields = new UserFields();
                userFields.Country = "JP";
                userFields["age"] = 30;

                pseudoUser.PutIdentity(identityData, userFields, TextUtils.randomAlphaNumeric(50), (KiiUser user, Exception e2)=>
                {
                    Assert.IsNull(e2);
                });
            });
        }
        [Test()]
        public void Test_PutIdentity_With_51Length_Password()
        {
            // for RegisterAsPseudoUser
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            KiiUser.RegisterAsPseudoUser(null, (KiiUser pseudoUser, Exception e1)=>{
                IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
                builder.WithEmail("kii_user01@gmail.com");
                builder.WithPhone("+819011112222");
                IdentityData identityData = builder.Build();

                UserFields userFields = new UserFields();
                userFields.Country = "JP";
                userFields["age"] = 30;

                try
                {
                    pseudoUser.PutIdentity(identityData, userFields, TextUtils.randomAlphaNumeric(51));
                    Assert.Fail("Exception is not thrown");
                } catch (ArgumentException)
                {
                    // Pass
                }
                catch
                {
                    Assert.Fail("Unexpected Exception");
                }
            });
        }
        [Test()]
        public void Test_PutIdentity_With_51Length_Password_Async()
        {
            // for RegisterAsPseudoUser
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            KiiUser.RegisterAsPseudoUser(null, (KiiUser pseudoUser, Exception e1)=>{
                IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
                builder.WithEmail("kii_user01@gmail.com");
                builder.WithPhone("+819011112222");
                IdentityData identityData = builder.Build();

                UserFields userFields = new UserFields();
                userFields.Country = "JP";
                userFields["age"] = 30;

                pseudoUser.PutIdentity(identityData, userFields, TextUtils.randomAlphaNumeric(51), (KiiUser user, Exception e2)=>
                {
                    Assert.IsNotNull(e2);
                    Assert.IsInstanceOfType(typeof(ArgumentException), e2);
                });
            });
        }
        [Test()]
        public void Test_PutIdentity_With_Invalid_Password()
        {
            // for RegisterAsPseudoUser
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            KiiUser.RegisterAsPseudoUser(null, (KiiUser pseudoUser, Exception e1)=>{
                IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
                builder.WithEmail("kii_user01@gmail.com");
                builder.WithPhone("+819011112222");
                IdentityData identityData = builder.Build();

                UserFields userFields = new UserFields();
                userFields.Country = "JP";
                userFields["age"] = 30;

                try
                {
                    pseudoUser.PutIdentity(identityData, userFields, "ぱすわーど");
                    Assert.Fail("Exception is not thrown");
                } catch (ArgumentException)
                {
                }
                catch
                {
                    Assert.Fail("Unexpected Exception");
                }
            });
        }
        [Test()]
        public void Test_PutIdentity_With_Invalid_Password_Async()
        {
            // for RegisterAsPseudoUser
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            KiiUser.RegisterAsPseudoUser(null, (KiiUser pseudoUser, Exception e1)=>{
                IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
                builder.WithEmail("kii_user01@gmail.com");
                builder.WithPhone("+819011112222");
                IdentityData identityData = builder.Build();

                UserFields userFields = new UserFields();
                userFields.Country = "JP";
                userFields["age"] = 30;

                pseudoUser.PutIdentity(identityData, userFields, "ぱすわーど", (KiiUser user, Exception e2)=>
                {
                    Assert.IsNotNull(e2);
                    Assert.IsInstanceOfType(typeof(ArgumentException), e2);
                });
            });
        }
        [Test()]
        public void Test_PutIdentity_Without_Password()
        {
            // for RegisterAsPseudoUser
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            KiiUser.RegisterAsPseudoUser(null, (KiiUser pseudoUser, Exception e1)=>{
                IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
                builder.WithEmail("kii_user01@gmail.com");
                builder.WithPhone("+819011112222");
                IdentityData identityData = builder.Build();

                UserFields userFields = new UserFields();
                userFields.Country = "JP";
                userFields["age"] = 30;

                try
                {
                    pseudoUser.PutIdentity(identityData, userFields, null);
                    Assert.Fail("Exception is not thrown");
                } catch (ArgumentException)
                {
                }
                catch
                {
                    Assert.Fail("Unexpected Exception");
                }
            });
        }
        [Test()]
        public void Test_PutIdentity_Without_Password_Async()
        {
            // for RegisterAsPseudoUser
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            KiiUser.RegisterAsPseudoUser(null, (KiiUser pseudoUser, Exception e1)=>{
                IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
                builder.WithEmail("kii_user01@gmail.com");
                builder.WithPhone("+819011112222");
                IdentityData identityData = builder.Build();

                UserFields userFields = new UserFields();
                userFields.Country = "JP";
                userFields["age"] = 30;

                pseudoUser.PutIdentity(identityData, userFields, null, (KiiUser user, Exception e2)=>
                {
                    Assert.IsNotNull(e2);
                    Assert.IsInstanceOfType(typeof(ArgumentException), e2);
                });
            });
        }
        [Test()]
        public void Test_PutIdentity_Without_IdentityData()
        {
            // for RegisterAsPseudoUser
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            KiiUser.RegisterAsPseudoUser(null, (KiiUser pseudoUser, Exception e1)=>{
                UserFields userFields = new UserFields();
                userFields.Country = "JP";
                userFields["age"] = 30;

                try
                {
                    pseudoUser.PutIdentity(null, userFields, "password");
                    Assert.Fail("Exception is not thrown");
                } catch (ArgumentException)
                {
                }
                catch
                {
                    Assert.Fail("Unexpected Exception");
                }
            });
        }
        [Test()]
        public void Test_PutIdentity_Without_IdentityData_Async()
        {
            // for RegisterAsPseudoUser
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            KiiUser.RegisterAsPseudoUser(null, (KiiUser pseudoUser, Exception e1)=>{
                UserFields userFields = new UserFields();
                userFields.Country = "JP";
                userFields["age"] = 30;

                pseudoUser.PutIdentity(null, userFields, "password", (KiiUser user, Exception e2)=>
                {
                    Assert.IsNotNull(e2);
                    Assert.IsInstanceOfType(typeof(ArgumentException), e2);
                });
            });
        }
        [Test()]
        public void Test_PutIdentity_Without_Callback()
        {
            // for RegisterAsPseudoUser
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            KiiUser.RegisterAsPseudoUser(null, (KiiUser pseudoUser, Exception e1)=>{
                IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
                builder.WithEmail("kii_user01@gmail.com");
                builder.WithPhone("+819011112222");
                IdentityData identityData = builder.Build();

                UserFields userFields = new UserFields();
                userFields.Country = "JP";
                userFields["age"] = 30;

                try
                {
                    pseudoUser.PutIdentity(identityData, userFields, "password", null);
                    Assert.Fail("Exception is not thrown");
                } catch (ArgumentNullException)
                {
                }
                catch
                {
                    Assert.Fail("Unexpected Exception");
                }
            });
        }
        [Test(), ExpectedException(typeof(InvalidOperationException))]
        public void Test_PutIdentity_By_NotExistUser()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
            builder.WithEmail("kii_user01@gmail.com");
            builder.WithPhone("+819011112222");
            IdentityData identityData = builder.Build();

            UserFields userFields = new UserFields();
            userFields.Country = "JP";
            userFields["age"] = 30;

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/abcd"));
            user.PutIdentity(identityData, userFields, "123ABC");
        }
        [Test()]
        public void Test_PutIdentity_By_NotExistUser_Async()
        {
            // for RegisterAsPseudoUser
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
            builder.WithEmail("kii_user01@gmail.com");
            builder.WithPhone("+819011112222");
            IdentityData identityData = builder.Build();

            UserFields userFields = new UserFields();
            userFields.Country = "JP";
            userFields["age"] = 30;

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/abcd"));
            user.PutIdentity(identityData, userFields, "123ABC", (KiiUser u, Exception e2)=>
            {
                Assert.IsNotNull(e2);
                Assert.IsInstanceOfType(typeof(InvalidOperationException), e2);
            });
        }
        [Test()]
        public void Test_PutIdentity_By_NotLoginedUser()
        {
            // for RegisterAsPseudoUser
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            KiiUser.RegisterAsPseudoUser(null, (KiiUser pseudoUser, Exception e)=>{

                LogIn("other-user-00001");

                IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
                builder.WithEmail("kii_user01@gmail.com");
                builder.WithPhone("+819011112222");
                IdentityData identityData = builder.Build();

                UserFields userFields = new UserFields();
                userFields.Country = "JP";
                userFields["age"] = 30;

                // For refresh
                string response2 = @"
                {
                     ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81""
                }";
                client.AddResponse (200, response2);
                // for PutIdentity
                string response3 = @"
                {
                    ""modifiedAt"":123456789
                }";
                client.AddResponse (200, response3);

                try
                {
                    pseudoUser.PutIdentity(identityData, userFields, "123ABC");
                    Assert.Fail("Exception is not thrown");
                }
                catch (InvalidOperationException)
                {
                    // Test is success
                }
                catch
                {
                    Assert.Fail("Unexpected Exception");
                }
            });
        }
        [Test()]
        public void Test_PutIdentity_Async_By_NotLoginedUser()
        {
            // for RegisterAsPseudoUser
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""internalUserID"" : 111,
                 ""_accessToken"" : ""1234567890abcdefghijklmnopqrstuvwxyz""
            }
            ";
            client.AddResponse (201, response1);

            KiiUser.RegisterAsPseudoUser(null, (KiiUser pseudoUser, Exception e1)=>{

                LogIn("other-user-00001");

                IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
                builder.WithEmail("kii_user01@gmail.com");
                builder.WithPhone("+819011112222");
                IdentityData identityData = builder.Build();

                UserFields userFields = new UserFields();
                userFields.Country = "JP";
                userFields["age"] = 30;

                // For refresh
                string response4 = @"
                {
                     ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81""
                }";
                client.AddResponse (200, response4);
                // for PutIdentity
                string response5 = @"
                {
                    ""modifiedAt"":123456789
                }";
                client.AddResponse (200, response5);

                pseudoUser.PutIdentity(identityData, userFields, "123ABC", (KiiUser user, Exception e2)=>
                {
                    Assert.IsNotNull(e2);
                    Assert.IsInstanceOfType(typeof(InvalidOperationException), e2);
                });
            });
        }
        #endregion

        #region Update
        [Test()]
        public void Test_Update()
        {
            this.LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            // for Refresh
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""loginName"" : ""kii_user01""
            }
            ";
            client.AddResponse (200, response1);
            // for Update
            string response2 = @"
            {
                ""modifiedAt"":123456789
            }";
            client.AddResponse (200, response2);

            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
            builder.WithEmail("kii_user01@gmail.com");
            builder.WithPhone("+819011112222");
            IdentityData identityData = builder.Build();

            UserFields userFields = new UserFields();
            userFields.Country = "JP";
            userFields["age"] = 30;

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/0398e67a-818d-47ee-83fb-3519a6197b81"));
            user.SetReserveProperty("_hasPassword", true);
            user.Update(identityData, userFields);

            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [1]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/0398e67a-818d-47ee-83fb-3519a6197b81", client.RequestUrl [1]);
            Assert.AreEqual(identityData.UserName, user.Username);
            Assert.AreEqual(identityData.Email, user.Email);
            Assert.AreEqual(identityData.Phone, user.Phone);
        }
        [Test()]
        public void Test_Update_Async()
        {
            this.LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            // for Refresh
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""loginName"" : ""kii_user01""
            }
            ";
            client.AddResponse (200, response1);
            // for Update
            string response2 = @"
            {
                ""modifiedAt"":123456789
            }";
            client.AddResponse (200, response2);

            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
            builder.WithEmail("kii_user01@gmail.com");
            builder.WithPhone("+819011112222");
            IdentityData identityData = builder.Build();

            UserFields userFields = new UserFields();
            userFields.Country = "JP";
            userFields["age"] = 30;

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/0398e67a-818d-47ee-83fb-3519a6197b81"));
            user.Update(identityData, userFields, (KiiUser u, Exception e)=>
            {
                Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [1]);
                Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/0398e67a-818d-47ee-83fb-3519a6197b81", client.RequestUrl [1]);
                Assert.AreEqual(identityData.UserName, u.Username);
                Assert.AreEqual(identityData.Email, u.Email);
                Assert.AreEqual(identityData.Phone, u.Phone);
            });
        }
        [Test(), ExpectedException(typeof(ArgumentNullException))]
        public void Test_Update_Without_Callback()
        {
            this.LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            // for Refresh
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""loginName"" : ""kii_user01""
            }
            ";
            client.AddResponse (200, response1);
            // for Update
            string response2 = @"
            {
                ""modifiedAt"":123456789
            }";
            client.AddResponse (200, response2);

            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
            builder.WithEmail("kii_user01@gmail.com");
            builder.WithPhone("+819011112222");
            IdentityData identityData = builder.Build();

            UserFields userFields = new UserFields();
            userFields.Country = "JP";
            userFields["age"] = 30;

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/0398e67a-818d-47ee-83fb-3519a6197b81"));
            user.Update(identityData, userFields, null);
        }
        [Test(), ExpectedException(typeof(ArgumentNullException))]
        public void Test_Update_Without_IdentityData_And_UserFields()
        {
            this.LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            // for Refresh
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""loginName"" : ""kii_user01""
            }
            ";
            client.AddResponse (200, response1);
            // for Update
            string response2 = @"
            {
                ""modifiedAt"":123456789
            }";
            client.AddResponse (200, response2);

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/0398e67a-818d-47ee-83fb-3519a6197b81"));
            user.SetReserveProperty("_hasPassword", true);
            user.Update((IdentityData)null, (UserFields)null);
        }
        [Test()]
        public void Test_Update_Without_IdentityData_And_UserFields_Async()
        {
            this.LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            // for Refresh
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""loginName"" : ""kii_user01""
            }
            ";
            client.AddResponse (200, response1);
            // for Update
            string response2 = @"
            {
                ""modifiedAt"":123456789
            }";
            client.AddResponse (200, response2);

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/0398e67a-818d-47ee-83fb-3519a6197b81"));
            user.Update(null, null, (KiiUser u, Exception e)=>
            {
                Assert.IsNotNull(e);
                Assert.IsInstanceOfType(typeof(ArgumentNullException), e);
            });
        }
        [Test(), ExpectedException(typeof(ArgumentNullException))]
        public void Test_Update_Without_IdentityData_And_EmptyUserFields()
        {
            this.LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            // for Refresh
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""loginName"" : ""kii_user01""
            }
            ";
            client.AddResponse (200, response1);
            // for Update
            string response2 = @"
            {
                ""modifiedAt"":123456789
            }";
            client.AddResponse (200, response2);

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/0398e67a-818d-47ee-83fb-3519a6197b81"));
            user.SetReserveProperty("_hasPassword", true);
            user.Update((IdentityData)null, new UserFields());
        }
        [Test()]
        public void Test_Update_Without_IdentityData_And_EmptyUserFields_Async()
        {
            this.LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            // for Refresh
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""loginName"" : ""kii_user01""
            }
            ";
            client.AddResponse (200, response1);
            // for Update
            string response2 = @"
            {
                ""modifiedAt"":123456789
            }";
            client.AddResponse (200, response2);

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/0398e67a-818d-47ee-83fb-3519a6197b81"));
            user.Update(null, new UserFields(), (KiiUser u, Exception e)=>
            {
                Assert.IsNotNull(e);
                Assert.IsInstanceOfType(typeof(ArgumentNullException), e);
            });
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_UpdateWithIdentityDataByPseudoUser()
        {
            this.LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            // for Refresh
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81""
            }
            ";
            client.AddResponse (200, response1);

            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
            builder.WithEmail("kii_user01@gmail.com");
            builder.WithPhone("+819011112222");
            IdentityData identityData = builder.Build();

            UserFields userFields = new UserFields();
            userFields.Country = "JP";
            userFields["age"] = 30;

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/0398e67a-818d-47ee-83fb-3519a6197b81"));
            user.Update(identityData, userFields);
        }
        [Test()]
        public void Test_UpdateWithIdentityDataByPseudoUser_Async()
        {
            this.LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            // for Refresh
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81""
            }
            ";
            client.AddResponse (200, response1);

            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
            builder.WithEmail("kii_user01@gmail.com");
            builder.WithPhone("+819011112222");
            IdentityData identityData = builder.Build();

            UserFields userFields = new UserFields();
            userFields.Country = "JP";
            userFields["age"] = 30;

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/0398e67a-818d-47ee-83fb-3519a6197b81"));
            user.Update(identityData, userFields, (KiiUser u, Exception e) =>
            {
                Assert.IsNotNull(e);
                Assert.IsInstanceOfType(typeof(ArgumentException), e);
            });
        }
        [Test()]
        public void Test_Update_With_Local_Phone_With_Country()
        {
            this.LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            // for Refresh
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""loginName"" : ""kii_user01""
            }
            ";
            client.AddResponse (200, response1);
            // for Update
            string response2 = @"
            {
                ""modifiedAt"":123456789
            }";
            client.AddResponse (200, response2);

            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
            builder.WithEmail("kii_user01@gmail.com");
            builder.WithPhone("09011112222");
            IdentityData identityData = builder.Build();

            UserFields userFields = new UserFields();
            userFields["age"] = 30;
            userFields.Country = "JP";

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/0398e67a-818d-47ee-83fb-3519a6197b81"));
            user.Update(identityData, userFields);
            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [1]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/0398e67a-818d-47ee-83fb-3519a6197b81", client.RequestUrl [1]);
            Assert.AreEqual("09011112222", user.Phone);
            Assert.AreEqual("JP", user.Country);
            Assert.AreEqual(30, user.GetInt("age"));
        }
        [Test()]
        public void Test_Update_With_Local_Phone_With_Country_Async()
        {
            this.LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            // for Refresh
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""loginName"" : ""kii_user01""
            }
            ";
            client.AddResponse (200, response1);
            // for Update
            string response2 = @"
            {
                ""modifiedAt"":123456789
            }";
            client.AddResponse (200, response2);

            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
            builder.WithEmail("kii_user01@gmail.com");
            builder.WithPhone("09011112222");
            IdentityData identityData = builder.Build();

            UserFields userFields = new UserFields();
            userFields["age"] = 30;
            userFields.Country = "JP";

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/0398e67a-818d-47ee-83fb-3519a6197b81"));
            user.Update(identityData, userFields, (KiiUser u, Exception e)=>
            {
                Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [1]);
                Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/0398e67a-818d-47ee-83fb-3519a6197b81", client.RequestUrl [1]);
                Assert.AreEqual("09011112222", user.Phone);
                Assert.AreEqual("JP", user.Country);
                Assert.AreEqual(30, user.GetInt("age"));
            });
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_Update_With_Local_Phone_Without_Country()
        {
            this.LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            // for Refresh
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""loginName"" : ""kii_user01""
            }
            ";
            client.AddResponse (200, response1);
            // for Update
            string response2 = @"
            {
                ""modifiedAt"":123456789
            }";
            client.AddResponse (200, response2);

            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
            builder.WithEmail("kii_user01@gmail.com");
            builder.WithPhone("09011112222");
            IdentityData identityData = builder.Build();

            UserFields userFields = new UserFields();
            userFields["age"] = 30;

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/0398e67a-818d-47ee-83fb-3519a6197b81"));
            user.Update(identityData, userFields);
        }
        [Test()]
        public void Test_Update_With_Local_Phone_Without_Country_Async()
        {
            this.LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            // for Refresh
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""loginName"" : ""kii_user01""
            }
            ";
            client.AddResponse (200, response1);
            // for Update
            string response2 = @"
            {
                ""modifiedAt"":123456789
            }";
            client.AddResponse (200, response2);

            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
            builder.WithEmail("kii_user01@gmail.com");
            builder.WithPhone("09011112222");
            IdentityData identityData = builder.Build();

            UserFields userFields = new UserFields();
            userFields["age"] = 30;

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/0398e67a-818d-47ee-83fb-3519a6197b81"));
            user.Update(identityData, userFields, (KiiUser u, Exception e)=>
            {
                Assert.IsNotNull(e);
                Assert.IsInstanceOfType(typeof(ArgumentException), e);
            });
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_Update_With_Local_Phone_Without_Userfields()
        {
            this.LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            // for Refresh
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""loginName"" : ""kii_user01""
            }
            ";
            client.AddResponse (200, response1);
            // for Update
            string response2 = @"
            {
                ""modifiedAt"":123456789
            }";
            client.AddResponse (200, response2);

            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
            builder.WithEmail("kii_user01@gmail.com");
            builder.WithPhone("09011112222");
            IdentityData identityData = builder.Build();

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/0398e67a-818d-47ee-83fb-3519a6197b81"));
            user.Update(identityData, null);
        }
        [Test()]
        public void Test_Update_With_Local_Phone_Without_Userfields_Async()
        {
            this.LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            // for Refresh
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""loginName"" : ""kii_user01""
            }
            ";
            client.AddResponse (200, response1);
            // for Update
            string response2 = @"
            {
                ""modifiedAt"":123456789
            }";
            client.AddResponse (200, response2);

            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
            builder.WithEmail("kii_user01@gmail.com");
            builder.WithPhone("09011112222");
            IdentityData identityData = builder.Build();

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/0398e67a-818d-47ee-83fb-3519a6197b81"));
            user.Update(identityData, null, (KiiUser u, Exception e)=>
            {
                Assert.IsNotNull(e);
                Assert.IsInstanceOfType(typeof(ArgumentException), e);
            });
        }
        [Test(), ExpectedException(typeof(InvalidOperationException))]
        public void Test_Update_By_NotLoginedUser()
        {
            LogIn("other-user-00001");
            // for Refresh
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""loginName"" : ""kii_user01""
            }
            ";
            client.AddResponse (200, response1);
            // for Update
            string response2 = @"
            {
                ""modifiedAt"":123456789
            }";
            client.AddResponse (200, response2);

            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
            builder.WithEmail("kii_user01@gmail.com");
            builder.WithPhone("+819011112222");
            IdentityData identityData = builder.Build();

            UserFields userFields = new UserFields();
            userFields.Country = "JP";
            userFields["age"] = 30;

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/0398e67a-818d-47ee-83fb-3519a6197b81"));
            user.SetReserveProperty("_hasPassword", true);
            user.Update(identityData, userFields);

            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [1]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/0398e67a-818d-47ee-83fb-3519a6197b81", client.RequestUrl [1]);
            Assert.AreEqual(identityData.UserName, user.Username);
            Assert.AreEqual(identityData.Email, user.Email);
            Assert.AreEqual(identityData.Phone, user.Phone);
        }
        [Test()]
        public void Test_Update_Async_By_NotLoginedUser()
        {
            LogIn("other-user-00001");
            // for Refresh
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""loginName"" : ""kii_user01""
            }
            ";
            client.AddResponse (200, response1);
            // for Update
            string response2 = @"
            {
                ""modifiedAt"":123456789
            }";
            client.AddResponse (200, response2);

            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("kii_user01");
            builder.WithEmail("kii_user01@gmail.com");
            builder.WithPhone("+819011112222");
            IdentityData identityData = builder.Build();

            UserFields userFields = new UserFields();
            userFields.Country = "JP";
            userFields["age"] = 30;

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/0398e67a-818d-47ee-83fb-3519a6197b81"));
            user.Update(identityData, userFields, (KiiUser u, Exception e)=>
            {
                Assert.IsNotNull(e);
                Assert.IsInstanceOfType(typeof(InvalidOperationException), e);
            });
        }
        #endregion

        #region UpdateWithoutIdentityData
        [Test()]
        public void Test_UpdateWithoutIdentityDataByPseudoUser()
        {
            this.LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            // for Refresh
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81""
            }
            ";
            client.AddResponse (200, response1);
            // for Update
            string response2 = @"
            {
                ""modifiedAt"":123456789
            }";
            client.AddResponse (200, response2);

            UserFields userFields = new UserFields();
            userFields.Country = "JP";
            userFields["age"] = 30;

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/0398e67a-818d-47ee-83fb-3519a6197b81"));
            user.Update(userFields);

            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [1]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/0398e67a-818d-47ee-83fb-3519a6197b81", client.RequestUrl [1]);
            Assert.AreEqual("JP", user.Country);
            Assert.AreEqual(30, user.GetInt("age"));
        }

        [Test()]
        public void Test_UpdateWithoutIdentityDataByPseudoUser_Async()
        {
            this.LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            // for Refresh
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81""
            }
            ";
            client.AddResponse (200, response1);
            // for Update
            string response2 = @"
            {
                ""modifiedAt"":123456789
            }";
            client.AddResponse (200, response2);

            UserFields userFields = new UserFields();
            userFields.Country = "JP";
            userFields["age"] = 30;

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/0398e67a-818d-47ee-83fb-3519a6197b81"));
            user.Update(userFields, (KiiUser u, Exception e) =>
            {
                Assert.IsNull(e);
                Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [1]);
                Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/0398e67a-818d-47ee-83fb-3519a6197b81", client.RequestUrl [1]);
                Assert.AreEqual("JP", user.Country);
                Assert.AreEqual(30, user.GetInt("age"));
            });
        }

        [Test(), ExpectedException(typeof(ArgumentNullException))]
        public void Test_UpdateWithoutIdentityDataByPseudoUser_Without_Callback()
        {
            this.LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            // for Refresh
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""loginName"" : ""kii_user01""
            }
            ";
            client.AddResponse (200, response1);
            // for Update
            string response2 = @"
            {
                ""modifiedAt"":123456789
            }";
            client.AddResponse (200, response2);

            UserFields userFields = new UserFields();
            userFields.Country = "JP";
            userFields["age"] = 30;

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/0398e67a-818d-47ee-83fb-3519a6197b81"));
            user.Update(userFields, (KiiUserCallback)null);
        }

        [Test(), ExpectedException(typeof(ArgumentNullException))]
        public void Test_UpdateWithoutIdentityDataByPseudoUser_Without_UserFields()
        {
            this.LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            // for Refresh
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""loginName"" : ""kii_user01""
            }
            ";
            client.AddResponse (200, response1);
            // for Update
            string response2 = @"
            {
                ""modifiedAt"":123456789
            }";
            client.AddResponse (200, response2);

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/0398e67a-818d-47ee-83fb-3519a6197b81"));
            user.Update((UserFields)null);
        }

        [Test()]
        public void Test_UpdateWithoutIdentityDataByPseudoUser_Without_UserFields_Async()
        {
            this.LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            // for Refresh
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""loginName"" : ""kii_user01""
            }
            ";
            client.AddResponse (200, response1);
            // for Update
            string response2 = @"
            {
                ""modifiedAt"":123456789
            }";
            client.AddResponse (200, response2);

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/0398e67a-818d-47ee-83fb-3519a6197b81"));
            user.Update(null, (KiiUser u, Exception e) =>
            {
                Assert.IsNotNull(e);
                Assert.AreEqual(typeof(ArgumentNullException), e.GetType());
            });
        }

        [Test(), ExpectedException(typeof(ArgumentNullException))]
        public void Test_UpdateWithoutIdentityDataByPseudoUser_EmptyUserFields()
        {
            this.LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            // for Refresh
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""loginName"" : ""kii_user01""
            }
            ";
            client.AddResponse (200, response1);
            // for Update
            string response2 = @"
            {
                ""modifiedAt"":123456789
            }";
            client.AddResponse (200, response2);

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/0398e67a-818d-47ee-83fb-3519a6197b81"));
            user.Update(new UserFields());
        }

        [Test()]
        public void Test_UpdateWithoutIdentityDataByPseudoUser_EmptyUserFields_Async()
        {
            this.LogIn("0398e67a-818d-47ee-83fb-3519a6197b81");
            // for Refresh
            string response1 = @"
            {
                 ""userID"" : ""0398e67a-818d-47ee-83fb-3519a6197b81"",
                 ""loginName"" : ""kii_user01""
            }
            ";
            client.AddResponse (200, response1);
            // for Update
            string response2 = @"
            {
                ""modifiedAt"":123456789
            }";
            client.AddResponse (200, response2);

            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/0398e67a-818d-47ee-83fb-3519a6197b81"));
            user.Update(new UserFields(), (KiiUser u, Exception e) =>
            {
                Assert.IsNotNull(e);
                Assert.AreEqual(typeof(ArgumentNullException), e.GetType());
            });
        }
        #endregion
    }
}
