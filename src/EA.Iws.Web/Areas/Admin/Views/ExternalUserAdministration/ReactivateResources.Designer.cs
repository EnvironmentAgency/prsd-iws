﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EA.Iws.Web.Areas.Admin.Views.ExternalUserAdministration {
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
    public class ReactivateResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ReactivateResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("EA.Iws.Web.Areas.Admin.Views.ExternalUserAdministration.ReactivateResources", typeof(ReactivateResources).Assembly);
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
        ///   Looks up a localized string similar to You are about to reactivate the user account for {0}..
        /// </summary>
        public static string Confirmation {
            get {
                return ResourceManager.GetString("Confirmation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Are you sure you want to reactivate the user account?.
        /// </summary>
        public static string ConfirmationTitle {
            get {
                return ResourceManager.GetString("ConfirmationTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reactivate.
        /// </summary>
        public static string ReactivateButton {
            get {
                return ResourceManager.GetString("ReactivateButton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reactivate external user.
        /// </summary>
        public static string ReactivateTitle {
            get {
                return ResourceManager.GetString("ReactivateTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to External user account reactivated.
        /// </summary>
        public static string SuccessTitle {
            get {
                return ResourceManager.GetString("SuccessTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The user account for {0} has been reactivated..
        /// </summary>
        public static string UserAccountDeactivated {
            get {
                return ResourceManager.GetString("UserAccountDeactivated", resourceCulture);
            }
        }
    }
}
