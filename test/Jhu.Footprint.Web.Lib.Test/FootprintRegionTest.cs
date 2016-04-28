using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Serialization;
using Jhu.Graywulf.AccessControl;

namespace Jhu.Footprint.Web.Lib
{
    [TestClass]
    public class FootprintRegionTest : FootprintTestBase
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            InitializeDatabase();
        }

        [TestMethod]
        public void CreateRegionTest()
        {
            int footprintid;

            using (var context = CreateContext())
            {
                var footprint = new Footprint(context)
                {
                    Name = "CreateRegionTest",
                };

                footprintid = (int)footprint.Save();

                var region = new FootprintRegion(footprint)
                {
                    Name = "CreateRegionTest",
                    Type = RegionType.Single,
                    Region = Spherical.Region.Parse("CIRCLE J2000 10 10 10"),
                };

                footprint.Save();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateNameException))]
        public void DuplicateRegionNameTest()
        {
            int footprintid;

            using (var context = CreateContext())
            {
                var footprint = new Footprint(context)
                {
                    Name = "DuplicateRegionNameTest",
                };

                footprintid = (int)footprint.Save();

                var region = new FootprintRegion(footprint)
                {
                    Name = "DuplicateRegionNameTest",
                    Type = RegionType.Single,
                    Region = Spherical.Region.Parse("CIRCLE J2000 10 10 10"),
                };

                region.Save();

                region = new FootprintRegion(footprint)
                {
                    Name = "DuplicateRegionNameTest",
                    Type = RegionType.Single,
                    Region = Spherical.Region.Parse("CIRCLE J2000 10 10 10"),
                };

                region.Save();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(AccessDeniedException))]
        public void AccessDeniedCreateRegionTest()
        {
            int regionId;
            int footprintid;

            using (var context = CreateContext())
            {
                var region = CreateRegion(context, "AccessDeniedCreateRegionTest");
                regionId = region.Id;
                footprintid = region.FootprintId;

                // grant access to other identity

                var footprint = new Footprint(context);
                footprint.Load(footprintid);
                footprint.Permissions.Grant(CreateOtherPrincipal().Identity.Name, DefaultAccess.Read);
                footprint.Save();
            }

            using (var context = CreateContext())
            {
                context.Principal = CreateOtherPrincipal();

                var footprint = new Footprint(context);
                footprint.Load(footprintid);

                var region = new FootprintRegion(footprint)
                {
                    FootprintId = footprintid,
                    Id = regionId
                };

                region.Load();

                // Now try to modify, this should fail

                region.Save();
            }
        }

        [TestMethod]
        public void UpdateRegionCacheTest()
        {
        }
    }
}
