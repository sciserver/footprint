using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jhu.Footprint.Web.Api.Test
{
    [TestClass]
    public class FootprintTest
    {
        [TestMethod]
        public void GetUserFootprintTest()
        {
            var uri = new Uri("http://localhost/footprint/api/v1/Footprint.svc/users/evelin/footprints/SDSS.DR7/Stripe2");
            var req = (HttpWebRequest)WebRequest.Create(uri);

            req.Accept = MediaTypeNames.Text.Xml;

            var res = (HttpWebResponse)req.GetResponse();

            using (var reader = new StreamReader(res.GetResponseStream()))
            {
                var text = reader.ReadToEnd();
                System.Diagnostics.Debug.WriteLine(text);
            }
        }
    }
}
