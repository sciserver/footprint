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
                var url = "http://" + Environment.MachineName + AppSettings.WebUIPath + "/api/v1/Editor.svc";
                return url;
            }
        }

        protected Region TestRegion1
        {
            get
            {
                return GetTestRegion("RECT J2000 0 0 10 10");
            }
        }

        protected Region TestRegion2
        {
            get
            {
                return GetTestRegion("CIRCLE J2000 10 10 300");
            }
        }

        protected Rotation TestRotation
        {
            get
            {
                return new Rotation()
                {
                    Alpha = 5,
                    Beta = 10,
                    Gamma = 20,
                };
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

            var uri = new Uri(EditorApiBaseUrl);
            var client = session.CreateClient<IEditorService>(uri, null);

            return client;
        }

        protected Region GetTestRegion(string regionString)
        {
            return new Region()
            {
                RegionString = regionString,
                FillFactor = 0.8,
            };
        }

    }
}
