﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PosTech.Contacts.ApplicationCore.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Messages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PosTech.Contacts.ApplicationCore.Resources.Messages", typeof(Messages).Assembly);
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
        ///   Looks up a localized string similar to O DDD informado deve ser válido.
        /// </summary>
        public static string DddInvalid {
            get {
                return ResourceManager.GetString("DddInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to O e-mail informado deve ser válido.
        /// </summary>
        public static string EmailInvalid {
            get {
                return ResourceManager.GetString("EmailInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to É necessário informar um endereço de e-mail.
        /// </summary>
        public static string EmailRequired {
            get {
                return ResourceManager.GetString("EmailRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Necessário informar um nome com pelo menos 2 caracteres.
        /// </summary>
        public static string MinLengthForName {
            get {
                return ResourceManager.GetString("MinLengthForName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Necessário informar um sobrenome com pelo menos 5 caracteres.
        /// </summary>
        public static string MinLengthForSurname {
            get {
                return ResourceManager.GetString("MinLengthForSurname", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to É necessário informar um nome.
        /// </summary>
        public static string NameRequired {
            get {
                return ResourceManager.GetString("NameRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to O telefone informado é inválido. Formatos válidos 912345678 (celular) ou 23456789 (fixo).
        /// </summary>
        public static string PhoneInvalid {
            get {
                return ResourceManager.GetString("PhoneInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to É necessário informar um telefone.
        /// </summary>
        public static string PhoneRequired {
            get {
                return ResourceManager.GetString("PhoneRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to É necessário informar um sobrenome.
        /// </summary>
        public static string SurnameRequired {
            get {
                return ResourceManager.GetString("SurnameRequired", resourceCulture);
            }
        }
    }
}
