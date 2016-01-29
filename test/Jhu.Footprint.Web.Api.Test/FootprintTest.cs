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

            var uri = new Uri("http://CASTOR/footprint/api/v1/Footprint.svc");

            var client = session.CreateClient<IFootprintService>(uri, null);
            return client;
        }

        [TestMethod]
        public void GetUserFootprintTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);
                var footprint = client.GetUserFootprint("evelin","SDSS.DR7","Stripe2");
            }

            /*
            var uri = new Uri("http://CASTOR/footprint/api/v1/Footprint.svc/users/evelin/footprints/SDSS.DR7/Stripe2");
            var req = (HttpWebRequest)WebRequest.Create(uri);

            req.Accept = MediaTypeNames.Text.Xml;

            var res = (HttpWebResponse)req.GetResponse();

            using (var reader = new StreamReader(res.GetResponseStream()))
            {
                var text = reader.ReadToEnd();
                System.Diagnostics.Debug.WriteLine(text);
            }
            */
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

                client.CreateUserFootprint("Evelin","Test","Test",request);
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
