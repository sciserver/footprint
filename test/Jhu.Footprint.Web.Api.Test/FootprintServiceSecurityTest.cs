using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.ServiceModel;
using System.ServiceModel.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lib = Jhu.Footprint.Web.Lib;
using Jhu.Spherical;
using Jhu.Graywulf.Web.Api.V1;
using Jhu.Graywulf.Web.Services;


namespace Jhu.Footprint.Web.Api.V1
{
    [TestClass]
    public class FootprintServiceSecurityTest : FootprintApiTestBase
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            InitializeDatabase();
        }

        #region Create footprint tests

        [TestMethod]
        public void CreateUserFootprintTest()
        {
            var name = GetTestUniqueName();

            var footprint = CreateTestFootprint(TestUser, TestUser, name, false);

            Assert.AreEqual(footprint.Owner, TestUser);
            Assert.AreEqual(footprint.Name, name);
            Assert.AreEqual(footprint.Type, Lib.FootprintType.None);
            Assert.AreEqual(footprint.Public, false);
        }

        [TestMethod]
        public void GrantCreateGroupFootprintByAdminTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(GroupAdminUser, TestGroup, name, true);
        }

        [TestMethod]
        public void GrantCreateGroupFootprintByWriterTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(GroupWriterUser, TestGroup, name, true);
        }

        [TestMethod]
        [ExpectedException(typeof(MessageSecurityException))]
        public void DenyCreateGroupFootprintByReaderTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(GroupReaderUser, TestGroup, name, true);
        }

        [TestMethod]
        [ExpectedException(typeof(MessageSecurityException))]
        public void DenyCreateGroupFootprintByOtherTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(OtherUser, TestGroup, name, true);
        }

        #endregion
        #region Get footprint tests

        [TestMethod]
        public void GrantGetUserFootprintTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(TestUser, TestUser, name, true);
            var f2 = GetTestFootprint(TestUser, TestUser, name);

            Assert.AreEqual(TestUser, f2.Owner);
            Assert.AreEqual(name, f2.Name);
        }

        [TestMethod]
        public void GrantGetPublicUserFootprintTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(TestUser, TestUser, name, true);
            var f2 = GetTestFootprint(OtherUser, TestUser, name);

            Assert.AreEqual(f1.Name, f2.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(MessageSecurityException))]
        public void DenyGetPrivateUserFootprintTest()
        {
            var name = GetTestUniqueName();

            CreateTestFootprint(TestUser, TestUser, name, false);
            GetTestFootprint(OtherUser, TestUser, name);
        }

        [TestMethod]
        public void GrantGetPrivateGroupFootprintTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(GroupWriterUser, TestGroup, name, false);
            var f2 = GetTestFootprint(GroupReaderUser, TestGroup, name);
        }

        [TestMethod]
        public void GrantGetPublicGroupFootprintTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(GroupWriterUser, TestGroup, name, true);
            var f2 = GetTestFootprint(OtherUser, TestGroup, name);
        }

        [TestMethod]
        [ExpectedException(typeof(MessageSecurityException))]
        public void DenyGetPrivateGroupFootprintTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(GroupWriterUser, TestGroup, name, false);
            var f2 = GetTestFootprint(OtherUser, TestGroup, name);
        }

        #endregion
        #region Modify footprint tests

        [TestMethod]
        public void GrantModifyUserFootprintTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(TestUser, TestUser, name, true);
            var f2 = ModifyTestFootprint(TestUser, TestUser, name);

            Assert.AreEqual(TestUser, f2.Owner);
            Assert.AreEqual(name, f2.Name);
            Assert.AreEqual("modified", f2.Comments);
        }

        [TestMethod]
        [ExpectedException(typeof(MessageSecurityException))]
        public void DenyModifyUserFootprintTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(TestUser, TestUser, name, true);
            var f2 = ModifyTestFootprint(OtherUser, TestUser, name);
        }

        [TestMethod]
        public void GrantModifyGroupFootprintByAdminTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(GroupAdminUser, TestGroup, name, true);
            var f2 = ModifyTestFootprint(GroupAdminUser, TestGroup, name);

            Assert.AreEqual(TestGroup, f2.Owner);
            Assert.AreEqual(name, f2.Name);
            Assert.AreEqual("modified", f2.Comments);
        }

        [TestMethod]
        public void GrantModifyGroupFootprintByWriterTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(GroupAdminUser, TestGroup, name, true);
            var f2 = ModifyTestFootprint(GroupWriterUser, TestGroup, name);

            Assert.AreEqual(TestGroup, f2.Owner);
            Assert.AreEqual(name, f2.Name);
            Assert.AreEqual("modified", f2.Comments);
        }

        [TestMethod]
        [ExpectedException(typeof(MessageSecurityException))]
        public void DenyModifyGroupFootprintByReaderTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(GroupAdminUser, TestGroup, name, true);
            var f2 = ModifyTestFootprint(GroupReaderUser, TestGroup, name);
        }

        // TODO: additional tests:
        // - try to modify group's entity while being the admin of another group
        // - try to modify group's entity while being the writer of another group

        #endregion
        #region

        [TestMethod]
        public void GrantDeleteUserFootprintTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(TestUser, TestUser, name, true);
            DeleteTestFootprint(TestUser, TestUser, name);
        }

        [TestMethod]
        [ExpectedException(typeof(MessageSecurityException))]
        public void DenyDeleteUserFootprintTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(TestUser, TestUser, name, true);
            DeleteTestFootprint(OtherUser, TestUser, name);
        }

        [TestMethod]
        public void GrantDeleteGroupFootprintByAdminTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(GroupAdminUser, TestGroup, name, true);
            DeleteTestFootprint(GroupAdminUser, TestGroup, name);
        }

        [TestMethod]
        public void GrantDeleteGroupFootprintByWriterTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(GroupAdminUser, TestGroup, name, true);
            DeleteTestFootprint(GroupWriterUser, TestGroup, name);
        }

        [TestMethod]
        [ExpectedException(typeof(MessageSecurityException))]
        public void DenyDeleteGroupFootprintByReaderTest()
        {
            var name = GetTestUniqueName();

            var f1 = CreateTestFootprint(GroupAdminUser, TestGroup, name, true);
            DeleteTestFootprint(GroupReaderUser, TestGroup, name);
        }

        #endregion
    }
}
