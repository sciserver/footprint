using System;
using Jhu.Graywulf.Web.Api.V1;
using Jhu.Graywulf.Web.Services;
using Jhu.Footprint.Web.Lib;
using Jhu.Footprint.Web.Api.V1;

namespace Jhu.Footprint.Web.Api.V1
{
    public class FootprintApiTestBase : ApiTestBase
    {
        protected static void InitializeDatabase()
        {
            FootprintTestBase.InitializeDatabase();
        }

        protected string FootprintApiBaseUrl
        {
            get
            {
                var url = "http://" + Environment.MachineName + "/" + AppSettings.WebUIPath + "/api/v1/Footprint.svc";
                return url;
            }
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
            var uri = new Uri(FootprintApiBaseUrl);
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

        protected FootprintRegion ModifyTestRegion(string user, string owner, string name, string regionName)
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, user);
                var region = client.GetUserFootprintRegion(owner, name, regionName).Region;

                region.FillFactor = 0.7;
                region.RegionString = "CIRCLE J2000 20 20 20";

                var req = new FootprintRegionRequest(region);

                client.ModifyUserFootprintRegion(owner, name, regionName, req);

                return client.GetUserFootprintRegion(owner, name, regionName).Region;
            }
        }

        protected void DeleteTestRegion(string user, string owner, string name, string regionName)
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, user);
                client.DeleteUserFootprintRegion(owner, name, regionName);
            }
        }

    }
}
