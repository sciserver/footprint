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
using Jhu.Graywulf.Entities.Mapping;

namespace Jhu.Footprint.Web.Lib
{
    public class Footprint : Jhu.Graywulf.Entities.Entity
    {
        #region Member variables

        private int id;
        private int folderId;
        private string name;
        private double fillFactor;
        private FootprintType type;
        private Region region;
        private byte[] thumbnail;

        #endregion
        #region Properties

        [DbColumn(Binding = DbColumnBinding.Key)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [DbColumn]
        public int FolderId
        {
            get { return folderId; }
            set { folderId = value; }
        }

        [DbColumn]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DbColumn]
        public double FillFactor
        {
            get { return fillFactor; }
            set { fillFactor = value; }
        }

        [DbColumn]
        public FootprintType Type
        {
            get { return type; }
            set { type = value; }
        }

        public Region Region
        {
            get { return region; }
            set { region = value; }
        }

        [DbColumn]
        public SqlBytes RegionBinary
        {
            get { return region.ToSqlBytes(); }
            set { region = Region.FromSqlBytes(value); }
        }

        [DbColumn]
        public SqlBytes Thumbnail
        {
            get { return new SqlBytes(thumbnail); }
            set { thumbnail = value.Value; }
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
            this.folderId = 0;
            this.name = "";
            this.fillFactor = 0;
            this.type = FootprintType.None;
            this.region = null;
            this.thumbnail = null;
        }

        private void CopyMembers(Footprint old)
        {
            this.id = old.id;
            this.folderId = old.folderId;
            this.name = old.name;
            this.fillFactor = old.fillFactor;
            this.type = old.type;
            this.region = new Region(old.region);
            this.thumbnail = old.thumbnail;
        }

        #endregion

        protected Boolean IsNameDuplicate()
        {
            var sql = @"
SELECT COUNT(*) FROM Footprint
WHERE FolderId = @FolderId
      AND FootprintID != @FootprintId
	  AND Name = @Name";

            using (var cmd = Context.CreateCommand(sql))
            {
                cmd.Parameters.Add("@folderId", SqlDbType.BigInt).Value = this.folderId;
                cmd.Parameters.Add("@FootprintId", SqlDbType.BigInt).Value = this.Id;
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 256).Value = this.Name;

                return (int)Context.ExecuteCommandScalar(cmd) > 0;
            }
        }

        #region Methods

        protected override void OnSaving(Graywulf.Entities.EntityEventArgs e)
        {
            // TODO: validate name

            if (IsNameDuplicate())
            {
                throw Error.DuplicateFootprintName(this.Name);
            }

            base.OnSaving(e);
        }

        #endregion
    }
}
