﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EA.Iws.Web.Areas.NotificationMovements.Views.ReceiptRecovery {
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
    public class QuantityAbnormalResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal QuantityAbnormalResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("EA.Iws.Web.Areas.NotificationMovements.Views.ReceiptRecovery.QuantityAbnormalReso" +
                            "urces", typeof(QuantityAbnormalResources).Assembly);
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
        ///   Looks up a localized string similar to Please note: You entered {0} as the quantity of waste received, however that’s over 50% more than the actual quantity..
        /// </summary>
        public static string AboveTolerance {
            get {
                return ResourceManager.GetString("AboveTolerance", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please note: You entered {0} as the quantity of waste received, however that’s over 50% less than the actual quantity..
        /// </summary>
        public static string BelowTolerance {
            get {
                return ResourceManager.GetString("BelowTolerance", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Yes - I’ve entered the right quantity of waste received.
        /// </summary>
        public static string Confirm {
            get {
                return ResourceManager.GetString("Confirm", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Are you sure you&apos;ve entered the right quantity?.
        /// </summary>
        public static string MainHeading {
            get {
                return ResourceManager.GetString("MainHeading", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Quantity received in {0}.
        /// </summary>
        public static string QuantityLabel {
            get {
                return ResourceManager.GetString("QuantityLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No - I’d like to correct the quantity of waste received.
        /// </summary>
        public static string Reject {
            get {
                return ResourceManager.GetString("Reject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Quantity received.
        /// </summary>
        public static string Title {
            get {
                return ResourceManager.GetString("Title", resourceCulture);
            }
        }
    }
}