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
        public static void ClassInit(TestContext testcontext)
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
                    Name = "DuplicatefootprintNameModifyTest2",
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
    }
}
