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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Jhu.Graywulf.Entities.Mapping;
using Jhu.Graywulf.AccessControl;
using Jhu.Spherical.Visualizer;

namespace Jhu.Footprint.Web.Lib
{
    [DbTable]
    public class FootprintRegion : Jhu.Graywulf.Entities.Entity
    {
        #region Member variables

        private Footprint parent;

        private int id;
        private int footprintId;
        private string name;
        private string footprintName;
        private string footprintOwner;
        private double fillFactor;
        private RegionType type;
        private Spherical.Region region;
        private byte[] thumbnail;
        

        #endregion
        #region Properties

        [DbKey]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [DbColumn]
        public int FootprintId
        {
            get { return footprintId; }
            set { footprintId = value; }
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
        public RegionType Type
        {
            get { return type; }
            set { type = value; }
        }

        public Spherical.Region Region
        {
            get
            {
                if (region == null)
                {
                    LoadRegion();
                }
                return region;
            }
            set { region = value; }
        }

        [DbColumn(Name = "Region", Binding = DbColumnBinding.Auxiliary)]
        public byte[] RegionBinary
        {
            get { return region.ToSqlBytes().Value; }
            set { region = Spherical.Region.FromSqlBytes(new SqlBytes(value)); }
        }

        [DbColumn(Binding = DbColumnBinding.Auxiliary)]
        public byte[] Thumbnail
        {
            get
            {
                if (thumbnail == null)
                {
                    LoadThumbnail();
                }
                return thumbnail;
            }
            set { thumbnail = value; }
        }

        [DbColumn(Name = "FootprintName", Binding = DbColumnBinding.Auxiliary)]
        public string FootprintName
        {
            get { return footprintName; }
            set { footprintName = value; }

        }

        [DbColumn(Name = "Owner", Binding = DbColumnBinding.Auxiliary)]
        public string FootprintOwner
        {
            get { return footprintOwner; }
            set { footprintOwner = value;  }
        }
        
        #endregion
        #region Constructors and initializers

        public FootprintRegion()
        {
            InitializeMembers();
        }

        public FootprintRegion(Footprint footprint)
            : base(footprint.Context)
        {
            InitializeMembers();

            this.parent = footprint;
            this.footprintId = footprint.Id;
        }

        public FootprintRegion(FootprintRegion old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.parent = null;

            this.id = 0;
            this.footprintId = 0;
            this.name = "";
            this.footprintName = "";
            this.fillFactor = 1.0;
            this.type = RegionType.Single;
            this.region = null;
            this.thumbnail = null;
        }

        private void CopyMembers(FootprintRegion old)
        {
            this.parent = new Footprint(old.parent);

            this.id = old.id;
            this.footprintId = old.footprintId;
            this.name = old.name;
            this.fillFactor = old.fillFactor;
            this.type = old.type;
            this.region = new Spherical.Region(old.region);
            this.thumbnail = old.thumbnail;
        }

        #endregion

        #region Methods

        public void Load(string regionName)
        {
            this.name = regionName;
            Load();
        }

        public bool CheckExists(string regionName)
        {
            this.name = regionName;
            return CheckExists();
        }

        protected override void OnValidating(Graywulf.Entities.EntityEventArgs e)
        {
            if (Constants.RestictedNames.Contains(this.name))
            {
                throw Error.FootprintNameNotAvailable(this.name);
            }

            if (!Constants.NamePattern.Match(this.name).Success)
            {
                throw Error.FootprintNameInvalid(this.name);
            }

            if (IsNameDuplicate())
            {
                throw Error.DuplicateFootprintRegionName(this.name);
            }

            base.OnValidating(e);
        }

        private AccessCollection EvaluateAccess()
        {
            if (parent == null)
            {
                parent = new Footprint((Context)Context);
            }

            if (!parent.IsLoaded)
            {
                parent.Load(this.footprintId);
            }

            return parent.Permissions.EvaluateAccess(Context.Principal);
        }

        protected override void OnCreating(Graywulf.Entities.EntityEventArgs e)
        {
            EvaluateAccess().EnsureUpdate();

            base.OnCreating(e);
        }

        protected override void OnModifying(Graywulf.Entities.EntityEventArgs e)
        {
            EvaluateAccess().EnsureUpdate();

            base.OnModifying(e);
        }

        protected override void OnDeleting(Graywulf.Entities.EntityEventArgs e)
        {
            EvaluateAccess().EnsureUpdate();

            base.OnDeleting(e);
        }

        protected override void OnDeleted(Graywulf.Entities.EntityEventArgs e)
        {
            parent.RefreshCombinedRegion();

            base.OnDeleted(e);
        }

        protected Boolean IsNameDuplicate()
        {
            var sql = @"
SELECT COUNT(*) FROM FootprintRegion
WHERE ID != @ID
      AND FootprintID = @FootprintID
	  AND Name = @Name";

            using (var cmd = Context.CreateCommand(sql))
            {
                cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = this.Id;
                cmd.Parameters.Add("@FootprintID", SqlDbType.BigInt).Value = this.footprintId;
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 256).Value = this.Name;

                return (int)Context.ExecuteCommandScalar(cmd) > 0;
            }
        }

        protected override string GetTableQuery()
        {
            return @"
SELECT r.*, f.Name AS FootprintName, f.Owner, f.__acl
FROM [FootprintRegion] r
INNER JOIN [Footprint] f
    ON r.FootprintID = f.ID
";
        }

        protected override SqlCommand GetSelectCommand()
        {
            if (id == 0 && footprintId != 0 && name != null)
            {
                var sql = @"
WITH __e AS
(
{0}
)
SELECT * 
FROM __e
WHERE FootprintID = @FootprintID AND Name = @Name;
";

                var cmd = new SqlCommand(
                String.Format(
                    sql,
                    GetTableQuery()));

                cmd.Parameters.Add("@FootprintID", SqlDbType.Int).Value = footprintId;
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = name;

                return cmd;
            }
            else
            {
                return base.GetSelectCommand();
            }
        }

        protected override SqlCommand GetCheckExistsCommand()
        {
            if (id == 0 && footprintId != 0 && name != null)
            {
                var sql = @"
WITH __e AS
(
{0}
)
SELECT ISNULL(COUNT(*), 0)
FROM __e
WHERE FootprintID = @FootprintID AND Name = @Name;
";

                var cmd = new SqlCommand(
                String.Format(
                    sql,
                    GetTableQuery()));

                cmd.Parameters.Add("@FootprintId", SqlDbType.NVarChar).Value = footprintId;
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = name;

                return cmd;
            }
            else
            {
                return base.GetCheckExistsCommand();
            }
        }

        private void LoadRegion()
        {
            EvaluateAccess().EnsureRead();

            var sql = @"
SELECT region
FROM [FootprintRegion] r
WHERE r.ID = @ID
";

            using (var cmd = Context.CreateCommand(sql))
            {
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = this.id;

                using (var dr = Context.ExecuteCommandReader(cmd))
                {
                    dr.Read();

                    this.region = Spherical.Region.FromSqlBytes(dr.GetSqlBytes(0));
                }
            }
        }

        public void SaveRegion()
        {
            EvaluateAccess().EnsureUpdate();

            var sql = "[fps].[spSaveRegion]";

            using (var cmd = Context.CreateStoredProcedureCommand(sql))
            {
                cmd.Parameters.Add("@RegionID", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@region", SqlDbType.VarBinary).Value = region == null ? (object)DBNull.Value : region.ToSqlBytes();

                Context.ExecuteCommandNonQuery(cmd);
            }
        }

        public void LoadThumbnail()
        {

            EvaluateAccess().EnsureRead();

            var sql = @"
SELECT thumbnail
FROM [FootprintRegion] r
WHERE r.ID = @ID
";

            using (var cmd = Context.CreateCommand(sql))
            {
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = this.id;

                using (var dr = Context.ExecuteCommandReader(cmd))
                {
                    dr.Read();

                    this.thumbnail = dr["thumbnail"] == DBNull.Value ? null : (byte[])dr["thumbnail"];

                    if (thumbnail == null)
                    {
                        CreateThumbnail();
                    }
                }
            }
        }

        public void RefreshThumbnail()
        {
            // TO DO
        }

        public void CreateThumbnail()
        {

            // generate plot
            var p = new Spherical.Visualizer.Plot()
            {
                Width = 300,
                Height = 224,
                Projection = new Spherical.Visualizer.AitoffProjection(),
                AutoRotate = true,
                AutoZoom = true
            };

            if (region == null)
            {
                LoadRegion();
            }

            // add grid
            var grid = new GridLayer();
            grid.Line.Pen = Pens.LightGray;
            grid.DecScale.Density = 150f;
            p.Layers.Add(grid);

            // add region
            var regionds = new ObjectListDataSource(new[] { region });
            var r = new RegionsLayer();
            r.DataSource = regionds;
            p.Layers.Add(r);

            // add axes
            var axes = new AxesLayer();
            
            axes.X1Axis.Title.Text = "Right ascension (deg)";
            axes.X1Axis.Title.Visible = true;
            axes.X2Axis.Labels.Visible = false;
            axes.Y1Axis.Title.Text = "Declination (deg)";
            axes.Y2Axis.Labels.Visible = false;
            p.Layers.Add(axes);

            // create binary array
            var ms = new MemoryStream();
            p.RenderToBitmap(ms, ImageFormat.Png);
            thumbnail = ms.ToArray();

            // SQL here

            var sql = @"
UPDATE [dbo].[FootprintRegion] 
SET Thumbnail= @Thumbnail
WHERE Id = @Id";

            using (var cmd = new SqlCommand(sql))
            {
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@Thumbnail", SqlDbType.VarBinary).Value = thumbnail == null ? (object)DBNull.Value : thumbnail;

                Context.ExecuteCommandNonQuery(cmd);
            }
        }



        #endregion
    }
}
