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
    public class FootprintTestBase
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

        public static void InitializeDatabase()
        {
            string script;
            string bin;

#if DEBUG
            bin = @"bin\Debug";
#else
                bin = @"bin\Release";
#endif

            using (var context = CreateContext())
            {
                script = File.ReadAllText(MapSolutionRelativePath(bin + @"\Graywulf.Entities.Sql.Drop.sql"));
                context.ExecuteScriptNonQuery(script);
            }

            using (var context = CreateContext())
            {
                script = File.ReadAllText(MapSolutionRelativePath(bin + @"\Graywulf.Entities.Sql.Create.sql"));
                context.ExecuteScriptNonQuery(script);
            }

            using (var context = CreateContext())
            {
                script = File.ReadAllText(MapProjectRelativePath(@"sql\Jhu.Footprint.Tables.sql"));
                context.ExecuteScriptNonQuery(script);
            }
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

        protected FootprintRegion CreateRegion(Context context, string name)
        {
            var footprint = new Footprint(context)
            {
                Name = name,
            };

            footprint.Save();

            var region = new FootprintRegion(footprint)
            {
                Name = name,
                Type = RegionType.Region,
                Region = Spherical.Region.Parse("CIRCLE J2000 10 10 10")
            };

            region.Save();

            return region;
        }
    }
}
