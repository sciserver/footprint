using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace Jhu.Footprint.Web.Lib
{
    public class FootprintTestBase
    {
        protected static void InitDatabase()
        {
            using (var context = new Context())
            {
                string path = Path.GetDirectoryName((string)Environment.GetEnvironmentVariables()["SolutionPath"]);

                ExecuteSqlScript(context, File.ReadAllText(Path.Combine(path, @"footprint\sql\Jhu.Footprint.Tables.sql")));
                
                ExecuteSqlBulkInsert(context, "Footprint", Path.Combine(path, @"footprint\data\test\footprint.dat"));
                ExecuteSqlBulkInsert(context, "FootprintFolder", Path.Combine(path, @"footprint\data\test\footprintfolder.dat"));
            }
        }

        private static void ExecuteSqlScript(Context context, string script)
        {
            var scripts = script.Split(new string[] { "\r\nGO", "\nGO" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var sql in scripts)
            {
                using (var cmd = new SqlCommand(sql, context.Connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static void ExecuteSqlBulkInsert(Context context, string table, string filename)
        {
            var sql = @"BULK INSERT {0} FROM '{1}' WITH (DATAFILETYPE = 'native')";
            sql = String.Format(sql, table, filename);

            using (var cmd = new SqlCommand(sql, context.Connection))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}
