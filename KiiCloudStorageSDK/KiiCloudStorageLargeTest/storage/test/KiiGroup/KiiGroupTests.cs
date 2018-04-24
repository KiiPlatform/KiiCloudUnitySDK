using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class KiiGroupTests : LargeTestBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test()]
        public void Test_0101_RemoveMyself_FromGroupMember()
        {
            // Create KiiUser for group member
            long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            string memberName = "kiiGroupMember-" + milliseconds;
            KiiUser member = KiiUser.BuilderWithName(memberName).Build();
            member.Register("pass1234");
            string memberToken = KiiUser.AccessToken;

            // Create KiiUser for group member
            string ownerName = "kiiGroupOwner-" + milliseconds;
            KiiUser owner = KiiUser.BuilderWithName(ownerName).Build();
            owner.Register("pass1234");

            // Create group
            KiiGroup group = Kii.Group("testGroup");

            // Add member and save
            group.AddUser(member);
            group.Save();

            // Auth as member
            KiiUser.LogOut();
            KiiUser.LoginWithToken(memberToken);

            // Try to remove myself from the group
            group.RemoveUser(KiiUser.CurrentUser);
            group.Save();

            // Check value
            HashSet<KiiUser> addUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "addUsers");
            HashSet<KiiUser> removeUsers = (HashSet<KiiUser>)SDKTestHack.GetField(group, "removeUsers");
            Assert.AreEqual(0, addUsers.Count);
            Assert.AreEqual(0, removeUsers.Count);
        }
    }
}

