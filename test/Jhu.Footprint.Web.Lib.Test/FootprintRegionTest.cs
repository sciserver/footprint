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
                    Type = RegionType.Region,
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
                    Type = RegionType.Region,
                    Region = Spherical.Region.Parse("CIRCLE J2000 10 10 10"),
                };

                region.Save();

                region = new FootprintRegion(footprint)
                {
                    Name = "DuplicateRegionNameTest",
                    Type = RegionType.Region,
                    Region = Spherical.Region.Parse("CIRCLE J2000 10 10 10"),
                };

                region.Save();
            }
        }

        [TestMethod]
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

                try
                {
                    region.Save();
                    Assert.Fail();
                }
                catch (System.Security.SecurityException)
                {
                }
            }
        }

        /*
        [TestMethod]      
        [ExpectedException(typeof(FootprintException))]
        public void FootprintCreateTest2()
        {
            using (var context = new Context())
            {
                var footprint = new Footprint(context);

                footprint.Name = "Stripe5";
                footprint.FolderId = 1;
                footprint.User = "evelin";

                context.User = "evelin";
                footprint.Comment = "duplicate name test.";
                footprint.Save();
            }
        }


        [TestMethod]
        [ExpectedException(typeof(FootprintException))]
        public void FootprintCreateTest3()
        {
            using (var context = new Context())
            {
                var footprint = new Footprint(context);

                footprint.Name = "footprint";
                footprint.FolderId = 1;

                context.User = "evelin";
                footprint.Save();
            }
        }

        [TestMethod]
        public void FootprintModifyTest()
        {
            using (var context = new Context())
            {
                var footprint = new Footprint(context);

                footprint.Id = 2;
                context.User = "evelin";

                footprint.Load();
                var s = @"REGION
	CONVEX
	-0.70710678118654746 0.70710678118654757 0 0
	0 0 1 0
	0.14356697510519473 0.53579924538156265 -0.83205029433784372 0
	0.96592582628906831 0.25881904510252085 5.5511151231257839E-17 0
	CONVEX
	0 0 1 0
	0 1 0 0
	0.33081724288831171 0.25384499855699239 -0.9089129048018717 0
	0.70710678118654746 -0.70710678118654757 0 0";

                footprint.Region = Jhu.Spherical.Region.Parse(s);
                footprint.Region.Simplify();
                footprint.Save();
                
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FootprintException))]
        public void FootprintModifyTest2()
        {
            using (var context = new Context())
            {
                var footprint = new Footprint(context);

                footprint.Id = 5;
                context.User = "mike";

                footprint.Load();

                footprint.Name = "South";

                footprint.Save();

            }
        }

        [TestMethod]
        public void FootprintDeleteTest()
        {
            using (var context = new Context())
            {
                var footprint = new Footprint(context);

                footprint.Id = 6;
                context.User = "bob";

                footprint.Delete();
            }
        }

        [TestMethod]
        public void FootprintLoadTest()
        {
            // Test loading by ID
            using (var context = new Context())
            {
                var footprint = new Footprint(context);

                footprint.Id = 1;
                context.User = "evelin";

                footprint.Load();
            }
        }
         * */
    }
}
