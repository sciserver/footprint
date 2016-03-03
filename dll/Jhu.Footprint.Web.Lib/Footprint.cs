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
    public class Footprint : Entity
    {
        #region Member variables

        private long id;
        private string name;
        private string user;
        private byte @public;                // public flag, >0 when visible to the public
        private DateTime dateCreated;
        private Region region;
        private double fillFactor;
        private FootprintType type;
        private long folderId;
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

        public FootprintType Type
        {
            get { return type; }
            set { type = value; }
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

        public Footprint(Footprint old)
            : base(old)
        {
            CopyMembers(old);
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
            this.type = FootprintType.None;
            this.folderId = 0;
            this.comment = "";
        }

        private void CopyMembers(Footprint old)
        {
            this.id = old.id;
            this.name = old.name;
            this.user = old.user;
            this.@public = old.@public;
            this.dateCreated = old.dateCreated;
            this.region = new Region(old.region);
            this.fillFactor = old.fillFactor;
            this.type = old.type;
            this.folderId = old.folderId;
            this.comment = old.comment;
        }

        #endregion

        #region SQL - get commands
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

            string sql = "fps.spDeleteFootprint";
            var cmd = new SqlCommand(sql);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("@FootprintId", SqlDbType.BigInt).Value = id;
            cmd.Parameters.Add("@User", SqlDbType.NVarChar, 250).Value = Context.User;

            cmd.Parameters.Add("RETVAL", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
            return cmd;
        }

        protected override SqlCommand GetLoadCommand()
        {

            var sql = "fps.spGetFootprint";
            var cmd = new SqlCommand(sql);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@User", SqlDbType.NVarChar, 250).Value = Context.User;
            cmd.Parameters.Add("@FootprintId", SqlDbType.BigInt).Value = id;

            return cmd;
        }

        private void AppendCreateModifyParameters(SqlCommand cmd)
        {
            cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 256).Value = name;
            cmd.Parameters.Add("@User", SqlDbType.NVarChar, 250).Value = Context.User;
            cmd.Parameters.Add("@Public", SqlDbType.TinyInt).Value = @public;
            cmd.Parameters.Add("@FillFactor", SqlDbType.Float).Value = fillFactor;
            cmd.Parameters.Add("@FootprintType", SqlDbType.TinyInt).Value = type;
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
            this.type = (FootprintType)Enum.ToObject(typeof(FootprintType), dr["FootprintType"]);
            this.folderId = (long)dr["FolderID"];
            this.comment = (string)dr["Comment"];

            var o = dr.GetOrdinal("RegionBinary");
            if (!dr.IsDBNull(o))
            {
                var bytes = dr.GetSqlBytes(o);
                this.region = bytes.IsNull ? null : Jhu.Spherical.Region.FromSqlBytes(bytes);
            }
            else
            {
                this.region = null;
            }
        }

        protected override SqlCommand GetIsNameDuplicateCommand()
        {
            var sql = "fps.spIsDuplicateFootprintName";
            var cmd = new SqlCommand(sql);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@User", SqlDbType.NVarChar, 250).Value = this.user;
            cmd.Parameters.Add("@FootprintId", SqlDbType.BigInt).Value = this.id;
            cmd.Parameters.Add("@folderId", SqlDbType.BigInt).Value = this.folderId;
            cmd.Parameters.Add("@FootprintName", SqlDbType.NVarChar, 256).Value = this.name;

            cmd.Parameters.Add("@Match", SqlDbType.Int).Direction = ParameterDirection.Output;

            return cmd;
        }
        #endregion

        #region Methods
        public override void Save()
        {
            if (IsNameDuplicate())
            {
                throw Error.DuplicateFootprintName(this.name);
            }


            if (this.id == 0)
            {
                Create();
            }
            else
            {
                Modify();
            }
        }

        private void Create()
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
            }

        }

        private void Modify()
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


        #endregion
    }
}
