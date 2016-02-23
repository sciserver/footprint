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
        public void FolderCreateTest()
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

        public void FolderModifyTest()
        {
            using (var context = new Context())
            {
                var folder = new FootprintFolder(context);

                folder.Id = 4;
                folder.User = "mike";
                folder.Load();

                folder.Name = "ModifyTest";
                folder.Comment = "FootprintFolder.Modify Unit Test";

                folder.Modify(false);

                folder.Load();
            }
        }

        [TestMethod]
        public void FolderDeleteTest()
        {
            using (var context = new Context())
            {
                var folder = new FootprintFolder(context);

                folder.Id = 9;
                folder.User = "lilly";

                folder.Delete();
            }
        }

        [TestMethod]
        public void FolderLoadTest()
        { 
            using (var context = new Context())
            {
                var folder = new FootprintFolder(context);

                folder.Id = 6;
                folder.User = "kate";

                folder.Load();
            }

        }

        [TestMethod]
        public void RefreshFolderFootprintTest()
        {
            // Num. of footprints < 2 , dedicated folder footprint
            using (var context = new Context())
            {
                var folder = new FootprintFolder(context);

                folder.Id = 10;
                folder.User = "test";
                folder.Load();
                folder.RefreshFolderFootprint();
            }
        }

        [TestMethod]
        public void RefreshFolderFootprintTest2()
        {
            // Num. of footprints < 2 , no dedicated folder footprint
            using (var context = new Context())
            {
                var folder = new FootprintFolder(context);

                folder.Id = 11;
                folder.User = "test";
                folder.Load();
                folder.RefreshFolderFootprint();
            }
        }

        [TestMethod]
        public void RefreshFolderFootprintTest3()
        {
            // Num. of footprints >= 2, no dedicated folder fp.
            using (var context = new Context())
            {
                var folder = new FootprintFolder(context);

                folder.Id = 12;
                folder.User = "test";
                folder.Load();
                folder.RefreshFolderFootprint();
            }
        }

        [TestMethod]
        public void RefreshFolderFootprintTest4()
        {
            // Num. of footprints >=2, dedicated folder fp., union
            using (var context = new Context())
            {
                var folder = new FootprintFolder(context);

                folder.Id = 13;
                folder.User = "test";
                folder.Load();
                folder.RefreshFolderFootprint();
            }
        }


        [TestMethod]
        public void RefreshFolderFootprintTest5()
        {
            // Num. of footprints >= 2, dedicated folder fp., intersect.
            using (var context = new Context())
            {
                var folder = new FootprintFolder(context);

                folder.Id = 14;
                folder.User = "test";
                folder.Load();
                folder.RefreshFolderFootprint();
            }
        }

        [TestMethod]
        public void UpdateFolderFootprintTest()
        {
            using (var context = new Context())
            {
                var folder = new FootprintFolder(context);

                folder.Id = 8;
                folder.User = "james";
                folder.Load();
                folder.UpdateFolderFootprint(7);
            }
        }
    }
}
