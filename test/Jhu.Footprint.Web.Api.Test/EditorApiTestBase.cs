using System;
using Jhu.Graywulf.Web.Api.V1;
using Jhu.Graywulf.Web.Services;
using Jhu.Footprint.Web.Lib;

namespace Jhu.Footprint.Web.Api.V1
{
    public class EditorApiTestBase : ApiTestBase
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

        protected IEditorService CreateClient(RestClientSession session)
        {
            return CreateClient(session, null);
        }

        protected IEditorService CreateClient(RestClientSession session, string user)
        {
            if (user != null)
            {
                AuthenticateUser(session, user);
            }

            var host = Environment.MachineName;
            var uri = new Uri("http://" + host + "/footprint/api/v1/Editor.svc");
            var client = session.CreateClient<IEditorService>(uri, null);

            return client;
        }

        protected FootprintRegionRequest GetTestRegion()
        {
            return GetTestRegion("CIRCLE J2000 10 10 10");
        }

        protected FootprintRegionRequest GetTestRegion(string regionString)
        {
            var req = new FootprintRegionRequest()
            {
                Region = new FootprintRegion()
                {
                    RegionString = regionString,
                    FillFactor = 0.8,
                }
            };
            return req;
        }

    }
}
