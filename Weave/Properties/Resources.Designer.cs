﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Weave.Properties {
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
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Weave.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to The setting &apos;{0}&apos; has already been specified..
        /// </summary>
        internal static string WEAVE0001_SETTING_ALREADY_SPECIFIED {
            get {
                return ResourceManager.GetString("WEAVE0001_SETTING_ALREADY_SPECIFIED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The setting &apos;{0}&apos; is not recognized..
        /// </summary>
        internal static string WEAVE0002_SETTING_UNKNOWN {
            get {
                return ResourceManager.GetString("WEAVE0002_SETTING_UNKNOWN", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The value &apos;{0}&apos; is invalid for the &apos;{1}&apos; setting..
        /// </summary>
        internal static string WEAVE0003_SETTING_VALUE_INVALID {
            get {
                return ResourceManager.GetString("WEAVE0003_SETTING_VALUE_INVALID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No namespace has been specified..
        /// </summary>
        internal static string WEAVE0004_NAMESPACE_NOT_SPECIFIED {
            get {
                return ResourceManager.GetString("WEAVE0004_NAMESPACE_NOT_SPECIFIED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Encoding is used in the template, but no encode method has been specified..
        /// </summary>
        internal static string WEAVE0005_ENCODE_NOT_SPECIFIED {
            get {
                return ResourceManager.GetString("WEAVE0005_ENCODE_NOT_SPECIFIED", resourceCulture);
            }
        }
    }
}
