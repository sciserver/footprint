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
            const string cstr = "Data Source=localhost;Initial Catalog=Footprint;Integrated Security=true";
            using (var context = new Context())
            {
                context.ConnectionString = cstr;

                var folder = new FootprintFolder(context);

                folder.Name = "CreateTest";
                folder.User = "ebanyai";
                folder.Type = 0;
                folder.Public = 1;
                folder.Comment = "FootprintFolder.Create Unit Test";

                folder.Create();


            }
        }
    }
}
