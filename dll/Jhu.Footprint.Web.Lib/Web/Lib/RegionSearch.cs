using System.Data;
using Jhu.Graywulf.Entities.Mapping;
using Jhu.Spherical;

namespace Jhu.Footprint.Web.Lib
{
    public abstract class RegionSearch<T> : Graywulf.Entities.SecurableEntitySearch<T>, IRegionSearch
        where T : Graywulf.Entities.Entity, new()
    {
        protected string owner;
        protected string name;
        protected SearchMethod searchMethod;
        protected Cartesian point;
        protected double radius;
        protected Region region;

        [DbColumn]
        public string Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        [DbColumn]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public SearchMethod SearchMethod
        {
            get { return searchMethod; }
            set { searchMethod = value; }
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

        #region Constructors & initializers
        public RegionSearch()
            : base()
        {
        }

        public RegionSearch(FootprintContext context)
                : base(context)
        {
            InitializeMembers();
        }
        #endregion
        private void InitializeMembers()
        {

            name = null;
            owner = null;
            searchMethod = SearchMethod.Name;
            point = new Cartesian();
            radius = 0;
            region = null;
        }

        protected override string GetTableQuery()
        {
            switch (searchMethod)
            {

                case SearchMethod.Point:
                    return GetTableQuery_PointSearch();
                case SearchMethod.Intersect:
                    return GetTableQuery_IntersectSearch();
                case SearchMethod.Contain:
                    return GetTableQuery_ContainSearch();
                case SearchMethod.Cover:
                    return GetTableQuery_CoverSearch();
                default:
                    return base.GetTableQuery();
            }
        }

        protected abstract string GetTableQuery_PointSearch();

        protected abstract string GetTableQuery_IntersectSearch();

        protected abstract string GetTableQuery_ContainSearch();

        protected abstract string GetTableQuery_CoverSearch();

        protected override void AppendParameters()
        {
            switch (searchMethod)
            {
                case SearchMethod.Point:
                    AppendParameters_PointSearch();
                    break;
                case SearchMethod.Contain:
                case SearchMethod.Intersect:
                case SearchMethod.Cover:
                    AppendParameters_RegionSearch();
                    break;
                default:
                    base.AppendParameters();
                    break;
            }
        }

        private void AppendParameters_PointSearch()
        {
            AppendSearchParameter("@ra", SqlDbType.Float, Point.RA);
            AppendSearchParameter("@dec", SqlDbType.Float, Point.Dec);
        }

        private void AppendParameters_RegionSearch()
        {
            AppendSearchParameter("@region", SqlDbType.VarBinary, Region.ToSqlBytes().Value);
        }


    }
}


