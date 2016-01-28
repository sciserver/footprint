﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Test;

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
                var search = new FootprintFolderSearch(context);

                search.Name = "Test";
                search.User = "webtestuser";
                search.SearchMethod = FootprintSearchMethod.Name;

                search.Find();
            }
        }
    }
}