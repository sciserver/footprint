using System;
using System.IO;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Sdk;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jhu.Footprint.Web.Lib.Test
{

    [TestClass]
    public class FootprintFolderTest
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
        public void CreateTest()
        {
            using (var context = new Context())
            {

                var folder = new FootprintFolder(context);

                folder.Name = "CreateTest";
                folder.User = "webtestuser";
                folder.Type = FolderType.None;
                folder.Public = 1;
                folder.Comment = "FootprintFolder.Create Unit Test";

                folder.Create();
            }
        }

        [TestMethod]

        public void ModifyTest()
        {
            using (var context = new Context())
            {
                var folder = new FootprintFolder(context);

                folder.Id = 8;
                folder.Name = "ModifyTest";
                folder.User = "evelin";
                //folder.Type = 0;
                //folder.Public = 0;
                folder.Comment = "FootprintFolder.Modify Unit Test";

                folder.Modify();
            }
        }

        [TestMethod]
        public void DeleteTest()
        {
            using (var context = new Context())
            {
                var folder = new FootprintFolder(context);

                folder.Id = 5;
                folder.User = "bob";

                folder.Delete();
            }
        }

        [TestMethod]
        public void LoadTest()
        { 
            using (var context = new Context())
            {
                var folder = new FootprintFolder(context);

                folder.Id = 6;
                folder.User = "kate";

                folder.Load();
            }

        }
    }
}
