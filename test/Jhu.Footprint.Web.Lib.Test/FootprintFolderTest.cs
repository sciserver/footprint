using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jhu.Footprint.Web.Lib.Test
{
    [TestClass]
    public class FootprintFolderTest
    {
        [TestMethod]
        public void CreateTest()
        {
            using (var context = new Context())
            {
                
                var folder = new FootprintFolder(context);

                folder.Name = "CreateTest2";
                folder.User = "webtestuser";
                folder.Type = 0;
                folder.Public = 1;
                folder.Comment = "FootprintFolder.Create Unit Test";

                folder.Create();


            }
        }

        [TestMethod]

        public void ModifyTest()
        {
            using (var context = new Context())
            {
                var folder = new FootprintFolder(context);

                folder.Id = 7;
                folder.Name = "CreateTest3";
                folder.User = "webtestuser";
                folder.Type = 0;
                folder.Public = 0;
                folder.Comment = "FootprintFolder.Modify Unit Test";

                folder.Modify();
            }
        }
    }
}
