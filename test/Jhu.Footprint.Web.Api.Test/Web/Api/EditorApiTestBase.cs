using System;
using Jhu.Graywulf.Web.Api.V1;
using Jhu.Graywulf.Web.Services;
using Jhu.Footprint.Web.Lib;

namespace Jhu.Footprint.Web.Api.V1
{
    public class EditorApiTestBase : ApiTestBase
    {
        protected static void InitializeDatabase()
        {
            FootprintTestBase.InitializeDatabase();
        }

        protected string EditorApiBaseUrl
        {
            get
            {
                var url = "http://" + Environment.MachineName + "/" + AppSettings.WebUIPath + "/api/v1/Editor.svc";
                return url;
            }
        }

        protected FootprintContext CreateContext()
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
            var uri = new Uri("http://" + host + "/" + AppSettings.WebUIPath + "/api/v1/Editor.svc");
            var client = session.CreateClient<IEditorService>(uri, null);

            return client;
        }

        protected RegionRequest GetTestRegion()
        {
            return GetTestRegion("CIRCLE J2000 10 10 10");
        }

        protected RegionRequest GetTestRegion(string regionString)
        {
            var req = new RegionRequest()
            {
                Region = new Region()
                {
                    RegionString = regionString,
                    FillFactor = 0.8,
                }
            };
            return req;
        }

    }
}
