using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jhu.Spherical;
using System.Data;
using System.Data.SqlClient;

namespace Jhu.Footprint.Web.Lib
{
    public class FootprintFolderSearch: ContextObject
    {
        private FootprintSearchMethod searchMethod;
        private string user;
        private string name;
        private string @object;
        private Cartesian point;
        private Region region;
        private SearchSource source;

        public FootprintSearchMethod SearchMethod
        {
            get { return searchMethod; }
            set { searchMethod = value; }
        }

        public string User
        {
            get { return user; }
            set { user = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value;  }
        }

        public string Object
        {
            get { return @object; }
            set { @object = value; }
        }

        public Cartesian Point
        {
            get { return point; }
            set { point = value; }
        }

        public Region Region
        {
            get { return region; }
            set { region = value; }
        }

        public SearchSource Source
        {
            get { return source; }
            set { source = value; }
        }

        public FootprintFolderSearch()
        {

        }

        public FootprintFolderSearch(Context context)
            : base(context) 
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.name = "";
            this.user = "";
            this.@object = null;
            this.point = new Cartesian();
            this.region = null;
        }


        public int Count()
        {
            switch (SearchMethod)
            {
                case FootprintSearchMethod.Name:
                    return CountName();
                case FootprintSearchMethod.Object:
                    throw new NotImplementedException();
                case FootprintSearchMethod.Point:
                    throw new NotImplementedException();
                case FootprintSearchMethod.Intersect:
                    throw new NotImplementedException();

                default:
                    throw new NotImplementedException();
            }
        }
        public int CountName()
        {
            string sql = "fps.spCountFootprintFolderByName";

            using (var cmd = new SqlCommand(sql, Context.Connection, Context.Transaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 256).Value = this.name;
                cmd.Parameters.Add("@User", SqlDbType.NVarChar, 250).Value = this.user;
                //cmd.Parameters.Add("@Source", SqlDbType.Int).Value = (int)this.source;
                cmd.Parameters.Add("RETVAL", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

                return (int)cmd.Parameters["RETVAL"].Value;
            }
        }

        public IEnumerable<FootprintFolder> Find()
        {
            switch (SearchMethod)
            {
                case FootprintSearchMethod.Name:
                    return FindName();
                case FootprintSearchMethod.Object:
                    throw new NotImplementedException();
                case FootprintSearchMethod.Point:
                    throw new NotImplementedException();
                case FootprintSearchMethod.Intersect:
                    throw new NotImplementedException();

                default:
                    throw new NotImplementedException();
            }
        }

        private IEnumerable<FootprintFolder> FindName()
        { 
            var res = new List<FootprintFolder>();
            string sql = "fps.spFindFootprintFolderByName";

            using (var cmd = new SqlCommand(sql,Context.Connection,Context.Transaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Name",SqlDbType.NVarChar,256).Value = this.name;
                cmd.Parameters.Add("@User", SqlDbType.NVarChar, 250).Value = this.user; 
                cmd.Parameters.Add("@Source", SqlDbType.Int).Value = (int)this.source;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var ff = new FootprintFolder(Context);
                        ff.LoadFromDataReader(dr);

                        res.Add(ff);
                    }
                }
                
            }
            
            return res;
        }
    }
}
