﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ComponentModel;
using Jhu.Graywulf.AccessControl;
using Jhu.Graywulf.Entities;
using Util = Jhu.Graywulf.Web.Api.Util;
using Lib = Jhu.Footprint.Web.Lib;

namespace Jhu.Footprint.Web.Api.V1
{
    [DataContract(Name = "footprint")]
    [Description("A footprint is a collection of regions representing the sky coverage of observations.")]
    public class Footprint
    {
        [DataMember(Name = "owner")]
        [Description("Owner of the footprint.")]
        public string Owner { get; set; }

        [DataMember(Name = "name")]
        [Description("Name of the folder containing the footprint.")]
        public string Name { get; set; }

        [DataMember(Name = "url")]
        [Description("Footprint url.")]
        public Uri Url { get; set; }

        [IgnoreDataMember]
        public Jhu.Footprint.Web.Lib.FootprintType? Type { get; set; }

        [DataMember(Name = "type")]
        [DefaultValue("None")]
        [Description("Method to combine regions: union, intersection or none.")]
        public string Type_ForXml
        {
            get { return Util.EnumFormatter.ToNullableXmlString(Type); }
            set { Type = Util.EnumFormatter.FromNullableXmlString<Lib.FootprintType>(value); }
        }

        [DataMember(Name = "comments")]
        [DefaultValue("")]
        [Description("Comments.")]
        public string Comments { get; set; }

        [DataMember(Name = "public")]
        [Description("Visibility of the footprint to the public.")]
        public bool? Public { get; set; }

        public Footprint()
        {
        }

        public Footprint(Lib.Footprint footprint)
        {
            SetValues(footprint);
        }

        public void GetValues(Lib.Footprint footprint)
        {
            footprint.Type = this.Type ?? Lib.FootprintType.None;
            footprint.Comments = this.Comments ?? "";
            
            // To prevent resetting permission when modifying a footprint,
            // only set permission when the public field is present
            // in the request
            if (!footprint.IsExisting || Public.HasValue)
            {
                footprint.SetDefaultPermissions(Public ?? false);
            }
        }

        public void SetValues(Lib.Footprint footprint)
        {
            var access = footprint.Permissions.EvaluateAccess(Principal.Guest);

            this.Name = footprint.Name;
            this.Owner = footprint.Owner;
            this.Type = footprint.Type;
            this.Comments = footprint.Comments;
            this.Public = access.CanRead();
            this.Url = FootprintService.GetUrl(footprint);
        }

    }
}
