﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EA.Iws.Web.Areas.AdminImportAssessment.ViewModels.Decision {
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
    public class DecisionViewModelResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal DecisionViewModelResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("EA.Iws.Web.Areas.AdminImportAssessment.ViewModels.Decision.DecisionViewModelResou" +
                            "rces", typeof(DecisionViewModelResources).Assembly);
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
        ///   Looks up a localized string similar to Conditions of consent.
        /// </summary>
        public static string ConsentConditions {
            get {
                return ResourceManager.GetString("ConsentConditions", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Decision date.
        /// </summary>
        public static string ConsentGiven {
            get {
                return ResourceManager.GetString("ConsentGiven", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please provide the date of decision.
        /// </summary>
        public static string ConsentGivenRequired {
            get {
                return ResourceManager.GetString("ConsentGivenRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Valid from.
        /// </summary>
        public static string ConsentValidFrom {
            get {
                return ResourceManager.GetString("ConsentValidFrom", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please ensure consent valid from is before consent valid to.
        /// </summary>
        public static string ConsentValidFromBeforeTo {
            get {
                return ResourceManager.GetString("ConsentValidFromBeforeTo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please provide the date consent is valid from.
        /// </summary>
        public static string ConsentValidFromRequired {
            get {
                return ResourceManager.GetString("ConsentValidFromRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Valid until.
        /// </summary>
        public static string ConsentValidTo {
            get {
                return ResourceManager.GetString("ConsentValidTo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please provide the date consent is valid until.
        /// </summary>
        public static string ConsentValidToRequired {
            get {
                return ResourceManager.GetString("ConsentValidToRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Decision.
        /// </summary>
        public static string Decision {
            get {
                return ResourceManager.GetString("Decision", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please provide a notification decision.
        /// </summary>
        public static string DecisionRequired {
            get {
                return ResourceManager.GetString("DecisionRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Date.
        /// </summary>
        public static string ObjectedDateLabel {
            get {
                return ResourceManager.GetString("ObjectedDateLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter the date the objection was made.
        /// </summary>
        public static string ObjectedDateRequired {
            get {
                return ResourceManager.GetString("ObjectedDateRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reasons for withdrawal of consent.
        /// </summary>
        public static string ReasonConsentWithdrawalLabel {
            get {
                return ResourceManager.GetString("ReasonConsentWithdrawalLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The reason consent is being withdrawn is required.
        /// </summary>
        public static string ReasonConsentWithdrawnRequired {
            get {
                return ResourceManager.GetString("ReasonConsentWithdrawnRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reasons for objection.
        /// </summary>
        public static string ReasonObjectedLabel {
            get {
                return ResourceManager.GetString("ReasonObjectedLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The reason for objection is required.
        /// </summary>
        public static string ReasonObjectedRequired {
            get {
                return ResourceManager.GetString("ReasonObjectedRequired", resourceCulture);
            }
        }
    }
}
