using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using Jhu.Graywulf.Entities.AccessControl;

namespace Jhu.Footprint.Web.Lib
{
    public class FootprintTestBase
    {
        protected const string TestUser = "test";
        protected const string OtherUser = "other";

        protected static string MapPath(string path)
        {
            return Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\", path);
        }

        protected static Identity CreateTestIdentity()
        {
            return new Identity()
            {
                IsAuthenticated = true,
                Name = TestUser
            };
        }

        protected static Identity CreateOtherIdentity()
        {
            return new Identity()
            {
                IsAuthenticated = true,
                Name = OtherUser
            };
        }

        protected static Context CreateContext()
        {
            var context = new Context()
            {
                Identity = CreateTestIdentity()
            };

            return context;
        }

        protected static void InitializeDatabase()
        {
            using (var context = CreateContext())
            {
                string script = File.ReadAllText(MapPath(@"sql\Jhu.Footprint.Tables.sql"));
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
    }
}
