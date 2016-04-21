using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Test;

namespace Jhu.Footprint.Web.Lib.Test
{
    [TestClass]
    public class FootprintRegionSearchTest : FootprintTestBase
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            InitializeDatabase();

            using (var context = CreateContext())
            {
                context.Identity = CreateTestIdentity();

                var footprint = new Footprint(context)
                {
                    Name = "FootprintRegionSearchTest",
                };

                footprint.Save();

                var region = new FootprintRegion(context)
                {
                    FootprintId = footprint.Id,
                    Name = "FootprintRegionSearchTest",
                    Type = FootprintType.Region,
                    Region = Spherical.Region.Parse("CIRCLE J2000 10 10 10")
                };

                region.Save();
            }

            using (var context = CreateContext())
            {
                context.Identity = CreateOtherIdentity();

                var footprint = new Footprint(context)
                {
                    Name = "OtherFootprint",
                };

                footprint.Save();

                var region = new FootprintRegion(context)
                {
                    FootprintId = footprint.Id,
                    Name = "OtherFootprint",
                    Type = FootprintType.Region,
                    Region = Spherical.Region.Parse("CIRCLE J2000 10 10 10")
                };

                region.Save();
            }
        }

        [TestMethod]
        public void FindByOwnerTest()
        {
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
        public void FindByNameTest()
        {
            using (var context = CreateContext())
            {
                var search = new FootprintRegionSearch(context)
                {
                    Owner = context.Identity.Name,
                    FootprintName = "FootprintRegionSearch%",
                    Name = "FootprintRegionSearch%"
                };

                Assert.AreEqual(1, search.Count());
                Assert.AreEqual(1, search.Find().Count());
            }
        }

        [TestMethod]
        public void FindByFootprintIdTest()
        {
            int footprintid;

            using (var context = CreateContext())
            {
                var footprint = new Footprint(context)
                {
                    Name = "FindByFootprintIdTest",
                };

                footprintid = (int)footprint.Save();

                var region = new FootprintRegion(context)
                {
                    FootprintId = footprint.Id,
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
        public void AccessDeniedTest()
        {
            int footprintid;

            using (var context = CreateContext())
            {
                var footprint = new Footprint(context)
                {
                    Name = "AccessDeniedTest",
                };

                footprintid = (int)footprint.Save();

                var region = new FootprintRegion(context)
                {
                    FootprintId = footprint.Id,
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
