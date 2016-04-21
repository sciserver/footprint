using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jhu.Footprint.Web.Lib
{
    [TestClass]
    public class FootprintTest : FootprintTestBase
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            InitializeDatabase();
        }

        [TestMethod]
        public void CreateFootprintTest()
        {
            int id;

            using (var context = CreateContext())
            {
                var footprint = new Footprint(context)
                {
                    Name = "CreateFootprintTest",
                };

                id = (int)footprint.Save();
            }

            using (var context = CreateContext())
            {
                var footprint = new Footprint(context);
                footprint.Load(id);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateNameException))]
        public void DuplicateFootprintNameCreateTest()
        {
            using (var context = CreateContext())
            {
                var footprint = new Footprint(context)
                {
                    Name = "DuplicateFootprintNameCreateTest",
                };

                footprint.Save();
            }

            using (var context = CreateContext())
            {
                var footprint = new Footprint(context)
                {
                    Name = "DuplicateFootprintNameCreateTest",
                };

                footprint.Save();
            }
        }

        [TestMethod]
        public void ModifyFootprintTest()
        {
            int id;

            using (var context = CreateContext())
            {
                var footprint = new Footprint(context)
                {
                    Name = "ModifyFootprintTest",
                };

                id = (int)footprint.Save();
            }

            using (var context = CreateContext())
            {
                var footprint = new Footprint(context);
                footprint.Load(id);

                footprint.Name = "Rename";

                footprint.Save();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateNameException))]
        public void DuplicateFootprintNameModifyTest()
        {
            int id;

            using (var context = CreateContext())
            {
                var footprint = new Footprint(context)
                {
                    Name = "DuplicateFootprintNameModifyTest",
                };

                id = (int)footprint.Save();
            }

            using (var context = CreateContext())
            {
                var footprint = new Footprint(context)
                {
                    Name = "DuplicateFolderNameModifyTest2",
                };

                id = (int)footprint.Save();
            }

            using (var context = CreateContext())
            {
                var footprint = new Footprint(context);
                footprint.Load(id);

                footprint.Name = "DuplicateFootprintNameModifyTest";

                footprint.Save();
            }
        }

        [TestMethod]
        public void DeleteFootprintTest()
        {
            int id;

            using (var context = CreateContext())
            {
                var footprint = new Footprint(context)
                {
                    Name = "DeleteFootprintTest",
                };

                id = (int)footprint.Save();
            }

            using (var context = CreateContext())
            {
                var footprint = new Footprint(context);
                footprint.Load(id);
                footprint.Delete();
            }
        }

        /*

        

        [TestMethod]
        public void FolderLoadTest()
        {
            using (var context = new Context())
            {
                var folder = new FootprintFolder(context);

                folder.Id = 6;
                context.User = "kate";

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
                context.User = "test";
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
                context.User = "test";
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
                context.User = "test";
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
                context.User = "test";
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
                context.User = "test";
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
                context.User = "test";
                footprint.Load();

                folder.Id = 15;
                context.User = "test";
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
                context.User = "test";
                footprint.Load();

                folder.Id = 16;
                context.User = "test";
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
                context.User = "test";
                footprint.Load();

                folder.Id = 17;
                context.User = "test";
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
                context.User = "test";
                footprint.Load();

                folder.Id = 18;
                context.User = "test";
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
                context.User = "test";
                footprint.Load();

                folder.Id = 19;
                context.User = "test";
                folder.Load();
                folder.UpdateFolderFootprint(footprint);
            }
        }
         * */
    }
}
