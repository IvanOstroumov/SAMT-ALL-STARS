using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Resources.Scripts
{
    // Logger del gioco. Scrive su file in %LOCALAPPDATA%/SamtAllStars/Logs/.
    public static class LogManager
    {
        public enum Level { Info, Error }
        
        private static readonly string logDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "SamtAllStars", "Logs");
        
        private const int MAX_LOG_FILES = 20;
        
        private static string sessionLogPath;
        
        private static readonly object writeLock = new object();

        private static bool initialized;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            if (initialized) return;
            initialized = true;

            try
            {
                if (!Directory.Exists(logDirectory))
                    Directory.CreateDirectory(logDirectory);
                
                string stamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                sessionLogPath = Path.Combine(logDirectory, $"log_{stamp}.txt");

                CleanOldLogs();
                
                Application.logMessageReceivedThreaded += OnUnityLog;

                Info($"=== Nuova sessione - {DateTime.Now} ===");
                Info($"Unity {Application.unityVersion}, piattaforma {Application.platform}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[LogManager] Bootstrap fallito: {e}");
            }
        }
        
        public static void Info(string msg) => Write(Level.Info, msg);
        
        public static void Error(string msg) => Write(Level.Error, msg + "\n" + Environment.StackTrace);
        
        public static void Error(string msg, Exception ex) =>
            Write(Level.Error, $"{msg}\n{ex}");
        

        private static void Write(Level level, string msg)
        {
            if (string.IsNullOrEmpty(sessionLogPath)) return;

            string line = $"{DateTime.Now:HH:mm:ss.fff} [{level}] {msg}";
            
            lock (writeLock)
            {
                try
                {
                    File.AppendAllText(sessionLogPath, line + Environment.NewLine);
                }
                catch
                {

                }
            }
        }


        private static void OnUnityLog(string condition, string stackTrace, LogType type)
        {
            if (type != LogType.Error && type != LogType.Exception && type != LogType.Assert)
                return;

            Write(Level.Error, $"[Unity:{type}] {condition}\n{stackTrace}");
        }
        
        private static void CleanOldLogs()
        {
            try
            {
                FileInfo[] files = new DirectoryInfo(logDirectory)
                    .GetFiles("log_*.txt")
                    .OrderByDescending(f => f.CreationTimeUtc)
                    .ToArray();

                for (int i = MAX_LOG_FILES; i < files.Length; i++)
                {
                    try { files[i].Delete(); }
                    catch { /* ne riproveremo a cancellare al prossimo avvio */ }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[LogManager] Pulizia vecchi log fallita: {e.Message}");
            }
        }
    }
}
