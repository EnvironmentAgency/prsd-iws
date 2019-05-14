﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.Controllers {
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
    public class CancelControllerResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CancelControllerResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("EA.Iws.Web.Areas.AdminImportNotificationMovements.Controllers.CancelControllerRes" +
                            "ources", typeof(CancelControllerResources).Assembly);
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
        ///   Looks up a localized string similar to Disposed.
        /// </summary>
        public static string Disposed {
            get {
                return ResourceManager.GetString("Disposed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This Shipment number already exists in the table below and will be added to the list of shipments that will be cancelled..
        /// </summary>
        public static string DuplicateShipmentNumber {
            get {
                return ResourceManager.GetString("DuplicateShipmentNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You cannot add more than {0} extra records at a time. If more are needed to be added, please carry out this process a further time after confirmation as taken place..
        /// </summary>
        public static string ExceedShipmentLimit {
            get {
                return ResourceManager.GetString("ExceedShipmentLimit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This Shipment number already exists and is shown on the previous screen. Please tick the shipment number on that screen..
        /// </summary>
        public static string IsCancellableExistingShipment {
            get {
                return ResourceManager.GetString("IsCancellableExistingShipment", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This Shipment number already exists but the status is {0}. Seek further advice of how to proceed with the data team leader..
        /// </summary>
        public static string IsNonCancellableExistingShipment {
            get {
                return ResourceManager.GetString("IsNonCancellableExistingShipment", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Recovered.
        /// </summary>
        public static string Recovered {
            get {
                return ResourceManager.GetString("Recovered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to NewShipmentNumber.
        /// </summary>
        public static string ShipmentNumberField {
            get {
                return ResourceManager.GetString("ShipmentNumberField", resourceCulture);
            }
        }
    }
}
