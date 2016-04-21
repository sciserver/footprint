using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ComponentModel;
using Util = Jhu.Graywulf.Web.Api.Util;
using Lib = Jhu.Footprint.Web.Lib;

namespace Jhu.Footprint.Web.Api.V1
{
    [DataContract(Name = "region")]
    [Description("Represents a celestial region.")]
    public class FootprintRegion
    {
        [DataMember(Name = "id")]
        [Description("Region Id.")]
        public int Id { get; set; }

        [DataMember(Name = "url")]
        [Description("Region url.")]
        public Uri Url { get; set; }

        [DataMember(Name = "footprintId")]
        [Description("Id of the footprint containing the region.")]
        public int FootprintId { get; set; }

        [DataMember(Name = "footprintUrl")]
        [Description("Footprint url.")]
        public Uri FootprintUrl { get; set; }

        [DataMember(Name = "footprintName")]
        [Description("Name of the footprint containing the region.")]
        public string FolderName { get; set; }

        [DataMember(Name = "name")]
        [Description("Name of the region.")]
        public string Name { get; set; }

        [DataMember(Name = "owner")]
        [Description("Owner of the footprint.")]
        public string Owner { get; set; }

        [DataMember(Name = "fillFactor")]
        [Description("Fill factor of the region.")]
        public double FillFactor { get; set; }

        [DataMember(Name = "regionString")]
        [Description("Region string.")]
        public string RegionString { get; set; }

        public FootprintRegion()
        {
        }

        public FootprintRegion(Jhu.Footprint.Web.Lib.FootprintRegion region, string folderName)
        {
            SetValue(region, folderName);
        }

        public void SetValue(Jhu.Footprint.Web.Lib.FootprintRegion region, string folderName)
        {
            this.Id = region.Id;

            /*
            this.FolderName =  folderName;
            this.Name = region.Name;
            this.User = region.User;
            
            this.Public = region.Public;
            this.FillFactor = region.FillFactor;
            this.FootprintType = region.Type;
            this.Comment = region.Comment;
            this.FootprintId = region.FolderId;
            //TODO : host name?
            this.Url = new Uri("http://" + Environment.MachineName + "/footprint/api/v1/Footprint.svc/users/"+this.User+"/"+this.FolderName+"/"+this.Name);
            if (region.Region != null)
            {
                this.RegionString = region.Region.ToString();
            }
            else this.RegionString = null;
             * */
        }

        public Jhu.Footprint.Web.Lib.FootprintRegion GetValue()
        {
            var region = new Jhu.Footprint.Web.Lib.FootprintRegion();

            /*
            region.Name = this.Name;
            region.User = this.User;
            region.Id = this.Id;
            region.Public = this.Public;
            region.FillFactor = this.FillFactor;
            region.Type = this.FootprintType;
            region.Comment = this.Comment;
            region.FolderId = this.FootprintId;
            if (this.RegionString != null)
            {
                region.Region = Jhu.Spherical.Region.Parse(this.RegionString);
            }
            else region.Region = null;
             * */

            return region;
        }
    }
}
