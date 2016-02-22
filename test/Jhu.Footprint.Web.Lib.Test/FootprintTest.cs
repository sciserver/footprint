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

                footprint.Create();
                
            }
        }

        [TestMethod]
        public void FootprintModifyTest()
        {
            using (var context = new Context())
            {
                var footprint = new Footprint(context);

                footprint.Id = 3;
                footprint.User = "evelin";

                footprint.Load();
                footprint.Comment = "Footprint.Modify test";
                var s = @"REGION
	CONVEX
	-1 0 0 0.9975640502598242
	CONVEX
	0.86602540378444 -0.49999999999999767 0 0.99795299276600746
	CONVEX
	-0.34059287762313883 0.091261586506912448 0.93577124049664562 0
	-0.27340720301988569 0.20979272558500117 0.93874145196026126 0
	0.3386387493172805 -0.14026876271228036 -0.93040231710158272 0";

                footprint.Region = Jhu.Spherical.Region.Parse(s);
                
                footprint.Modify();
                
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

                footprint.Id = 4;
                footprint.User = "webtestuser";

                footprint.Load();
            }

            // Test loading by name-user-folderName
            using (var context = new Context())
            {
                var footprint = new Footprint(context);

                footprint.Name = "Stripe2";
                footprint.User = "evelin";
                footprint.FolderName = "SDSS.DR7";

                footprint.Load();
            }
        }
    }
}
