using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Jhu.Footprint.Web.Api.V1
{
    [DataContract(Name = "footprint")]
    [Description("TODO")]
    public class Footprint
    {
        [DataMember(Name = "folderName")]
        [Description("Name of the folder containing the footprint.")]
        public string FolderName { get; set; }

        [DataMember(Name = "name")]
        [Description("Name of the footprint.")]
        public string Name { get; set; }

        [DataMember(Name = "user")]
        [Description("User of the footprint.")]
        public string User { get; set; }

        [DataMember(Name = "regionString")]
        [Description("Region string.")]
        public string RegionString { get; set; }

        public Footprint()
        {
        }

        public Footprint(Jhu.Footprint.Web.Lib.Footprint footprint)
        {
            SetValue(footprint);
        }

        public void SetValue(Jhu.Footprint.Web.Lib.Footprint footprint)
        {
            this.FolderName = footprint.FolderName;
            this.Name = footprint.Name;
            this.User = footprint.User;
            //this.Region = footprint.Region.ToString();
        }

        public Jhu.Footprint.Web.Lib.Footprint GetValue()
        {
            var footprint = new Jhu.Footprint.Web.Lib.Footprint();

            footprint.Name = this.Name;
            footprint.FolderName = this.FolderName;
            footprint.User = this.User;
            //footprint.Region = Jhu.Spherical.Region.Parse(this.RegionString);

            return footprint;
        }
    }
}
