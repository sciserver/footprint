using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Serialization;

namespace Jhu.Footprint.Web.Lib
{
    [TestClass]
    public class FootprintTest : FootprintTestBase
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            InitDatabase();
        }

        [TestMethod]
        public void FootprintCreateTest()
        {
            using (var context = new Context())
            {
                var footprint = new Footprint(context);

                footprint.Name = "Csik2";
                context.User = "webtestuser";
                footprint.Public = 1;
                footprint.FillFactor = 0.9;
                footprint.FolderId = 2;
                footprint.Type = FootprintType.None;
                footprint.Comment = "Create Test";

                footprint.Save();
                
            }
        }

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
    }
}
