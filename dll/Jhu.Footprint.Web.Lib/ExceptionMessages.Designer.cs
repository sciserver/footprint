﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Jhu.Footprint.Web.Lib {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Jhu.Footprint.Web.Lib.ExceptionMessages", typeof(ExceptionMessages).Assembly);
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
        ///   Looks up a localized string similar to Access denied..
        /// </summary>
        internal static string AccessDenied {
            get {
                return ResourceManager.GetString("AccessDenied", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot find the specified footprint: &apos;{0}/{1}/{2}&apos;..
        /// </summary>
        internal static string CannotFindFootprint {
            get {
                return ResourceManager.GetString("CannotFindFootprint", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot find the specified footprint folder: &apos;{0}/{1}&apos;..
        /// </summary>
        internal static string CannotFindFootprintFolder {
            get {
                return ResourceManager.GetString("CannotFindFootprintFolder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Duplicate footprint folder name:  &apos;{0}&apos;..
        /// </summary>
        internal static string DuplicateFootprintFolderName {
            get {
                return ResourceManager.GetString("DuplicateFootprintFolderName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Duplicate footprint name: &apos;{0}&apos;..
        /// </summary>
        internal static string DuplicateFootprintName {
            get {
                return ResourceManager.GetString("DuplicateFootprintName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid footprint name &apos;{0}&apos;..
        /// </summary>
        internal static string FootprintNameInvalid {
            get {
                return ResourceManager.GetString("FootprintNameInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot use &apos;{0}&apos; as name of the footprint. The following strings are probhited: convexhull, footprint, outline, plot, points, reduced..
        /// </summary>
        internal static string FootprintNameNotAvailable {
            get {
                return ResourceManager.GetString("FootprintNameNotAvailable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot load specified footprint..
        /// </summary>
        internal static string NoFootprintDataToLoad {
            get {
                return ResourceManager.GetString("NoFootprintDataToLoad", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot load specified footprint folder..
        /// </summary>
        internal static string NoFootprintFolderDataToLoad {
            get {
                return ResourceManager.GetString("NoFootprintFolderDataToLoad", resourceCulture);
            }
        }
    }
}
