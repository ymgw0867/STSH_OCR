﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace STSH_OCR.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.4.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\\\192.168.1.51\\STSH_OCR\\DB\\STSH_OCR.db3")]
        public string DB_File {
            get {
                return ((string)(this["DB_File"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\192.168.1.51\\STSH_OCR\\MST\\syohinmst.csv")]
        public string 商品マスター {
            get {
                return ((string)(this["商品マスター"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\192.168.1.51\\STSH_OCR\\MST\\tokuisakimst.csv")]
        public string 得意先マスター {
            get {
                return ((string)(this["得意先マスター"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\192.168.1.51\\STSH_OCR\\MST\\syohinzaiko.csv")]
        public string 商品在庫マスター {
            get {
                return ((string)(this["商品在庫マスター"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\192.168.1.51\\STSH_OCR\\MST\\Shiiresaki.csv")]
        public string 仕入先マスター {
            get {
                return ((string)(this["仕入先マスター"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\192.168.1.51\\STSH_OCR\\XLS\\FAX注文書.xlsx")]
        public string FAX注文書 {
            get {
                return ((string)(this["FAX注文書"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\192.168.1.51\\STSH_OCR\\FaxRoot\\")]
        public string FaxRootDirectory {
            get {
                return ((string)(this["FaxRootDirectory"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\STSH_CLI\\DATA\\")]
        public string MyDataPath {
            get {
                return ((string)(this["MyDataPath"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\192.168.1.51\\STSH_OCR\\DATA\\")]
        public string DataPath {
            get {
                return ((string)(this["DataPath"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("PC1")]
        public string lockFileName {
            get {
                return ((string)(this["lockFileName"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\STSH_CLI\\DB\\STSH_CLI.db3")]
        public string Local_DB {
            get {
                return ((string)(this["Local_DB"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\192.168.1.51\\STSH_OCR\\Hold_Tiff\\")]
        public string HoldTifPath {
            get {
                return ((string)(this["HoldTifPath"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\192.168.1.51\\STSH_OCR\\TIF\\")]
        public string TifPath {
            get {
                return ((string)(this["TifPath"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\192.168.1.51\\STSH_OCR\\OCRERROR")]
        public string NgPath {
            get {
                return ((string)(this["NgPath"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\192.168.1.51\\STSH_OCR\\XLS\\ReFAX.xlsx")]
        public string ReFAXTempXlsx {
            get {
                return ((string)(this["ReFAXTempXlsx"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\192.168.1.51\\STSH_OCR\\XLS\\返信FAX.xlsx")]
        public string ReFaxSaveFile {
            get {
                return ((string)(this["ReFaxSaveFile"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("14")]
        public int orderDataBackupDay {
            get {
                return ((int)(this["orderDataBackupDay"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\192.168.1.51\\STSH_OCR\\MST\\商品分類リスト.xlsx")]
        public string 商品分類リスト {
            get {
                return ((string)(this["商品分類リスト"]));
            }
        }
    }
}
