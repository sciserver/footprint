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
        protected Spherical.Region MakeTestCircle(int i)
        {
            Spherical.Region r;

            switch (i)
            {
                case 0:
                    r = Spherical.Region.Parse("CIRCLE J2000 0.0 0.0 30");
                    break;
                case 1:
                    r = Spherical.Region.Parse("CIRCLE J2000 0.1 0.0 30");
                    break;
                case 2:
                    r =  Spherical.Region.Parse("CIRCLE J2000 -0.1 0.0 30");
                    break;
                default:
                    throw new NotImplementedException();
            }

            r.Simplify();
            return r;
        }
        
        public static Principal CreateTestPrincipal()
        {
            return new Principal()
            {
                Identity = new Identity()
                {
                    IsAuthenticated = true,
                    Name = TestUser,
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
                    Name = OtherUser,
                }
            };
        }

        public static FootprintContext CreateContext()
        {
            var context = new FootprintContext()
            {
                Principal = CreateTestPrincipal()
            };

            return context;
        }

        private static void RunScript(string filename)
        {
            using (var context = CreateContext())
            {
                var script = File.ReadAllText(GetTestFilePath(filename));
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

            RunScript(@"modules\footprint\sql\Jhu.Footprint.Logic.Drop.sql");
            RunScript(@"modules\footprint\sql\Jhu.Footprint.Tables.Drop.sql");
            RunScript(bin + @"\Jhu.Graywulf.Entities.Sql.Drop.sql");
            RunScript(bin + @"\Jhu.Spherical.Sql.Drop.sql");

            RunScript(bin + @"\Jhu.Spherical.Sql.Create.sql");
            RunScript(bin + @"\Jhu.Graywulf.Entities.Sql.Create.sql");
            RunScript(@"modules\footprint\sql\Jhu.Footprint.Tables.Create.sql");
            RunScript(@"modules\footprint\sql\Jhu.Footprint.Logic.Create.sql");
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

        protected Footprint CreateTestFootprint(FootprintContext context, string name)
        {
            return CreateTestFootprint(context, name, CombinationMethod.None);
        }

        protected Footprint CreateTestFootprint(FootprintContext context, string name, CombinationMethod combinationMethod)
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
            region.Region.Simplify();
            region.Save();
            region.SaveRegion();

            return region;
        }

        protected FootprintRegion CreateTestFootprintAndRegion(FootprintContext context, string name)
        {
            var footprint = CreateTestFootprint(context, name);
            var region = CreateTestRegion(footprint, name);

            return region;
        }
    }
}
