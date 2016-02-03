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
    public class FootprintFolderTest : ApiTestBase
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
        protected IFootprintFolderService CreateClient(RestClientSession session)
        {
            AuthenticateTestUser(session);

            var host = Environment.MachineName;
            var uri = new Uri("http://"+host+"/footprint/api/v1/FootprintFolder.svc");

            var client = session.CreateClient<IFootprintFolderService>(uri, null);
            return client;
        }

        [TestMethod]
        public void GetUserFootprintFolderTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var folder = client.GetUserFootprintFolder("evelin", "SDSS.DR7");
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
    }
}
