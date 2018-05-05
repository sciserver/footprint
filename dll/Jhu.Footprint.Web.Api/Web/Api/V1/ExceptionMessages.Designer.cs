﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Jhu.Footprint.Web.Api.V1 {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ExceptionMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ExceptionMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Jhu.Footprint.Web.Api.V1.ExceptionMessages", typeof(ExceptionMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot parse angle {0}..
        /// </summary>
        internal static string CannotParseAngle {
            get {
                return ResourceManager.GetString("CannotParseAngle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot parse coordinates: {0}..
        /// </summary>
        internal static string CannotParseCoordinates {
            get {
                return ResourceManager.GetString("CannotParseCoordinates", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid angle. Must specify either degrees (decimal or DMS), radians, arc minutes or arc seconds..
        /// </summary>
        internal static string InvalidAngle {
            get {
                return ResourceManager.GetString("InvalidAngle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid coordinates. Must specify either (RA, Dec - in degrees or HMS:DMS), (Lon, Lat - in degrees) or (Cx, Cy, Cz)..
        /// </summary>
        internal static string InvalidCoordinates {
            get {
                return ResourceManager.GetString("InvalidCoordinates", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Multiple region specifications found. Specify only one region per request..
        /// </summary>
        internal static string MultipleRegionsSpecified {
            get {
                return ResourceManager.GetString("MultipleRegionsSpecified", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No region specified in the request..
        /// </summary>
        internal static string NoRegionSpecified {
            get {
                return ResourceManager.GetString("NoRegionSpecified", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Only one representation of the angle must be specified. Must specify either degrees (decimal or DMS), radians, arc minutes or arc seconds..
        /// </summary>
        internal static string OneAngleRepresentationRequired {
            get {
                return ResourceManager.GetString("OneAngleRepresentationRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Only one set of coordinates must be specified. Must specify either (RA, Dec - in degrees or HMS:DMS), (Lon, Lat - in degrees) or (Cx, Cy, Cz)..
        /// </summary>
        internal static string OneCoordinateRepresentationRequired {
            get {
                return ResourceManager.GetString("OneCoordinateRepresentationRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No selections defined in request..
        /// </summary>
        internal static string SelectionNotDefined {
            get {
                return ResourceManager.GetString("SelectionNotDefined", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Too few selections are specified for the operation. At least {0} required..
        /// </summary>
        internal static string SelectionTooFew {
            get {
                return ResourceManager.GetString("SelectionTooFew", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Too many selection are specified for the operation. At most {0} requied..
        /// </summary>
        internal static string SelectionTooMany {
            get {
                return ResourceManager.GetString("SelectionTooMany", resourceCulture);
            }
        }
    }
}