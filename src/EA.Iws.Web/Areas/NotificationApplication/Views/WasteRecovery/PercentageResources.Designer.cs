﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EA.Iws.Web.Areas.NotificationApplication.Views.WasteRecovery {
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
    public class PercentageResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal PercentageResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("EA.Iws.Web.Areas.NotificationApplication.Views.WasteRecovery.PercentageResources", typeof(PercentageResources).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to What is the amount of recovered material in relation to the non-recoverable waste?.
        /// </summary>
        public static string Header {
            get {
                return ResourceManager.GetString("Header", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The percentage (%) must be between 0 and 100.
        /// </summary>
        public static string PercentageRange {
            get {
                return ResourceManager.GetString("PercentageRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter this amount as a percentage.
        /// </summary>
        public static string PercentageRecoverable {
            get {
                return ResourceManager.GetString("PercentageRecoverable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The percentage (%) of recoverable material must be a number with a maximum of 2 decimal places. Please ensure your number doesn&apos;t include any special characters like %, &lt; or £..
        /// </summary>
        public static string PercentageRecoverableValid {
            get {
                return ResourceManager.GetString("PercentageRecoverableValid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Percentage Recoverable.
        /// </summary>
        public static string Title {
            get {
                return ResourceManager.GetString("Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter a value.
        /// </summary>
        public static string ValueRequired {
            get {
                return ResourceManager.GetString("ValueRequired", resourceCulture);
            }
        }
    }
}
