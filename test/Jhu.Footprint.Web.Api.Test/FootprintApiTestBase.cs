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
        
        protected Footprint CreateTestFootprint(string user, string owner, string name, bool @public)
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, user);
                var req = new FootprintRequest()
                {
                    Footprint = new Footprint()
                    {
                        Public = @public
                    }
                };
                return client.CreateUserFootprint(owner, name, req).Footprint;
            }
        }

        protected Footprint GetTestFootprint(string user, string owner, string name)
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, user);
                return client.GetUserFootprint(owner, name).Footprint;
            }
        }

        protected Footprint ModifyTestFootprint(string user, string owner, string name)
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, user);
                var footprint = client.GetUserFootprint(owner, name).Footprint;

                footprint.Comments = "modified";

                var req = new FootprintRequest()
                {
                    Footprint = footprint
                };

                footprint = client.ModifyUserFootprint(owner, name, req).Footprint;
                footprint = client.GetUserFootprint(owner, name).Footprint;

                return footprint;
            }
        }

        protected void DeleteTestFootprint(string user, string owner, string name)
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, user);
                client.DeleteUserFootprint(owner, name);
            }
        }

        protected FootprintRegion CreateTestRegion(string user, string owner, string name, string regionName)
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, user);
                var fp = client.GetUserFootprint(owner, name);
                var req = new FootprintRegionRequest()
                {
                    Region = new FootprintRegion()
                    {
                        RegionString = "CIRCLE J2000 10 10 10",
                        FillFactor = 0.8,
                    }
                };
               
                return client.CreateUserFootprintRegion(owner, name, regionName, req).Region;
            }
        }

        protected FootprintRegion GetTestRegion(string user, string owner, string name, string regionName)
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, user);
                return client.GetUserFootprintRegion(owner, name, regionName).Region;
            }
        }
    }
}
