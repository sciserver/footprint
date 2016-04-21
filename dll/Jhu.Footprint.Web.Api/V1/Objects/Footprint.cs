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
    [DataContract(Name = "footprint")]
    [Description("A footprint is a collection of regions representing the sky coverage of observations.")]
    public class Footprint
    {
        [DataMember(Name = "id")]
        [Description("Footprint Id.")]
        public int Id { get; set; }

        [DataMember(Name = "url")]
        [Description("Footprint url.")]
        public Uri Url { get; set; }

        [DataMember(Name = "name")]
        [Description("Name of the folder containing the footprint.")]
        public string Name { get; set; }

        [DataMember(Name = "owner")]
        [Description("Owner of the footprint.")]
        public string Owner { get; set; }

        [IgnoreDataMember]
        public Jhu.Footprint.Web.Lib.FootprintType Type { get; set; }

        [DataMember(Name = "type")]
        [Description("Method to combine regions: union, intersection or none.")]
        public string Type_ForXml
        {
            get { return Util.EnumFormatter.ToXmlString(Type); }
            set { Type = Util.EnumFormatter.FromXmlString<Lib.FootprintType>(value); }
        }

        [DataMember(Name = "comments")]
        [Description("Comments.")]
        public string Comments { get; set; }

        [DataMember(Name = "public")]
        [DefaultValue(false)]
        [Description("Visibility of the footprint to the public.")]
        public bool Public { get; set; }

        public Footprint()
        {
        }

        public Footprint(Jhu.Footprint.Web.Lib.Footprint footprint)
        {
            SetValue(footprint);
        }

        public void SetValue(Jhu.Footprint.Web.Lib.Footprint footprint)
        {
            this.Id = footprint.Id;
            this.Name = footprint.Name;
            this.Owner = footprint.Owner;
            this.Type = footprint.Type;
            this.Comments = footprint.Comments;

            //TODO : host name?
            //this.Url = new Uri("http://" + Environment.MachineName + "/footprint/api/v1/Footprint.svc/users/" + this.User + "/" + this.Name);
        }

        public Jhu.Footprint.Web.Lib.Footprint GetValue()
        {
            var footprint =  new Jhu.Footprint.Web.Lib.Footprint()
            {
                Id = this.Id,
                Name = this.Name,
                Owner = this.Owner,
                Type = this.Type,
                Comments = this.Comments
            };

            // TODO: public/private?

            return footprint;
        }
    }
}
