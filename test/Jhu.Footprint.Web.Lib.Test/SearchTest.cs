using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jhu.Footprint.Web.Lib.Test
{
    [TestClass]
    public class SearchTest
    {
        [TestMethod]
        public void FindNameTest()
        {
            using (var context = new Context())
            {
                var search = new Search(context);

                search.Name = "SDSS";
                search.User = "webtestuser";
                search.SearchMethod = FootprintSearchMethod.Name;

                search.Find();
            }
        }
    }
}
