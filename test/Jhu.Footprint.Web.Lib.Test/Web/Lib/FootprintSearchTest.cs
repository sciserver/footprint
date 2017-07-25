using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Test;

namespace Jhu.Footprint.Web.Lib.Test
{
    [TestClass]
    public class FootprintSearchTest : FootprintTestBase
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            InitializeDatabase();

            using (var context = CreateContext())
            {
                context.Principal = CreateTestPrincipal();

                var footprint = new Footprint(context)
                {
                    Name = "FootprintSearchTest",
                };

                footprint.Save();
            }

            using (var context = CreateContext())
            {
                context.Principal = CreateOtherPrincipal();

                var footprint = new Footprint(context)
                {
                    Name = "OtherFootprint",
                };

                footprint.Save();
            }
        }

        [TestMethod]
        public void FindFootprintByOwnerTest()
        {
            using (var context = CreateContext())
            {
                var search = new FootprintSearch(context)
                {
                    Owner = context.Principal.Identity.Name
                };

                Assert.AreEqual(1, search.Count());
                Assert.AreEqual(1, search.Find().Count());
            }
        }

        [TestMethod]
        public void FindFootprintByNameTest()
        {
            var name = GetTestUniqueName();

            using (var context = CreateContext())
            {
                CreateTestFootprint(context, name);

                var search = new FootprintSearch(context)
                {
                    Owner = context.Principal.Identity.Name,
                    Name = name + "%"
                };

                Assert.AreEqual(1, search.Count());
                Assert.AreEqual(1, search.Find().Count());
            }
        }
    }
}
