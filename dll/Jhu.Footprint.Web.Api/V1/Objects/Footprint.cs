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

        [DataMember(Name = "region")]
        [Description("Region string.")]
        public string Region { get; set; }

        public Footprint()
        {
        }

        public Footprint(Jhu.Footprint.Web.Lib.Footprint footprint)
        {
            this.FolderName = footprint.FolderName;
            this.Name = footprint.Name;
            //this.Region = footprint.Region.ToString();
        }
    }
}
