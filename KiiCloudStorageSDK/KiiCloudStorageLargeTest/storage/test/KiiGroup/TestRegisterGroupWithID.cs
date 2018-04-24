using System;
using System.Collections.Generic;
using NUnit.Framework;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestRegisterGroupWithID : LargeTestBase
    {
        private KiiUser member1 = null;
        private KiiUser member2 = null;
        [SetUp()]
        public override void SetUp ()
        {
            base.SetUp ();
            member1 = AppUtil.CreateNewUser ("kiigrouptest-" + CurrentTimeMillis (), "123456");
            member2 = AppUtil.CreateNewUser ("kiigrouptest-" + CurrentTimeMillis (), "123456");
            // owner
            AppUtil.CreateNewUser ("kiigrouptest-" + CurrentTimeMillis (), "123456");
        }
        [TearDown()]
        public override void TearDown ()
        {
            AppUtil.DeleteUser (KiiUser.CurrentUser);
            base.TearDown ();
        }
        private long CurrentTimeMillis ()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }
        private string GetGroupID()
        {
            return "my-group" + CurrentTimeMillis();
        }
        [Test()]
        public void Test ()
        {
            string groupID = GetGroupID();
            string groupName = "group-" + DateTime.Now.Ticks.ToString(); 
            IList<KiiUser> members = new List<KiiUser>();
            members.Add(this.member1);
            members.Add(this.member2);

            CountDownLatch cd = new CountDownLatch(1);
            KiiGroup group = null;
            Exception exception = null;

            KiiGroup.RegisterGroupWithID(groupID, groupName, members, (KiiGroup result, Exception e)=>{
                group = result;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(exception);

            Assert.AreEqual(groupID, group.ID);
            Assert.AreEqual(groupName, group.Name);
            Assert.AreEqual(KiiUser.CurrentUser.ID, group.Owner.ID);

            members = group.ListMembers();
            Assert.AreEqual(3, members.Count); // 2 members and owner
        }
    }
}

