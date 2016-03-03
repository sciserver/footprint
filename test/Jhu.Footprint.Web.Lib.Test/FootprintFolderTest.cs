﻿using System;
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

                folder.Save();
            }
        }

        [TestMethod]
        public void FolderCreateTest2()
        {
            using (var context = new Context())
            {

                var folder = new FootprintFolder(context);

                folder.Name = "SDSS.DR7";
                folder.User = "evelin";
                folder.Comment = "Duplicate name exception test";

                try
                {
                    folder.Save();
                }
                catch (FootprintFolderException e)
                {
                    System.Diagnostics.Debug.Write(e);
                }
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

                folder.Save();
            }
        }

        [TestMethod]
        public void FolderModifyTest2()
        {
            using (var context = new Context())
            {
                var folder = new FootprintFolder(context);

                folder.Id = 3;
                folder.User = "bob";
                folder.Load();

                folder.Name = "2SLAQ";
                folder.Comment = "duplicate name exception test";

                try
                {
                folder.Save();
                }
                catch (FootprintFolderException e)
                {
                    System.Diagnostics.Debug.Write(e);
                }
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
        public void GetFootprintsByFolderIdTest()
        {
            using (var context = new Context())
            {
                var search = new FootprintSearch(context);

                search.User = "evelin";
                search.FolderId = 1;


                var footprints = search.GetFootprintsByFolderId();
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
            // no dedicated footprints, num. of fps > 1, union
            using (var context = new Context())
            {
                var folder = new FootprintFolder(context);
                var footprint = new Footprint(context);
                footprint.Id = 19;
                footprint.User = "test";
                footprint.Load();

                folder.Id = 15;
                folder.User = "test";
                folder.Load();
                folder.UpdateFolderFootprint(footprint);
            }
        }

        [TestMethod]
        public void UpdateFolderFootprintTest2()
        {

            //no dedicated folder fp., num of fps >1, intersect
            using (var context = new Context())
            {
                var folder = new FootprintFolder(context);
                var footprint = new Footprint(context);

                footprint.Id = 22;
                footprint.User = "test";
                footprint.Load();

                folder.Id = 16;
                folder.User = "test";
                folder.Load();
                folder.UpdateFolderFootprint(footprint);
            }
        }
        [TestMethod]
        public void UpdateFolderFootprintTest3()
        {
            // no dedicated folder fp., num of fps =1
            using (var context = new Context())
            {
                var folder = new FootprintFolder(context);
                var footprint = new Footprint(context);

                footprint.Id = 23;
                footprint.User = "test";
                footprint.Load();

                folder.Id = 17;
                folder.User = "test";
                folder.Load();
                folder.UpdateFolderFootprint(footprint);
            }
        }
        [TestMethod]
        public void UpdateFolderFootprintTest4()
        {
            // dedicated folder fp., union
            using (var context = new Context())
            {
                var folder = new FootprintFolder(context);
                var footprint = new Footprint(context);

                footprint.Id = 25;
                footprint.User = "test";
                footprint.Load();

                folder.Id = 18;
                folder.User = "test";
                folder.Load();
                folder.UpdateFolderFootprint(footprint);
            }
        }
        [TestMethod]
        public void UpdateFolderFootprintTest5()
        {
            // dedicated folder fp. intersect
            using (var context = new Context())
            {
                var folder = new FootprintFolder(context);
                var footprint = new Footprint(context);

                footprint.Id = 27;
                footprint.User = "test";
                footprint.Load();

                folder.Id = 19;
                folder.User = "test";
                folder.Load();
                folder.UpdateFolderFootprint(footprint);
            }
        }
    }
}
