﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WordleBlazor.Resources {
    using System;
    
    
    /// <summary>
    ///   Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.
    /// </summary>
    // StronglyTypedResourceBuilder generó automáticamente esta clase
    // a través de una herramienta como ResGen o Visual Studio.
    // Para agregar o quitar un miembro, edite el archivo .ResX y, a continuación, vuelva a ejecutar ResGen
    // con la opción /str o recompile su proyecto de VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Localization {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Localization() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WordleBlazor.Resources.Localization", typeof(Localization).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Reemplaza la propiedad CurrentUICulture del subproceso actual para todas las
        ///   búsquedas de recursos mediante esta clase de recurso fuertemente tipado.
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
        ///   Busca una cadena traducida similar a GAME OVER.
        /// </summary>
        public static string GameboardLose {
            get {
                return ResourceManager.GetString("GameboardLose", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Next word in.
        /// </summary>
        public static string GameboardNextWordCountdown {
            get {
                return ResourceManager.GetString("GameboardNextWordCountdown", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a The word was.
        /// </summary>
        public static string GameboardTheWordWas {
            get {
                return ResourceManager.GetString("GameboardTheWordWas", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a YOU WIN.
        /// </summary>
        public static string GameboardWin {
            get {
                return ResourceManager.GetString("GameboardWin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Not enough letters.
        /// </summary>
        public static string GameManagerNotEnoughLetters {
            get {
                return ResourceManager.GetString("GameManagerNotEnoughLetters", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Word does not exist.
        /// </summary>
        public static string GameManagerWordDoesNotExist {
            get {
                return ResourceManager.GetString("GameManagerWordDoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Change language.
        /// </summary>
        public static string HeaderChangeLanguage {
            get {
                return ResourceManager.GetString("HeaderChangeLanguage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Show help.
        /// </summary>
        public static string HeaderShowHelp {
            get {
                return ResourceManager.GetString("HeaderShowHelp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Show statistics.
        /// </summary>
        public static string HeaderShowStatistics {
            get {
                return ResourceManager.GetString("HeaderShowStatistics", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a DEL.
        /// </summary>
        public static string KeyboardDel {
            get {
                return ResourceManager.GetString("KeyboardDel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a SEND.
        /// </summary>
        public static string KeyboardSend {
            get {
                return ResourceManager.GetString("KeyboardSend", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Best streak.
        /// </summary>
        public static string StatsBestStreak {
            get {
                return ResourceManager.GetString("StatsBestStreak", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Current streak.
        /// </summary>
        public static string StatsCurrentStreak {
            get {
                return ResourceManager.GetString("StatsCurrentStreak", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Games Played.
        /// </summary>
        public static string StatsGamesPlayed {
            get {
                return ResourceManager.GetString("StatsGamesPlayed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Games won %.
        /// </summary>
        public static string StatsGamesWonPercent {
            get {
                return ResourceManager.GetString("StatsGamesWonPercent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a RESULT DISTRIBUTION.
        /// </summary>
        public static string StatsResultDistribution {
            get {
                return ResourceManager.GetString("StatsResultDistribution", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a STATISTICS.
        /// </summary>
        public static string StatsTitle {
            get {
                return ResourceManager.GetString("StatsTitle", resourceCulture);
            }
        }
    }
}
