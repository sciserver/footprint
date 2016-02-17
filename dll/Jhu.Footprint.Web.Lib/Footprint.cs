using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Jhu.Spherical;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;

namespace Jhu.Footprint.Web.Lib
{
    public class Footprint: Entity
    {
        #region Member variables

        private long id;
        private string name;
        private string user;               
        private byte @public;                // public flag, >0 when visible to the public
        private DateTime dateCreated;
        private Region region;
        private double fillFactor;
        private FolderType folderType;
        private long folderId;
        private string folderName;
        private string comment;
        #endregion

        #region Properties
        /// <summary>
        /// Returns or sets the database ID of the region. Set this before loading or
        /// modifying. If set to 0, Save() will create a new record in the database.
        /// </summary>
        public long Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Returns or sets the user ID. Set this before doing any database operation
        /// since it determines the security context for the stored procedures.
        /// </summary>

        [XmlIgnore]
        public string User
        {
            get { return user; }
            set { user = value; }
        }

        /// <summary>
        /// Returns or sets the public flag of the region. Set
        /// </summary>
        public byte Public
        {
            get { return @public; }
            set { @public = value; }
        }

        public DateTime DateCreated
        {
            get { return dateCreated; }
            private set { dateCreated = value; }
        }

        public long FolderId
        {
            get { return folderId; }
            set { folderId = value; }
        }

        public string FolderName
        {
            get { return folderName; }
            set { folderName = value; }
        }


        public Region Region
        {
            get { return region; }
            set { region = value; }
        }

        public double FillFactor
        {
            get { return fillFactor; }
            set { fillFactor = value; }
        }
        
        public FolderType FolderType
        {
            get { return folderType; }
            set { folderType = value; }
        }

        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        #endregion

        #region Constructors & Initializer
        public Footprint()
        {
            InitializeMembers();
        }

        public Footprint(Context context)
            : base(context)
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.id = 0;
            this.name = "";
            this.user = "";
            this.@public = 0;
            this.dateCreated = DateTime.Now;
            this.region = null;
            this.fillFactor = 0;
            this.folderType = FolderType.None;
            this.folderId = 0;
            this.folderName = "";
            this.comment = "";
        }
        #endregion

        #region Methods
        private Boolean FootprintNameIsAvailable()
        {
            throw new NotImplementedException();
        }

        protected override System.Data.SqlClient.SqlCommand GetCreateCommand()
        {
            string sql = "fps.spCreateFootprint";
            var cmd = new SqlCommand(sql);

            cmd.CommandType = CommandType.StoredProcedure;

            AppendCreateModifyParameters(cmd);

            cmd.Parameters.Add("@NewID", SqlDbType.BigInt).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("RETVAL", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
            return cmd;
        }

        protected override System.Data.SqlClient.SqlCommand GetModifyCommand()
        {
            if (this.id == 0) { GetFootprintId(); } 

            string sql = "fps.spModifyFootprint";
            var cmd = new SqlCommand(sql);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@FootprintId", SqlDbType.BigInt).Value = id;
            AppendCreateModifyParameters(cmd);

            cmd.Parameters.Add("RETVAL", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
            return cmd;
        }

        protected override System.Data.SqlClient.SqlCommand GetDeleteCommand()
        {
            if (this.id == 0) { GetFootprintId(); } 

            string sql = "fps.spDeleteFootprint";
            var cmd = new SqlCommand(sql);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("@FootprintId", SqlDbType.BigInt).Value = id;
            cmd.Parameters.Add("@User", SqlDbType.NVarChar, 250).Value = user;

            cmd.Parameters.Add("RETVAL", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
            return cmd;
        }

        protected override SqlCommand GetLoadCommand()
        {
            if (this.id == 0) { GetFootprintId(); } 
            
            var sql = "fps.spGetFootprint";
            var cmd = new SqlCommand(sql);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@User", SqlDbType.NVarChar, 250).Value = user;
            cmd.Parameters.Add("@FootprintId", SqlDbType.BigInt).Value = id;
            
            return cmd;
        }

        private void AppendCreateModifyParameters(SqlCommand cmd)
        {
            cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 256).Value = name;
            cmd.Parameters.Add("@User", SqlDbType.NVarChar, 250).Value = user;
            cmd.Parameters.Add("@Public", SqlDbType.TinyInt).Value = @public;
            cmd.Parameters.Add("@FillFactor", SqlDbType.Float).Value = fillFactor;
            cmd.Parameters.Add("@FolderType", SqlDbType.TinyInt).Value = folderType;
            cmd.Parameters.Add("@FolderId", SqlDbType.BigInt).Value = folderId;
            cmd.Parameters.Add("@Comment", SqlDbType.NVarChar, -1).Value = comment;

            SqlBytes bytes;
            using (var ms = new MemoryStream())
            {
                using (var w = new Spherical.IO.RegionWriter(ms))
                {
                    if (this.region != null)
                    {
                    w.Write(this.region);
                    }
                    else
                    {
                        w.Write(0);
                    }
                }
                bytes = new SqlBytes(ms.ToArray());
            }

            cmd.Parameters.Add("@RegionBinary", SqlDbType.VarBinary, -1).Value = bytes;
        }

        public override void LoadFromDataReader(SqlDataReader dr)
        {
            this.id = (long)dr["FootprintID"];
            this.name = (string)dr["Name"];
            this.user = (string)dr["User"];
            this.@public = (byte)dr["Public"];
            this.dateCreated = (DateTime)dr["DateCreated"];
            this.fillFactor = (double)dr["FillFactor"];
            this.folderType = (FolderType)Enum.ToObject(typeof(FolderType),dr["FolderType"]);
            this.folderId = (long)dr["FolderID"];
            this.comment = (string)dr["Comment"];
            this.folderName = (string)dr["FolderName"];

            if (!dr.IsDBNull(dr.GetOrdinal("RegionBinary")))
            {
                var bytes = (SqlBytes)dr["RegionBinary"];
                this.region = bytes.IsNull ? null : Jhu.Spherical.Region.FromSqlBytes(bytes);
            }
            else
            {
                this.region = null;
            }
        }

        public void Create()
        {
            using (var cmd = GetCreateCommand())
            {
                cmd.Connection = Context.Connection;
                cmd.Transaction = Context.Transaction;

                cmd.ExecuteNonQuery();

                int retval = (int)cmd.Parameters["RETVAL"].Value;

                if (retval == 0)
                {
                    throw new Exception("Cannot create Footprint.");
                }
                else
                {
                    this.id = (long)cmd.Parameters["@NewID"].Value;
                }

                // TODO
                // throw Error.DuplicateFootprintName(name);
            }

        }

        public void Modify()
        {
            using (var cmd = GetModifyCommand())
            {
                cmd.Connection = Context.Connection;
                cmd.Transaction = Context.Transaction;

                cmd.ExecuteNonQuery();

                int retval = (int)cmd.Parameters["RETVAL"].Value;

                if (retval == 0)
                {
                    throw new Exception("Cannot update Footprint");
                }

            }
        }

        public void Delete()
        {
            using (var cmd = GetDeleteCommand())
            {
                cmd.Connection = Context.Connection;
                cmd.Transaction = Context.Transaction;

                cmd.ExecuteNonQuery();

                int retval = (int)cmd.Parameters["RETVAL"].Value;

                if (retval == 0)
                {
                    throw new Exception("Cannot delete Footprint.");
                }
            }
        }

        private void GetFootprintId()
        {
            var sql = "fps.spGetFootprintId";
            using (var cmd = new SqlCommand(sql))
            {
                cmd.Connection = Context.Connection;
                cmd.Transaction = Context.Transaction;

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@User", SqlDbType.NVarChar, 250).Value = user;
                cmd.Parameters.Add("@FolderName", SqlDbType.NVarChar, 256).Value = folderName;
                cmd.Parameters.Add("@FootprintName", SqlDbType.NVarChar, 256).Value = name;
                cmd.Parameters.Add("@FootprintId", SqlDbType.BigInt).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                this.id = (long)cmd.Parameters["@FootprintId"].Value;
            }
        }
        #endregion
    }
}
