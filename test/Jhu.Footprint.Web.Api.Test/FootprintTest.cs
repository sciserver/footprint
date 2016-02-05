using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mime;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Sdk;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Footprint.Web.Lib;
using Jhu.Graywulf.Web.Api.V1;
using Jhu.Graywulf.Web.Services;

namespace Jhu.Footprint.Web.Api.V1
{
    [TestClass]
    public class FootprintTest : ApiTestBase
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
        protected IFootprintService CreateClient(RestClientSession session)
        {
            AuthenticateTestUser(session);

            var host = Environment.MachineName;

            var uri = new Uri("http://"+host+"/footprint/api/v1/Footprint.svc");

            var client = session.CreateClient<IFootprintService>(uri, null);
            return client;
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
                footprint.FolderName = "TestFolder";

                request.Footprint = new V1.Footprint(footprint);

                client.CreateUserFootprint(request.Footprint.User, request.Footprint.FolderName, request.Footprint.Name, request);

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

                request.Footprint = new V1.Footprint(footprint);

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

        // TEST for folder actions

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
    }
}
