﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace RebalancedIndustriesRevisited {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Localize {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Localize() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("RebalancedIndustriesRevisited.Localization.Localize", typeof(Localize).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   重写当前线程的 CurrentUICulture 属性，对
        ///   使用此强类型资源类的所有资源查找执行重写。
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
        ///   查找类似 Rebalances Industries DLC, reduce traffic flow, increase cargo loading and more. 的本地化字符串。
        /// </summary>
        public static string MOD_Description {
            get {
                return ResourceManager.GetString("MOD_Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 [FIX]Fixed incorrect display of vehicle count. 的本地化字符串。
        /// </summary>
        public static string UpdateLog_V0_1_0FIX {
            get {
                return ResourceManager.GetString("UpdateLog_V0_1_0FIX", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 [UPT]Using new algorithms to rebalances Industries. 的本地化字符串。
        /// </summary>
        public static string UpdateLog_V0_1_0UPT1 {
            get {
                return ResourceManager.GetString("UpdateLog_V0_1_0UPT1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 [UPT]Using the latest Harmony API. 的本地化字符串。
        /// </summary>
        public static string UpdateLog_V0_1_0UPT2 {
            get {
                return ResourceManager.GetString("UpdateLog_V0_1_0UPT2", resourceCulture);
            }
        }
    }
}
