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

        private long id;
        private string name;
        private long folderId;
        private Region region;
        private double fillFactor;
        private FootprintType type;

        #endregion
        #region Properties

        [DbColumn(Binding = DbColumnBinding.Key)]
        public long Id
        {
            get { return id; }
            set { id = value; }
        }

        [DbColumn]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public long FolderId
        {
            get { return folderId; }
            set { folderId = value; }
        }

        public SqlBytes RegionBinary
        {
            get { return region.ToSqlBytes(); }
            set { region = Region.FromSqlBytes(value); }
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
            this.folderId = 0;
            this.region = null;
            this.fillFactor = 0;
            this.type = FootprintType.None;
        }

        private void CopyMembers(Footprint old)
        {
            this.id = old.id;
            this.name = old.name;
            this.folderId = old.folderId;
            this.region = new Region(old.region);
            this.fillFactor = old.fillFactor;
            this.type = old.type;
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
