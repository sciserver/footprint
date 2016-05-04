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
    [DataContract(Name = "footprintList")]
    [Description("Represents a list of footprints.")]
    public class FootprintFolderResponse
    {
        [DataMember(Name = "footprintfolder")]
        [Description("Footprint folder.")]
        public FootprintFolder FootprintFolder { get; set; }

        [DataMember(Name = "footprintNameList")]
        [Description("List of fooptrint in folder.")]
        public string[] FootprintNameList { get; set; }

        [DataMember(Name = "footprintUriList")]
        [Description("List of fooptrint in folder.")]
        public Uri[] FootprintUriList { get; set; }

        public FootprintFolderResponse(FootprintFolder folder, Uri[] footprintUriList, string[] footprintNameList)
        {
            this.FootprintFolder = folder;
            this.FootprintUriList = footprintUriList;
            this.FootprintNameList = footprintNameList;
        }
    }
}
