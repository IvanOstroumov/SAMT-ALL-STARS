using System;
using System.IO;

namespace Resources.Scripts
{
    public class LogManager
    {
        private static readonly string logDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SamtAllStars", "Logs");

        public static void Log(string msg)
        {
            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);

            string logPath = Path.Combine(logDirectory, $"log_{DateTime.Now:yyyy-MM-dd}.txt");

            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {msg}";

            File.AppendAllText(logPath, logMessage + Environment.NewLine);
        }
    }
}