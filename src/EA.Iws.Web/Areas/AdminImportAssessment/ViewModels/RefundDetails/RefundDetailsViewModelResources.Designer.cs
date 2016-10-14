﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EA.Iws.Web.Areas.AdminImportAssessment.ViewModels.RefundDetails {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("EA.Iws.Web.Areas.AdminImportAssessment.ViewModels.RefundDetails.RefundDetailsView" +
                            "ModelResources", typeof(RefundDetailsViewModelResources).Assembly);
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
        ///   Looks up a localized string similar to Please do not exceed the refund limit of £{0}.
        /// </summary>
        public static string AmountCannotExceedLimit {
            get {
                return ResourceManager.GetString("AmountCannotExceedLimit", resourceCulture);
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
        ///   Looks up a localized string similar to Date refunded.
        /// </summary>
        public static string Date {
            get {
                return ResourceManager.GetString("Date", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The refund date cannot be before the payment received date.
        /// </summary>
        public static string DateNotBeforePaymentReceived {
            get {
                return ResourceManager.GetString("DateNotBeforePaymentReceived", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The refund date cannot be in the future.
        /// </summary>
        public static string DateNotInFuture {
            get {
                return ResourceManager.GetString("DateNotInFuture", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please provide the date the refund was made.
        /// </summary>
        public static string DateRequiredError {
            get {
                return ResourceManager.GetString("DateRequiredError", resourceCulture);
            }
        }
    }
}