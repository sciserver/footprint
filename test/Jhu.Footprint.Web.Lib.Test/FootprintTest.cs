using System;
using System.IO;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Sdk;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Serialization;

namespace Jhu.Footprint.Web.Lib.Test
{
    [TestClass]
    public class FootprintTest
    {

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            using (var context = new Context())
            {
                string path = Path.GetDirectoryName((string)Environment.GetEnvironmentVariables()["SolutionPath"]);
                string script = File.ReadAllText(path + @"\footprint\sql\Jhu.Footprint.Tables.sql");
                script += File.ReadAllText(path + @"\footprint\sql\Jhu.Footprint.FootprintFolder.TestInit.sql");

                var server = new Server(new ServerConnection(context.Connection));
                server.ConnectionContext.ExecuteNonQuery(script);
            }
        }

        [TestMethod]
        public void FootprintCreateTest()
        {
            using (var context = new Context())
            {
                var footprint = new Footprint(context);

                footprint.Name = "Csik2";
                footprint.User = "webtestuser";
                footprint.Public = 1;
                footprint.FillFactor = 0.9;
                footprint.FolderId = 2;
                footprint.Type = FootprintType.None;
                footprint.Comment = "Create Test";

                footprint.Save();
                
            }
        }

        [TestMethod]      
        public void FootprintCreateTest2()
        {
            using (var context = new Context())
            {
                var footprint = new Footprint(context);

                footprint.Name = "Stripe5";
                footprint.User = "evelin";
                footprint.FolderId = 1;

                footprint.Comment = "duplicate name test.";

                try
                {
                    footprint.Save();
                }
                catch (FootprintException e)
                { 
                    System.Diagnostics.Debug.Write(e);
                }

            }
        }

        [TestMethod]
        public void FootprintModifyTest()
        {
            using (var context = new Context())
            {
                var footprint = new Footprint(context);

                footprint.Id = 5;
                footprint.User = "mike";

                footprint.Load();
                var s = @"REGION
	CONVEX
	-0.49240387650610379 0.85286853195244328 0.17364817766693033 0.64278760968653936";

                footprint.Region = Jhu.Spherical.Region.Parse(s);
                footprint.Region.Simplify();
                footprint.Save();
                
            }
        }

        [TestMethod]
        public void FootprintModifyTest2()
        {
            using (var context = new Context())
            {
                var footprint = new Footprint(context);

                footprint.Id = 5;
                footprint.User = "mike";

                footprint.Load();

                footprint.Name = "South";

                try
                {
                    footprint.Save();
                }
                catch (FootprintException e)
                {
                    System.Diagnostics.Debug.Write(e);
                }

            }
        }

        [TestMethod]
        public void FootprintDeleteTest()
        {
            using (var context = new Context())
            {
                var footprint = new Footprint(context);

                footprint.Id = 6;
                footprint.User = "bob";

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
                footprint.User = "evelin";

                footprint.Load();
            }
        }
    }
}
