using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.AccessControl;

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

        [TestMethod]
        public void FindRegionByOwnerTest()
        {
            using (var context = CreateContext())
            {
                context.Principal = CreateTestPrincipal();
                CreateTestFootprintAndRegion(context, "FindRegionByOwnerTest");
            }

            using (var context = CreateContext())
            {
                var search = new FootprintRegionSearch(context)
                {
                    Owner = context.Principal.Identity.Name
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
                context.Principal = CreateTestPrincipal();
                CreateTestFootprintAndRegion(context, "FindRegionByNameTest");
            }

            using (var context = CreateContext())
            {
                var search = new FootprintRegionSearch(context)
                {
                    Owner = context.Principal.Identity.Name,
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
                    Type = RegionType.Single,
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
                    Type = RegionType.Single,
                    Region = Spherical.Region.Parse("CIRCLE J2000 10 10 10")
                };

                region.Save();
            }

            using (var context = CreateContext())
            {
                context.Principal = CreateOtherPrincipal();

                var search = new FootprintRegionSearch(context)
                {
                    FootprintId = footprintid
                };

                Assert.AreEqual(0, search.Count());
                Assert.AreEqual(0, search.Find().Count());
            }
        }

        [TestMethod]
        public void RegionPointSearchTest()
        {
            var name = GetTestUniqueName();

            using (var context = CreateContext())
            {
                CreateTestFootprintAndRegion(context, name);
            }

            using (var context = CreateContext())
            {
                var search = new FootprintRegionSearch(context)
                {
                    SearchMethod = SearchMethod.Point,
                    Point = new Spherical.Cartesian(10,10)
                };

                Assert.IsTrue(1 <= search.Count());
                Assert.IsTrue(1 <= search.Find().Count());
            }
        }

        [TestMethod]
        public void RegionIntersectSearchTest()
        {
            var name = GetTestUniqueName();

            using (var context = CreateContext())
            {
                CreateTestFootprintAndRegion(context, name);
            }

            using (var context = CreateContext())
            {
                var search = new FootprintRegionSearch(context)
                {
                    SearchMethod = SearchMethod.Intersect,
                    Region = Spherical.Region.Parse("CIRCLE J2000 9 9 100")
                };

                Assert.IsTrue(1 <= search.Count());
                Assert.IsTrue(1 <= search.Find().Count());
            }
        }

        [TestMethod]
        public void RegionContainSearchTest()
        {
            var name = GetTestUniqueName();

            using (var context = CreateContext())
            {
                CreateTestFootprintAndRegion(context, name);
            }

            using (var context = CreateContext())
            {
                var search = new FootprintRegionSearch(context)
                {
                    SearchMethod = SearchMethod.Contain,
                    Region = Spherical.Region.Parse("CIRCLE J2000 10 10 9")
                };

                Assert.IsTrue(1 <= search.Count());
                Assert.IsTrue(1 <= search.Find().Count());
            }
        }

        [TestMethod]
        public void RegionCoverSearchTest()
        {
            Assert.Inconclusive();
        }
    }
}
