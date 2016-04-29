using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using Jhu.Graywulf.AccessControl;

namespace Jhu.Footprint.Web.Lib
{
    public class FootprintTestBase : Jhu.Graywulf.Test.TestClassBase
    {
        protected const string TestUser = "test";
        protected const string OtherUser = "other";

        protected static string MapSolutionRelativePath(string path)
        {
            var dir = Path.GetDirectoryName(Environment.GetEnvironmentVariable("SolutionPath"));
            return Path.Combine(dir, path);
        }

        protected static string MapProjectRelativePath(string path)
        {
            return Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\", path);
        }

        public static Principal CreateTestPrincipal()
        {
            return new Principal()
            {
                Identity = new Identity()
                {
                    IsAuthenticated = true,
                    Name = TestUser
                }
            };
        }

        public static Principal CreateOtherPrincipal()
        {
            return new Principal()
            {
                Identity = new Identity()
                {
                    IsAuthenticated = true,
                    Name = OtherUser
                }
            };
        }

        public static Context CreateContext()
        {
            var context = new Context()
            {
                Principal = CreateTestPrincipal()
            };

            return context;
        }

        private static void RunScript(string filename)
        {
            using (var context = CreateContext())
            {
                var script = File.ReadAllText(MapSolutionRelativePath(filename));
                context.ExecuteScriptNonQuery(script);
            }
        }

        public static void InitializeDatabase()
        {
#if DEBUG
            string bin = @"bin\Debug";
#else
            string bin = @"bin\Release";
#endif

            RunScript(@"footprint\sql\Jhu.Footprint.Logic.Drop.sql");
            RunScript(@"footprint\sql\Jhu.Footprint.Tables.Drop.sql");
            RunScript(bin + @"\Graywulf.Entities.Sql.Drop.sql");
            RunScript(bin + @"\Jhu.Spherical.Sql.Drop.sql");

            RunScript(bin + @"\Jhu.Spherical.Sql.Create.sql");
            RunScript(bin + @"\Graywulf.Entities.Sql.Create.sql");
            RunScript(@"footprint\sql\Jhu.Footprint.Tables.Create.sql");
            RunScript(@"footprint\sql\Jhu.Footprint.Logic.Create.sql");
        }

        /*
        private static void ExecuteSqlBulkInsert(Context context, string table, string filename)
        {
            var sql = @"BULK INSERT {0} FROM '{1}' WITH (DATAFILETYPE = 'native')";
            sql = String.Format(sql, table, filename);

            using (var cmd = new SqlCommand(sql, context.Connection))
            {
                cmd.ExecuteNonQuery();
            }
        }*/

        protected Footprint CreateTestFootprint(Context context, string name)
        {
            return CreateTestFootprint(context, name, CombinationMethod.None);
        }

        protected Footprint CreateTestFootprint(Context context, string name, CombinationMethod combinationMethod)
        {
            var footprint = new Footprint(context)
            {
                Name = name,
                CombinationMethod = combinationMethod,
                Comments = name + " test",
            };

            footprint.Save();

            return footprint;
        }

        protected FootprintRegion CreateTestRegion(Footprint footprint, string name)
        {
            var region = new FootprintRegion(footprint)
            {
                Name = name,
                Type = RegionType.Single,
                Region = Spherical.Region.Parse("CIRCLE J2000 10 10 10")
            };

            region.Save();

            return region;
        }

        protected FootprintRegion CreateTestFootprintAndRegion(Context context, string name)
        {
            var footprint = CreateTestFootprint(context, name);
            var region = CreateTestRegion(footprint, name);

            return region;
        }
    }
}
