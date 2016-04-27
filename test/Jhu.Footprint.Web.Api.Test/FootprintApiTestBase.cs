using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Jhu.Graywulf.AccessControl;
using Jhu.Graywulf.Web.Api.V1;
using Jhu.Graywulf.Web.Services;
using Jhu.Footprint.Web.Lib;
using Jhu.Footprint.Web.Api.V1;

namespace Jhu.Footprint.Web.Api.V1
{
    public class FootprintApiTestBase: ApiTestBase
    {
        /*
         * Test prerequisites in Graywulf Registry:
         * Users: test, test-admin, test-writer, test-reader
         * Groups: testgroup
         * Roles: footprint-admin, footprint-writer, footprint-reader
         * test-* users in testgroup with role footprint-*
         * */

        protected const string TestUser = "test";
        protected const string OtherUser = "other";
        protected const string TestGroup = "testgroup";
        protected const string GroupAdminUser = "test-admin";
        protected const string GroupWriterUser = "test-writer";
        protected const string GroupReaderUser = "test-reader";

        protected static void InitializeDatabase()
        {
            FootprintTestBase.InitializeDatabase();
        }

        protected Context CreateContext()
        {
            return FootprintTestBase.CreateContext();
        }

        protected IFootprintService CreateClient(RestClientSession session)
        {
            return CreateClient(session, null);
        }

        protected IFootprintService CreateClient(RestClientSession session, string user)
        {
            if (user != null)
            {
                AuthenticateUser(session, user);
            }

            var host = Environment.MachineName;
            var uri = new Uri("http://" + host + "/footprint/api/v1/Footprint.svc");
            var client = session.CreateClient<IFootprintService>(uri, null);

            return client;
        }

        protected Principal CreateTestPrincipal()
        {
            return FootprintTestBase.CreateTestPrincipal();
        }

        protected Principal CreateOtherPrincipal()
        {
            return FootprintTestBase.CreateOtherPrincipal();
        }
    }
}
