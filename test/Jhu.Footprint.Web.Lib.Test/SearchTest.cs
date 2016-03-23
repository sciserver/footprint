using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Test;

namespace Jhu.Footprint.Web.Lib.Test
{
    [TestClass]
    public class SearchTest : FootprintTestBase
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            InitDatabase();
        }

        [TestMethod]
        public void FindNameTest()
        {
            using (var context = new Context())
            {
                var search = new FootprintFolderSearch(context);

                search.Name = "T";
                search.User = "webtestuser";
                search.Source = SearchSource.Public | SearchSource.My;
                search.SearchMethod = FootprintSearchMethod.Name;

                var res = search.Find();
            }
        }
    }
}
