using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Entities.AccessControl;

namespace Jhu.Footprint.Web.Lib.Test
{
    [TestClass]
    public class FootprintRegionSearchTest : FootprintTestBase
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            InitializeDatabase();

        }

        protected void CreateRegion(Context context, string name)
        {
            var footprint = new Footprint(context)
            {
                Name = name,
            };

            footprint.Save();

            var region = new FootprintRegion(footprint)
            {
                Name = name,
                Type = FootprintType.Region,
                Region = Spherical.Region.Parse("CIRCLE J2000 10 10 10")
            };

            region.Save();
        }

        [TestMethod]
        public void FindRegionByOwnerTest()
        {
            using (var context = CreateContext())
            {
                context.Identity = CreateTestIdentity();
                CreateRegion(context, "FindRegionByOwnerTest");
            }

            using (var context = CreateContext())
            {
                var search = new FootprintRegionSearch(context)
                {
                    Owner = context.Identity.Name
                };

                Assert.IsTrue(1 <= search.Count());
                Assert.IsTrue(1 <= search.Find().Count());
            }
        }

        [TestMethod]
        public void FindRegionByNameTest()
        {
            using (var context = CreateContext())
            {
                context.Identity = CreateTestIdentity();
                CreateRegion(context, "FindRegionByNameTest");
            }

            using (var context = CreateContext())
            {
                var search = new FootprintRegionSearch(context)
                {
                    Owner = context.Identity.Name,
                    FootprintName = "FindRegionByName%",
                    Name = "FindRegionByName%"
                };

                Assert.AreEqual(1, search.Count());
                Assert.AreEqual(1, search.Find().Count());
            }
        }

        [TestMethod]
        public void FindRegionByFootprintIdTest()
        {
            int footprintid;

            using (var context = CreateContext())
            {
                var footprint = new Footprint(context)
                {
                    Name = "FindByFootprintIdTest",
                };

                footprintid = (int)footprint.Save();

                var region = new FootprintRegion(footprint)
                {
                    Name = "FindByFootprintIdTest",
                    Type = FootprintType.Region,
                    Region = Spherical.Region.Parse("CIRCLE J2000 10 10 10")
                };

                region.Save();
            }

            using (var context = CreateContext())
            {
                var search = new FootprintRegionSearch(context)
                {
                    FootprintId = footprintid
                };

                Assert.AreEqual(1, search.Count());
                Assert.AreEqual(1, search.Find().Count());
            }
        }

        [TestMethod]
        public void RegionAccessDeniedTest()
        {
            int footprintid;

            using (var context = CreateContext())
            {
                var footprint = new Footprint(context)
                {
                    Name = "AccessDeniedTest",
                };

                footprintid = (int)footprint.Save();

                var region = new FootprintRegion(footprint)
                {
                    Name = "AccessDeniedTest",
                    Type = FootprintType.Region,
                    Region = Spherical.Region.Parse("CIRCLE J2000 10 10 10")
                };

                region.Save();
            }

            using (var context = CreateContext())
            {
                context.Identity = CreateOtherIdentity();

                var search = new FootprintRegionSearch(context)
                {
                    FootprintId = footprintid
                };

                Assert.AreEqual(0, search.Count());
                Assert.AreEqual(0, search.Find().Count());
            }
        }
    }
}
