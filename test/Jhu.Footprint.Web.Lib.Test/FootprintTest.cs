using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jhu.Footprint.Web.Lib.Test
{
    [TestClass]
    public class FootprintTest
    {
        [TestMethod]
        public void CreateTest()
        {
            using (var context = new Context())
            {
                var footprint = new Footprint(context);

                footprint.Name = "Csík";
                footprint.User = "webtestuser";
                footprint.Public = 1;
                footprint.FillFactor = 0.9;
                footprint.FolderId = 2;
                footprint.FolderType = FolderType.None;

                footprint.Create();
            }
        }
    }
}
