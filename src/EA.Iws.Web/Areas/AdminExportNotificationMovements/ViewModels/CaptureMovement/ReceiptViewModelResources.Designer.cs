﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.CaptureMovement {
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
    public class ReceiptViewModelResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ReceiptViewModelResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.CaptureMovement.Rece" +
                            "iptViewModelResources", typeof(ReceiptViewModelResources).Assembly);
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
        ///   Looks up a localized string similar to Quantity received.
        /// </summary>
        public static string ActualQuantityLabel {
            get {
                return ResourceManager.GetString("ActualQuantityLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter a smaller number .
        /// </summary>
        public static string MaximumActualQuantity {
            get {
                return ResourceManager.GetString("MaximumActualQuantity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please provide the actual quantity.
        /// </summary>
        public static string QuantityRequired {
            get {
                return ResourceManager.GetString("QuantityRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to When was the waste received?.
        /// </summary>
        public static string ReceivedDateLabel {
            get {
                return ResourceManager.GetString("ReceivedDateLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please provide the received date.
        /// </summary>
        public static string ReceivedDateRequired {
            get {
                return ResourceManager.GetString("ReceivedDateRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reason why the shipment was rejected and any relevant details.
        /// </summary>
        public static string RejectionFurtherInformationLabel {
            get {
                return ResourceManager.GetString("RejectionFurtherInformationLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Why was it rejected?.
        /// </summary>
        public static string RejectionReasonLabel {
            get {
                return ResourceManager.GetString("RejectionReasonLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please provide a reason for rejection.
        /// </summary>
        public static string RejectReasonRequired {
            get {
                return ResourceManager.GetString("RejectReasonRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please select the quantity units.
        /// </summary>
        public static string UnitsRequired {
            get {
                return ResourceManager.GetString("UnitsRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Was the shipment accepted?.
        /// </summary>
        public static string WasShipmentAcceptedLabel {
            get {
                return ResourceManager.GetString("WasShipmentAcceptedLabel", resourceCulture);
            }
        }
    }
}
