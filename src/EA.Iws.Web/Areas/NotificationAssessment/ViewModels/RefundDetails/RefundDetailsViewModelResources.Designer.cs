﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EA.Iws.Web.Areas.NotificationAssessment.ViewModels.RefundDetails {
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
    public class RefundDetailsViewModelResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal RefundDetailsViewModelResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("EA.Iws.Web.Areas.NotificationAssessment.ViewModels.RefundDetails.RefundDetailsVie" +
                            "wModelResources", typeof(RefundDetailsViewModelResources).Assembly);
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
        ///   Looks up a localized string similar to The amount entered cannot be negative.
        /// </summary>
        public static string AmountCannotBeNegative {
            get {
                return ResourceManager.GetString("AmountCannotBeNegative", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter a valid number with a maximum of {0} decimal places.
        /// </summary>
        public static string AmountDecimalPlaceError {
            get {
                return ResourceManager.GetString("AmountDecimalPlaceError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter the amount to refund.
        /// </summary>
        public static string AmountRefundedError {
            get {
                return ResourceManager.GetString("AmountRefundedError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Amount to refund (£).
        /// </summary>
        public static string AmountRefundedLabel {
            get {
                return ResourceManager.GetString("AmountRefundedLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Comments.
        /// </summary>
        public static string CommentsLabel {
            get {
                return ResourceManager.GetString("CommentsLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The maximum length of the comments is 500 characters.
        /// </summary>
        public static string CommentsLengthError {
            get {
                return ResourceManager.GetString("CommentsLengthError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter a valid number in the &apos;Day&apos; field.
        /// </summary>
        public static string DayError {
            get {
                return ResourceManager.GetString("DayError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Day.
        /// </summary>
        public static string DayLabel {
            get {
                return ResourceManager.GetString("DayLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter a valid number in the &apos;Month&apos; field.
        /// </summary>
        public static string MonthError {
            get {
                return ResourceManager.GetString("MonthError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Month.
        /// </summary>
        public static string MonthLabel {
            get {
                return ResourceManager.GetString("MonthLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter a valid number in the &apos;Year&apos; field.
        /// </summary>
        public static string YearError {
            get {
                return ResourceManager.GetString("YearError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Year.
        /// </summary>
        public static string YearLabel {
            get {
                return ResourceManager.GetString("YearLabel", resourceCulture);
            }
        }
    }
}
