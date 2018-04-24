using System;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture ()]
    public class ServerCodeTest : LargeTestBase
    {
        [Test()]
        public void DeployWithEnvironmentVersion0Test()
        {
            string code = @"function test() {
                return 'OK';
            }
            ";
            string versionId = AppUtil.DeployServerCode(this.app, code);
            KiiServerCodeExecResult result = Kii.ServerCodeEntry("test", versionId, KiiServerCodeEnvironmentVersion.V0).Execute(null);
            Assert.AreEqual(KiiServerCodeEnvironmentVersion.V0, result.EnvironmentVersion);
        }
        [Test()]
        public void DeployWithEnvironmentVersion6Test()
        {
            string code = @"function test() {
                return 'OK';
            }
            ";
            string versionId = AppUtil.DeployServerCode(this.app, code);
            KiiServerCodeExecResult result = Kii.ServerCodeEntry("test", versionId, KiiServerCodeEnvironmentVersion.V6).Execute(null);
            Assert.AreEqual(KiiServerCodeEnvironmentVersion.V6, result.EnvironmentVersion);
        }
    }
}

