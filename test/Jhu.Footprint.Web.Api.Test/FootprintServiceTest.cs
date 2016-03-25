using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Footprint.Web.Lib;
using Jhu.Footprint.Web.Api.Test;
using Jhu.Graywulf.Web.Api.V1;
using Jhu.Graywulf.Web.Services;


namespace Jhu.Footprint.Web.Api.V1
{
    [TestClass]
    public class FootprintServiceTest : FootprintApiTestBase
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            InitDatabase();
        }

        [TestMethod]
        public void GetUserFootprintFolderListTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var folders = client.GetUserFootprintFolderList("evelin");
            }
        }

        [TestMethod]
        public void GetUserFootprintFolderTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var folder = client.GetUserFootprintFolder("evelin","SDSS.DR7");
            }
        }

        [TestMethod]
        public void GetUserFootprintFolderRegionTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var folder = client.GetUserFootprintFolderRegion("evelin","SDSS.DR7");
            }
        }

        [TestMethod]
        public void GetUserFootprintFolderRegionOutlineTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var folder = client.GetUserFootprintFolderRegionOutline("evelin", "SDSS.DR7");
            }
        }

        [TestMethod]
        public void GetUserFootprintFolderRegionOutlinePointsTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var folder = client.GetUserFootprintFolderRegionOutlinePoints("evelin", "SDSS.DR7", 0.3);
            }
        
        }

        [TestMethod]
        public void GetUserFootprintFolderRegionConvexHullTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var folder = client.GetUserFootprintFolderRegionConvexHull("evelin", "SDSS.DR7");
            }        
        }

        [TestMethod]
        public void GetUserFootprintFolderRegionConvexHullOutlineTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var folder = client.GetUserFootprintFolderRegionConvexHullOutline("evelin", "SDSS.DR7");
            }
        }

        [TestMethod]
        public void GetUserFootprintFolderRegionConvexHullOutlinePointsTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var folder = client.GetUserFootprintFolderRegionConvexHullOutlinePoints("evelin", "SDSS.DR7", 0.3);
            }
        }

        [TestMethod]
        public void CreateUserFootprintFolderTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);

                var request = new FootprintFolderRequest();
                var folder = new Lib.FootprintFolder();

                folder.Comment = "Test Api Create Folder";
                folder.User = "Evelin";
                folder.Name = "Test Api";
                folder.Type = FolderType.Intersection;

                request.FootprintFolder = new V1.FootprintFolder(folder);
                client.CreateUserFootprintFolder(request.FootprintFolder.User, request.FootprintFolder.Name, request);
            }
        }

        [TestMethod]
        public void ModifyUserFootprintFolderTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);

                var folder = new Jhu.Footprint.Web.Lib.FootprintFolder();
                var request = new FootprintFolderRequest();

                using (var context = new Context())
                {
                    folder.Context = context;
                    folder.Id = 8;
                    folder.Load();
                }

                folder.Comment = "Api modification test.";

                request.FootprintFolder = new V1.FootprintFolder(folder);

                client.ModifyUserFootprintFolder(request.FootprintFolder.User, request.FootprintFolder.Name, request);
            }
        }

        [TestMethod]
        public void DeleteUserFootprintFolderTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);

                client.DeleteUserFootprintFolder("kate", "COSMOS");
            }
        }

        [TestMethod]
        public void GetUserFootprintTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var footprint = client.GetUserFootprint("evelin", "SDSS.DR7", "Stripe2");
            }
        }

        [TestMethod]
        public void GetUserFootprintRegionTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var footprint = client.GetUserFootprintRegion("evelin","SDSS.DR7","Stripe2");
            }
        }



        [TestMethod]
        public void GetUserFootprintRegionOutlineTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var footprint = client.GetUserFootprintRegionOutline("evelin", "SDSS.DR7", "Stripe2");
            }
        }

        [TestMethod]
        public void GetUserFootprintRegionOutlinePointsTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var footprint = client.GetUserFootprintRegionOutlinePoints("evelin", "SDSS.DR7", "Stripe2",0.9);
            }            
        }

        [TestMethod]
        public void GetUserFootprintRegionConvexHullTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var footprint = client.GetUserFootprintRegionConvexHullOutline("evelin", "SDSS.DR7", "Stripe5");
            }

        }

        [TestMethod]
        public void GetUserFootprintRegionConvexHullOutlineTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var footprint = client.GetUserFootprintRegionConvexHullOutline("evelin", "SDSS.DR7", "Stripe5");
            }

        }

        [TestMethod]
        public void GetUserFootprintRegionConvexHullOutlinePointsTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var footprint = client.GetUserFootprintRegionConvexHullOutlinePoints("evelin", "SDSS.DR7", "Stripe5", 0.1);
            }

        }

        [TestMethod]
        public void CreatUserFootprintTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var request = new FootprintRequest();


                var footprint = new Lib.Footprint();
                footprint.Comment = "Test Api Create Footprint";
                footprint.User = "Evelin";
                footprint.Name = "Test api";
                footprint.FolderId = 1;

                request.Footprint = new V1.Footprint(footprint,"SDSS.DR7");

                client.CreateUserFootprint(request.Footprint.User, "SDSS.DR7", request.Footprint.Name, request);

            }
        }

        [TestMethod]
        public void ModifyUserFootprintTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);

                var footprint = new Jhu.Footprint.Web.Lib.Footprint();
                var request = new FootprintRequest();

                using (var context = new Context())
                {
                    footprint.Context = context;
                    footprint.Id = 4;
                    footprint.Load();
                }

                footprint.Comment = "Api modification test.";

                request.Footprint = new V1.Footprint(footprint,"2MASS");

                client.ModifyUserFootprint("mike", "2MASS", "South", request);
            }

        }

        [TestMethod]
        public void DeleteUserFootprintTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);

                client.DeleteUserFootprint("evelin", "SDSS.DR7", "ApiDelete");
            }
        }


    }
}
