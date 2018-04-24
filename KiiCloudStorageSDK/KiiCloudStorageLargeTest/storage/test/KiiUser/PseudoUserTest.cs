using System;
using NUnit.Framework;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture ()]
    public class PseudoUserTest : LargeTestBase
    {
        private KiiUser existingUser = null;

        [SetUp()]
        public override void SetUp ()
        {
            base.SetUp ();
            string unixTime = CurrentTimeMillis().ToString();
            string username = "kii_user" + unixTime;
            string email = username + "@kii.com";
            string phone = "+874" + unixTime.Substring(unixTime.Length - 9, 9);
            existingUser = KiiUser.BuilderWithName(username).WithEmail(email).WithPhone(phone).Build();
            existingUser.Register("password");
        }
        [TearDown()]
        public override void TearDown ()
        {
            base.TearDown ();
        }
        private long CurrentTimeMillis ()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        #region RegisterAsPseudoUser
        [Test()]
        public void RegisterAsPseudoUserTest ()
        {
            KiiUser.RegisterAsPseudoUser(null);
            Assert.IsNotNull(KiiUser.AccessToken);
            Assert.IsNull(KiiUser.CurrentUser.Displayname);
            Assert.IsNull(KiiUser.CurrentUser.Country);
            Assert.IsTrue(KiiUser.AccessToken.Length > 10);
            Assert.IsTrue(KiiUser.CurrentUser.IsPseudoUser);
        }
        [Test()]
        public void RegisterAsPseudoUserWithUserFieldsTest ()
        {
            UserFields userFields = new UserFields();
            userFields.Displayname = "LargeTestUser";
            KiiUser.RegisterAsPseudoUser(userFields);
            Assert.IsNotNull(KiiUser.AccessToken);
            Assert.IsTrue(KiiUser.AccessToken.Length > 10);
            Assert.IsTrue(KiiUser.CurrentUser.IsPseudoUser);
            Assert.AreEqual(userFields.Displayname, KiiUser.CurrentUser.Displayname);
            Assert.IsNull(KiiUser.CurrentUser.Country);
        }
        [Test()]
        public void RegisterAsPseudoUserTest_Async ()
        {
            CountDownLatch cd = new CountDownLatch (1);

            KiiUser.RegisterAsPseudoUser(null, (KiiUser user, Exception e)=>{
                cd.Signal();
            });

            if (!cd.Wait(new TimeSpan(0, 0, 0, 10)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNotNull(KiiUser.AccessToken);
            Assert.IsNull(KiiUser.CurrentUser.Displayname);
            Assert.IsNull(KiiUser.CurrentUser.Country);
            Assert.IsTrue(KiiUser.AccessToken.Length > 10);
            Assert.IsTrue(KiiUser.CurrentUser.IsPseudoUser);
        }
        [Test()]
        public void RegisterAsPseudoUserWithUserFieldsTest_Async ()
        {
            CountDownLatch cd = new CountDownLatch (1);

            UserFields userFields = new UserFields();
            userFields.Displayname = "LargeTestUser";
            KiiUser.RegisterAsPseudoUser(userFields, (KiiUser user, Exception e)=>{
                cd.Signal();
            });

            if (!cd.Wait(new TimeSpan(0, 0, 0, 10)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNotNull(KiiUser.AccessToken);
            Assert.IsTrue(KiiUser.AccessToken.Length > 10);
            Assert.IsTrue(KiiUser.CurrentUser.IsPseudoUser);
            Assert.AreEqual(userFields.Displayname, KiiUser.CurrentUser.Displayname);
            Assert.IsNull(KiiUser.CurrentUser.Country);
        }
        #endregion

        #region PutIdentity
        [Test()]
        public void PutIdentityTest ()
        {
            string unixTime = CurrentTimeMillis().ToString();
            string username = "kii_user" + unixTime;
            string email = username + "@kii.com";
            string phone = "+874" + unixTime.Substring(unixTime.Length - 9, 9);

            KiiUser.RegisterAsPseudoUser(null);
            IdentityData.Builder builder = IdentityData.Builder.CreateWithName(username);
            builder.WithEmail(email);
            builder.WithPhone(phone);
            IdentityData identityData = builder.Build();

            KiiUser.CurrentUser.PutIdentity(identityData, null, "123ABC");

            Assert.IsFalse(KiiUser.CurrentUser.IsPseudoUser);
            Assert.AreEqual(username, KiiUser.CurrentUser.Username);
            Assert.AreEqual(email, KiiUser.CurrentUser.Email);
            Assert.AreEqual(phone, KiiUser.CurrentUser.Phone);
            Assert.IsNull(KiiUser.CurrentUser.Displayname);
            Assert.IsNull(KiiUser.CurrentUser.Country);
            Assert.IsFalse(KiiUser.CurrentUser.Has("age"));
        }
        [Test()]
        public void PutIdentityTest_Async ()
        {
            CountDownLatch cd = new CountDownLatch (1);

            string unixTime = CurrentTimeMillis().ToString();
            string username = "kii_user" + unixTime;
            string email = username + "@kii.com";
            string phone = "+874" + unixTime.Substring(unixTime.Length - 9, 9);
            KiiUser actual = null;

            KiiUser.RegisterAsPseudoUser(null, (KiiUser registeredUser, Exception e1)=>{
                IdentityData.Builder builder = IdentityData.Builder.CreateWithName(username);
                builder.WithEmail(email);
                builder.WithPhone(phone);
                IdentityData identityData = builder.Build();

                registeredUser.PutIdentity(identityData, null, "123ABC", (KiiUser user, Exception e2) => {
                    actual = user;
                    cd.Signal();
                });
            });

            if (!cd.Wait(new TimeSpan(0, 0, 0, 10)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsFalse(actual.IsPseudoUser);
            Assert.AreEqual(username, actual.Username);
            Assert.AreEqual(email, actual.Email);
            Assert.AreEqual(phone, actual.Phone);
            Assert.IsNull(actual.Displayname);
            Assert.IsNull(actual.Country);
            Assert.IsFalse(actual.Has("age"));
        }
        [Test()]
        public void PutIdentityWithUserFieldsTest ()
        {
            string unixTime = CurrentTimeMillis().ToString();
            string username = "kii_user" + unixTime;
            string email = username + "@kii.com";
            string phone = "+874" + unixTime.Substring(unixTime.Length - 9, 9);

            KiiUser.RegisterAsPseudoUser(null);
            IdentityData.Builder builder = IdentityData.Builder.CreateWithName(username);
            builder.WithEmail(email);
            builder.WithPhone(phone);
            IdentityData identityData = builder.Build();

            UserFields userFields = new UserFields();
            userFields.Displayname = "disp";
            userFields.Country = "JP";
            userFields["age"] = 30;

            KiiUser.CurrentUser.PutIdentity(identityData, userFields, "123ABC");

            Assert.IsFalse(KiiUser.CurrentUser.IsPseudoUser);
            Assert.AreEqual(username, KiiUser.CurrentUser.Username);
            Assert.AreEqual(email, KiiUser.CurrentUser.Email);
            Assert.AreEqual(phone, KiiUser.CurrentUser.Phone);
            Assert.AreEqual("disp", KiiUser.CurrentUser.Displayname);
            Assert.AreEqual("JP", KiiUser.CurrentUser.Country);
            Assert.AreEqual(30, KiiUser.CurrentUser.GetInt("age"));
        }
        [Test()]
        public void PutIdentityWithUserFieldsTest_Async ()
        {
            CountDownLatch cd = new CountDownLatch (1);

            string unixTime = CurrentTimeMillis().ToString();
            string username = "kii_user" + unixTime;
            string email = username + "@kii.com";
            string phone = "+874" + unixTime.Substring(unixTime.Length - 9, 9);
            KiiUser actual = null;

            KiiUser.RegisterAsPseudoUser(null, (KiiUser registeredUser, Exception e1)=>{
                IdentityData.Builder builder = IdentityData.Builder.CreateWithName(username);
                builder.WithEmail(email);
                builder.WithPhone(phone);
                IdentityData identityData = builder.Build();

                UserFields userFields = new UserFields();
                userFields.Displayname = "disp";
                userFields.Country = "JP";
                userFields["age"] = 30;

                registeredUser.PutIdentity(identityData, userFields, "123ABC", (KiiUser user, Exception e2) => {
                    actual = user;
                    cd.Signal();
                });
            });

            if (!cd.Wait(new TimeSpan(0, 0, 0, 10)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsFalse(actual.IsPseudoUser);
            Assert.AreEqual(username, actual.Username);
            Assert.AreEqual(email, actual.Email);
            Assert.AreEqual(phone, actual.Phone);
            Assert.AreEqual("disp", actual.Displayname);
            Assert.AreEqual("JP", actual.Country);
            Assert.AreEqual(30, actual.GetInt("age"));
        }
        [Test()]
        public void PutIdentityWithRemoveFieldsTest ()
        {
            string unixTime = CurrentTimeMillis().ToString();
            string username = "kii_user" + unixTime;
            string email = username + "@kii.com";
            string phone = "+874" + unixTime.Substring(unixTime.Length - 9, 9);

            UserFields userFields = new UserFields();
            userFields.Country = "US";
            userFields.Displayname = "disp";
            userFields["birthday"] = "1978/7/22";
            userFields["age"] = 30;

            KiiUser.RegisterAsPseudoUser(userFields);
            IdentityData.Builder builder = IdentityData.Builder.CreateWithName(username);
            builder.WithEmail(email);
            builder.WithPhone(phone);
            IdentityData identityData = builder.Build();

            UserFields removeFields = new UserFields();
            removeFields.RemoveDisplayname(); // remove displayname only local.
            removeFields.RemoveFromServer("birthday");
            removeFields["newFields"] = "new!!";

            KiiUser.CurrentUser.PutIdentity(identityData, removeFields, "123ABC");

            Assert.IsFalse(KiiUser.CurrentUser.IsPseudoUser);
            Assert.AreEqual(username, KiiUser.CurrentUser.Username);
            Assert.AreEqual(email, KiiUser.CurrentUser.Email);
            Assert.AreEqual(phone, KiiUser.CurrentUser.Phone);
            Assert.AreEqual("disp", KiiUser.CurrentUser.Displayname);
            Assert.AreEqual("US", KiiUser.CurrentUser.Country);
            Assert.IsFalse(KiiUser.CurrentUser.Has("birthday"));
            Assert.AreEqual(30, KiiUser.CurrentUser.GetInt("age"));
        }
        [Test()]
        public void PutIdentityWithRemoveFieldsTest_Async ()
        {
            CountDownLatch cd = new CountDownLatch (1);

            string unixTime = CurrentTimeMillis().ToString();
            string username = "kii_user" + unixTime;
            string email = username + "@kii.com";
            string phone = "+874" + unixTime.Substring(unixTime.Length - 9, 9);
            KiiUser actual = null;

            UserFields userFields = new UserFields();
            userFields.Country = "US";
            userFields.Displayname = "disp";
            userFields["birthday"] = "1978/7/22";
            userFields["age"] = 30;

            KiiUser.RegisterAsPseudoUser(userFields, (KiiUser registeredUser, Exception e1)=>{
                IdentityData.Builder builder = IdentityData.Builder.CreateWithName(username);
                builder.WithEmail(email);
                builder.WithPhone(phone);
                IdentityData identityData = builder.Build();

                UserFields removeFields = new UserFields();
                removeFields.RemoveDisplayname(); // remove displayname only local.
                removeFields.RemoveFromServer("birthday");
                removeFields["newFields"] = "new!!";

                registeredUser.PutIdentity(identityData, removeFields, "123ABC", (KiiUser user, Exception e2) => {
                    actual = user;
                    cd.Signal();
                });
            });

            if (!cd.Wait(new TimeSpan(0, 0, 0, 10)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsFalse(actual.IsPseudoUser);
            Assert.AreEqual(username, actual.Username);
            Assert.AreEqual(email, actual.Email);
            Assert.AreEqual(phone, actual.Phone);
            Assert.AreEqual("disp", actual.Displayname);
            Assert.AreEqual("US", actual.Country);
            Assert.IsFalse(actual.Has("birthday"));
            Assert.AreEqual(30, actual.GetInt("age"));
        }
        [Test(), ExpectedException(typeof(ConflictException))]
        public void PutIdentityWithExistingNameTest ()
        {
            KiiUser.RegisterAsPseudoUser(null);
            IdentityData.Builder builder = IdentityData.Builder.CreateWithName(this.existingUser.Username);
            IdentityData identityData = builder.Build();

            KiiUser.CurrentUser.PutIdentity(identityData, null, "123ABC");
        }
        [Test()]
        public void PutIdentityWithExistingNameTest_Async ()
        {
            CountDownLatch cd = new CountDownLatch (1);
            Exception e = null;
            KiiUser.RegisterAsPseudoUser(null, (KiiUser registeredUser, Exception e1)=>{
                IdentityData.Builder builder = IdentityData.Builder.CreateWithName(this.existingUser.Username);
                IdentityData identityData = builder.Build();
                registeredUser.PutIdentity(identityData, null, "123ABC", (KiiUser user, Exception e2) => {
                    e = e2;
                    cd.Signal();
                });
            });

            if (!cd.Wait(new TimeSpan(0, 0, 0, 10)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNotNull(e);
            Assert.IsInstanceOfType(typeof(ConflictException), e);
        }
        [Test(), ExpectedException(typeof(ConflictException))]
        public void PutIdentityWithExistingEmailTest ()
        {
            KiiUser.RegisterAsPseudoUser(null);
            IdentityData.Builder builder = IdentityData.Builder.CreateWithEmail(this.existingUser.Email);
            IdentityData identityData = builder.Build();

            KiiUser.CurrentUser.PutIdentity(identityData, null, "123ABC");
        }
        [Test()]
        public void PutIdentityWithExistingEmailTest_Async ()
        {
            CountDownLatch cd = new CountDownLatch (1);
            Exception e = null;
            KiiUser.RegisterAsPseudoUser(null, (KiiUser registeredUser, Exception e1)=>{
                IdentityData.Builder builder = IdentityData.Builder.CreateWithEmail(this.existingUser.Email);
                IdentityData identityData = builder.Build();
                registeredUser.PutIdentity(identityData, null, "123ABC", (KiiUser user, Exception e2) => {
                    e = e2;
                    cd.Signal();
                });
            });

            if (!cd.Wait(new TimeSpan(0, 0, 0, 10)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNotNull(e);
            Assert.IsInstanceOfType(typeof(ConflictException), e);
        }
        [Test(), ExpectedException(typeof(ConflictException))]
        public void PutIdentityWithExistingPhoneTest ()
        {
            KiiUser.RegisterAsPseudoUser(null);
            IdentityData.Builder builder = IdentityData.Builder.CreateWithPhone(this.existingUser.Phone);
            IdentityData identityData = builder.Build();

            KiiUser.CurrentUser.PutIdentity(identityData, null, "123ABC");
        }
        [Test()]
        public void PutIdentityWithExistingPhoneTest_Async ()
        {
            CountDownLatch cd = new CountDownLatch (1);
            Exception e = null;
            KiiUser.RegisterAsPseudoUser(null, (KiiUser registeredUser, Exception e1)=>{
                IdentityData.Builder builder = IdentityData.Builder.CreateWithPhone(this.existingUser.Phone);
                IdentityData identityData = builder.Build();
                registeredUser.PutIdentity(identityData, null, "123ABC", (KiiUser user, Exception e2) => {
                    e = e2;
                    cd.Signal();
                });
            });

            if (!cd.Wait(new TimeSpan(0, 0, 0, 10)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNotNull(e);
            Assert.IsInstanceOfType(typeof(ConflictException), e);
        }
        [Test(), ExpectedException(typeof(ForbiddenException))]
        public void PutIdentityByDeletedUserTest ()
        {
            string unixTime = CurrentTimeMillis().ToString();
            string username = "kii_user" + unixTime;
            string email = username + "@kii.com";
            string phone = "+874" + unixTime.Substring(unixTime.Length - 9, 9);

            KiiUser.RegisterAsPseudoUser(null);
            IdentityData.Builder builder = IdentityData.Builder.CreateWithName(username);
            builder.WithEmail(email);
            builder.WithPhone(phone);
            IdentityData identityData = builder.Build();

            string currentUserId = Kii.CurrentUser.ID;
            string currentUserAccessToken = KiiUser.AccessToken;
            KiiUser.CurrentUser.Delete();
            Kii.CurrentUser = KiiUser.GetById(currentUserId);
            KiiCloudEngine.UpdateAccessToken(currentUserAccessToken);

            KiiUser.CurrentUser.PutIdentity(identityData, null, "123ABC");
        }
        [Test()]
        public void PutIdentityByDeletedUserTest_Async ()
        {
            CountDownLatch cd = new CountDownLatch (1);

            string unixTime = CurrentTimeMillis().ToString();
            string username = "kii_user" + unixTime;
            string email = username + "@kii.com";
            string phone = "+874" + unixTime.Substring(unixTime.Length - 9, 9);
            Exception e = null;

            KiiUser.RegisterAsPseudoUser(null, (KiiUser registeredUser, Exception e1)=>{
                IdentityData.Builder builder = IdentityData.Builder.CreateWithName(username);
                builder.WithEmail(email);
                builder.WithPhone(phone);
                IdentityData identityData = builder.Build();

                string currentUserId = Kii.CurrentUser.ID;
                string currentUserAccessToken = KiiUser.AccessToken;
                KiiUser.CurrentUser.Delete();
                Kii.CurrentUser = KiiUser.GetById(currentUserId);
                KiiCloudEngine.UpdateAccessToken(currentUserAccessToken);

                Kii.CurrentUser.PutIdentity(identityData, null, "123ABC", (KiiUser user, Exception e2) => {
                    e = e2;
                    cd.Signal();
                });
            });

            if (!cd.Wait(new TimeSpan(0, 0, 0, 10)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNotNull(e);
            Assert.IsInstanceOfType(typeof(ForbiddenException), e);
        }
        [Test(), ExpectedException(typeof(AlreadyHasIdentityException))]
        public void PutIdentityByUnnoticedNormalUserTest ()
        {
            string unixTime = CurrentTimeMillis().ToString();
            string username = "kii_user" + unixTime;
            string email = username + "@kii.com";
            string phone = "+874" + unixTime.Substring(unixTime.Length - 9, 9);

            KiiUser user = KiiUser.BuilderWithName(username).WithEmail(email).WithPhone(phone).Build();
            user.Register("password");

            KiiUser unnoticedNormalUser = KiiUser.CreateByUri(user.Uri);

            IdentityData.Builder builder = IdentityData.Builder.CreateWithName(username);
            builder.WithEmail(email);
            builder.WithPhone(phone);
            IdentityData identityData = builder.Build();

            unnoticedNormalUser.PutIdentity(identityData, null, "123ABC");
        }
        [Test()]
        public void PutIdentityByUnnoticedNormalUserTest_Async ()
        {
            CountDownLatch cd = new CountDownLatch (1);

            string unixTime = CurrentTimeMillis().ToString();
            string username = "kii_user" + unixTime;
            string email = username + "@kii.com";
            string phone = "+874" + unixTime.Substring(unixTime.Length - 9, 9);
            Exception e = null;

            KiiUser user = KiiUser.BuilderWithName(username).WithEmail(email).WithPhone(phone).Build();
            user.Register("password");

            KiiUser unnoticedNormalUser = KiiUser.CreateByUri(user.Uri);

            IdentityData.Builder builder = IdentityData.Builder.CreateWithName(username);
            builder.WithEmail(email);
            builder.WithPhone(phone);
            IdentityData identityData = builder.Build();

            unnoticedNormalUser.PutIdentity(identityData, null, "123ABC", (KiiUser u, Exception e2) => {
                e = e2;
                cd.Signal();
            });

            if (!cd.Wait(new TimeSpan(0, 0, 0, 10)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNotNull(e);
            Assert.IsInstanceOfType(typeof(AlreadyHasIdentityException), e);
        }
        #endregion

        #region
        [Test()]
        public void UpdateTest ()
        {
            string unixTime = CurrentTimeMillis().ToString();
            string username = "kii_user" + unixTime;
            string email = username + "@kii.com";
            string phone = "+874" + unixTime.Substring(unixTime.Length - 9, 9);

            KiiUser newUser = KiiUser.BuilderWithName(username).Build();
            newUser["birthday"] = "1978/7/22";
            newUser.Register("123ABC");

            IdentityData.Builder builder = IdentityData.Builder.CreateWithEmail(email);
            builder.WithPhone(phone);
            IdentityData identityData = builder.Build();

            UserFields userFields = new UserFields();
            userFields.Displayname = "disp";
            userFields.Country = "JP";
            userFields["age"] = 30;
            userFields.RemoveFromServer("birthday");

            newUser.Update(identityData, userFields);

            Assert.IsFalse(newUser.IsPseudoUser);
            Assert.AreEqual(username, newUser.Username);
            Assert.AreEqual(email, newUser.Email);
            Assert.AreEqual(phone, newUser.Phone);
            Assert.AreEqual("disp", newUser.Displayname);
            Assert.AreEqual("JP", newUser.Country);
            Assert.IsFalse(newUser.Has("birthday"));
            Assert.AreEqual(30, newUser.GetInt("age"));
        }
        [Test()]
        public void UpdateTest_Async ()
        {
            CountDownLatch cd = new CountDownLatch (1);

            string unixTime = CurrentTimeMillis().ToString();
            string username = "kii_user" + unixTime;
            string email = username + "@kii.com";
            string phone = "+874" + unixTime.Substring(unixTime.Length - 9, 9);
            KiiUser actual = null;

            KiiUser newUser = KiiUser.BuilderWithName(username).Build();
            newUser["birthday"] = "1978/7/22";
            newUser.Register("123ABC", (KiiUser registeredUser, Exception e1) => {
                IdentityData.Builder builder = IdentityData.Builder.CreateWithEmail(email);
                builder.WithPhone(phone);
                IdentityData identityData = builder.Build();

                UserFields userFields = new UserFields();
                userFields.Displayname = "disp";
                userFields.Country = "JP";
                userFields["age"] = 30;
                userFields.RemoveFromServer("birthday");

                registeredUser.Update(identityData, userFields, (KiiUser updatedUser, Exception e2) => {
                    actual = updatedUser;
                    cd.Signal();
                });
            });

            if (!cd.Wait(new TimeSpan(0, 0, 0, 10)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsFalse(actual.IsPseudoUser);
            Assert.AreEqual(username, actual.Username);
            Assert.AreEqual(email, actual.Email);
            Assert.AreEqual(phone, actual.Phone);
            Assert.AreEqual("disp", actual.Displayname);
            Assert.AreEqual("JP", actual.Country);
            Assert.IsFalse(actual.Has("birthday"));
            Assert.AreEqual(30, actual.GetInt("age"));
        }
        [Test()]
        public void UpdateByPseudoUserTest ()
        {
            KiiUser.RegisterAsPseudoUser(null);

            UserFields userFields = new UserFields();
            userFields.Displayname = "disp";
            userFields.Country = "JP";
            userFields["age"] = 30;

            KiiUser.CurrentUser.Update(userFields);

            Assert.IsTrue(KiiUser.CurrentUser.IsPseudoUser);
            Assert.IsNull(KiiUser.CurrentUser.Username);
            Assert.IsNull(KiiUser.CurrentUser.Email);
            Assert.IsNull(KiiUser.CurrentUser.Phone);
            Assert.AreEqual("disp", KiiUser.CurrentUser.Displayname);
            Assert.AreEqual("JP", KiiUser.CurrentUser.Country);
            Assert.AreEqual(30, KiiUser.CurrentUser.GetInt("age"));
        }
        [Test()]
        public void UpdateWithoutIdentityDataByPseudoUserTest ()
        {
            KiiUser.RegisterAsPseudoUser(null);

            UserFields userFields = new UserFields();
            userFields.Displayname = "disp";
            userFields.Country = "JP";
            userFields["age"] = 30;

            KiiUser.CurrentUser.Update(null, userFields);

            Assert.IsTrue(KiiUser.CurrentUser.IsPseudoUser);
            Assert.IsNull(KiiUser.CurrentUser.Username);
            Assert.IsNull(KiiUser.CurrentUser.Email);
            Assert.IsNull(KiiUser.CurrentUser.Phone);
            Assert.AreEqual("disp", KiiUser.CurrentUser.Displayname);
            Assert.AreEqual("JP", KiiUser.CurrentUser.Country);
            Assert.AreEqual(30, KiiUser.CurrentUser.GetInt("age"));
        }
        [Test()]
        public void UpdateByPseudoUserTest_Async ()
        {
            CountDownLatch cd = new CountDownLatch (1);

            KiiUser actual = null;

            KiiUser.RegisterAsPseudoUser(null, (KiiUser registeredUser, Exception e1)=>{
                UserFields userFields = new UserFields();
                userFields.Displayname = "disp";
                userFields.Country = "JP";
                userFields["age"] = 30;

                registeredUser.Update(userFields, (KiiUser updatedUser, Exception e2) => {
                    actual = updatedUser;
                    cd.Signal();
                });
            });

            if (!cd.Wait(new TimeSpan(0, 0, 0, 10)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsTrue(actual.IsPseudoUser);
            Assert.IsNull(actual.Username);
            Assert.IsNull(actual.Email);
            Assert.IsNull(actual.Phone);
            Assert.AreEqual("disp", actual.Displayname);
            Assert.AreEqual("JP", actual.Country);
            Assert.AreEqual(30, actual.GetInt("age"));
        }
        [Test()]
        public void UpdateWithoutIdentityDataByPseudoUserTest_Async ()
        {
            CountDownLatch cd = new CountDownLatch (1);

            KiiUser actual = null;

            KiiUser.RegisterAsPseudoUser(null, (KiiUser registeredUser, Exception e1)=>{
                UserFields userFields = new UserFields();
                userFields.Displayname = "disp";
                userFields.Country = "JP";
                userFields["age"] = 30;

                registeredUser.Update(null, userFields, (KiiUser updatedUser, Exception e2) => {
                    actual = updatedUser;
                    cd.Signal();
                });
            });

            if (!cd.Wait(new TimeSpan(0, 0, 0, 10)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsTrue(actual.IsPseudoUser);
            Assert.IsNull(actual.Username);
            Assert.IsNull(actual.Email);
            Assert.IsNull(actual.Phone);
            Assert.AreEqual("disp", actual.Displayname);
            Assert.AreEqual("JP", actual.Country);
            Assert.AreEqual(30, actual.GetInt("age"));
        }
        [Test(), ExpectedException(typeof(ForbiddenException))]
        public void UpdateByDeletedPseudoUserTest ()
        {
            KiiUser.RegisterAsPseudoUser(null);

            UserFields userFields = new UserFields();
            userFields.Displayname = "disp";
            userFields.Country = "JP";
            userFields["age"] = 30;
            userFields.RemoveFromServer("birthday");

            string currentUserId = Kii.CurrentUser.ID;
            string currentUserAccessToken = KiiUser.AccessToken;
            KiiUser.CurrentUser.Delete();
            Kii.CurrentUser = KiiUser.GetById(currentUserId);
            KiiCloudEngine.UpdateAccessToken(currentUserAccessToken);

            KiiUser.CurrentUser.Update(userFields);
        }
        [Test()]
        public void UpdateByDeletedPseudoUserTest_Async ()
        {
            CountDownLatch cd = new CountDownLatch (1);
            Exception e = null;

            KiiUser.RegisterAsPseudoUser(null, (KiiUser registeredUser, Exception e1)=>{
                string currentUserId = Kii.CurrentUser.ID;
                string currentUserAccessToken = KiiUser.AccessToken;
                KiiUser.CurrentUser.Delete();
                Kii.CurrentUser = KiiUser.GetById(currentUserId);
                KiiCloudEngine.UpdateAccessToken(currentUserAccessToken);

                UserFields userFields = new UserFields();
                userFields.Displayname = "disp";
                userFields.Country = "JP";
                userFields["age"] = 30;
                userFields.RemoveFromServer("birthday");

                Kii.CurrentUser.Update(null, userFields, (KiiUser updatedUser, Exception e2) => {
                    e = e2;
                    cd.Signal();
                });
            });

            if (!cd.Wait(new TimeSpan(0, 0, 0, 10)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNotNull(e);
            Assert.IsInstanceOfType(typeof(ForbiddenException), e);
        }
        [Test()]
        public void UpdateByUnnoticedNormalUserTest ()
        {
            string unixTime = CurrentTimeMillis().ToString();
            string username = "kii_user" + unixTime;
            string email = username + "@kii.com";
            string phone = "+874" + unixTime.Substring(unixTime.Length - 9, 9);

            KiiUser user = KiiUser.BuilderWithName(username).Build();
            user.Register("password");

            KiiUser unnoticedNormalUser = KiiUser.CreateByUri(user.Uri);

            IdentityData.Builder builder = IdentityData.Builder.CreateWithName(username);
            builder.WithEmail(email);
            builder.WithPhone(phone);
            IdentityData identityData = builder.Build();

            unnoticedNormalUser.Update(identityData, null);

            Assert.AreEqual(email, unnoticedNormalUser.Email);
            Assert.AreEqual(phone, unnoticedNormalUser.Phone);
        }
        [Test()]
        public void UpdateByUnnoticedNormalUserTest_Async ()
        {
            CountDownLatch cd = new CountDownLatch (1);

            string unixTime = CurrentTimeMillis().ToString();
            string username = "kii_user" + unixTime;
            string email = username + "@kii.com";
            string phone = "+874" + unixTime.Substring(unixTime.Length - 9, 9);

            KiiUser user = KiiUser.BuilderWithName(username).Build();
            user.Register("password");

            KiiUser unnoticedNormalUser = KiiUser.CreateByUri(user.Uri);

            IdentityData.Builder builder = IdentityData.Builder.CreateWithName(username);
            builder.WithEmail(email);
            builder.WithPhone(phone);
            IdentityData identityData = builder.Build();

            KiiUser updatedUser = null;

            unnoticedNormalUser.Update(identityData, null, (KiiUser u, Exception e2) => {
                updatedUser = u;
                cd.Signal();
            });

            if (!cd.Wait(new TimeSpan(0, 0, 0, 10)))
            {
                Assert.Fail("Callback not fired.");
            }

            Assert.AreEqual(email, updatedUser.Email);
            Assert.AreEqual(phone, updatedUser.Phone);
        }
        [Test(), ExpectedException(typeof(ConflictException))]
        public void UpdateWithExistingNameTest ()
        {
            string unixTime = CurrentTimeMillis().ToString();
            string username = "kii_user" + unixTime;
            KiiUser newUser = KiiUser.BuilderWithName(username).Build();
            newUser.Register("123ABC");

            IdentityData.Builder builder = IdentityData.Builder.CreateWithName(this.existingUser.Username);
            IdentityData identityData = builder.Build();

            newUser.Update(identityData, null);
        }
        [Test()]
        public void UpdateWithExistingNameTest_Async ()
        {
            string unixTime = CurrentTimeMillis().ToString();
            string username = "kii_user" + unixTime;
            KiiUser newUser = KiiUser.BuilderWithName(username).Build();
            newUser.Register("123ABC");

            CountDownLatch cd = new CountDownLatch (1);
            Exception e = null;
            IdentityData.Builder builder = IdentityData.Builder.CreateWithName(this.existingUser.Username);
            IdentityData identityData = builder.Build();
            newUser.Update(identityData, null, (KiiUser user, Exception e2) => {
                e = e2;
                cd.Signal();
            });

            if (!cd.Wait(new TimeSpan(0, 0, 0, 10)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNotNull(e);
            Assert.IsInstanceOfType(typeof(ConflictException), e);
        }
        [Test(), ExpectedException(typeof(ConflictException))]
        public void UpdateWithExistingEmailTest ()
        {
            string unixTime = CurrentTimeMillis().ToString();
            string username = "kii_user" + unixTime;
            KiiUser newUser = KiiUser.BuilderWithName(username).Build();
            newUser.Register("123ABC");

            IdentityData.Builder builder = IdentityData.Builder.CreateWithEmail(this.existingUser.Email);
            IdentityData identityData = builder.Build();

            newUser.Update(identityData, null);
        }
        [Test()]
        public void UpdateWithExistingEmailTest_Async ()
        {
            string unixTime = CurrentTimeMillis().ToString();
            string username = "kii_user" + unixTime;
            KiiUser newUser = KiiUser.BuilderWithName(username).Build();
            newUser.Register("123ABC");

            CountDownLatch cd = new CountDownLatch (1);
            Exception e = null;
            IdentityData.Builder builder = IdentityData.Builder.CreateWithEmail(this.existingUser.Email);
            IdentityData identityData = builder.Build();
            newUser.Update(identityData, null, (KiiUser user, Exception e2) => {
                e = e2;
                cd.Signal();
            });

            if (!cd.Wait(new TimeSpan(0, 0, 0, 10)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNotNull(e);
            Assert.IsInstanceOfType(typeof(ConflictException), e);
        }
        [Test(), ExpectedException(typeof(ConflictException))]
        public void UpdateWithExistingPhoneTest ()
        {
            string unixTime = CurrentTimeMillis().ToString();
            string username = "kii_user" + unixTime;
            KiiUser newUser = KiiUser.BuilderWithName(username).Build();
            newUser.Register("123ABC");

            IdentityData.Builder builder = IdentityData.Builder.CreateWithPhone(this.existingUser.Phone);
            IdentityData identityData = builder.Build();

            newUser.Update(identityData, null);
        }
        [Test()]
        public void UpdateWithExistingPhoneTest_Async ()
        {
            string unixTime = CurrentTimeMillis().ToString();
            string username = "kii_user" + unixTime;
            KiiUser newUser = KiiUser.BuilderWithName(username).Build();
            newUser.Register("123ABC");

            CountDownLatch cd = new CountDownLatch (1);
            Exception e = null;
            IdentityData.Builder builder = IdentityData.Builder.CreateWithPhone(this.existingUser.Phone);
            IdentityData identityData = builder.Build();
            newUser.Update(identityData, null, (KiiUser user, Exception e2) => {
                e = e2;
                cd.Signal();
            });

            if (!cd.Wait(new TimeSpan(0, 0, 0, 10)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNotNull(e);
            Assert.IsInstanceOfType(typeof(ConflictException), e);
        }
        #endregion
    }
}

