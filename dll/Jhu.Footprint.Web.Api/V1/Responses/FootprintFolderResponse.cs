using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.ServiceModel;


namespace Jhu.Footprint.Web.Api.V1.Responses
{
    [DataContract(Name = "footprintList")]
    [Description("Represents a list of footprints.")]
    class FootprintFolderResponse
    {
        [DataMember(Name = "footprintfolder")]
        [Description("Footprint folder.")]
        public FootprintFolder Footprints { get; set; }

        public FootprintFolderResponse(FootprintFolder folder)
        {
            this.Footprints = folder;
        }
    }
}
