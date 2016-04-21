using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.ServiceModel;


namespace Jhu.Footprint.Web.Api.V1
{
    [DataContract(Name = "footprint")]
    [Description("Represents a footprint.")]
    public class FootprintResponse
    {
        [DataMember(Name = "footprint")]
        [Description("Footprint.")]
        public Footprint FootprintFolder { get; set; }

        [DataMember(Name = "footprintList")]
        [Description("List of fooptrint in folder.")]
        public Uri[] FootprintList { get; set; }

        public FootprintResponse(Footprint folder, Uri[] footprintList)
        {
            this.FootprintFolder = folder;
            this.FootprintList = footprintList;
        }
    }
}
