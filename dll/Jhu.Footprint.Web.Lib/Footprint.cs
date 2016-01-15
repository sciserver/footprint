using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Jhu.Spherical;
using System.Data;
using System.Data.SqlClient;

namespace Jhu.Footprint.Web.Lib
{
    public class Footprint: ContextObject
    {
        #region Member variables

        private long id;
        private string name;
        private string user;               // user database guid
        private byte @public;                // public flag, >0 when visible to the public
        private DateTime dateCreated;
        private Region region;
        private double fillFactor;
        private string type;
        private byte mask;
        private FolderType folderType;
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

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public byte Mask
        {
            get { return mask; }
            set { mask = value; }
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
            this.folderId = 0;
            this.comment = "";
        }

        protected override System.Data.SqlClient.SqlCommand GetCreateCommand()
        {
            throw new NotImplementedException();
        }

        protected override System.Data.SqlClient.SqlCommand GetModifyCommand()
        {
            throw new NotImplementedException();
        }

        protected override System.Data.SqlClient.SqlCommand GetDeleteCommand()
        {
            string sql = "fps.spDeleteFootprint";
            var cmd = new SqlCommand(sql);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("@FootprintId", SqlDbType.BigInt).Value = id;
            cmd.Parameters.Add("@User", SqlDbType.NVarChar, 250).Value = user;

            cmd.Parameters.Add("RETVAL", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
            return cmd;
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
    }
}
