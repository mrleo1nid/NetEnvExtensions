using System.IO;
using System.Linq;
using DotNetEnv;

namespace NetEnvExtensions
{
    public static class EnvironmentLoader
    {
        /// <summary>
        /// Загрузить переменные окружения из .env файла
        /// </summary>
        public static void LoadEnvironmentVariables()
        {
            var envPath = FindEnvFile();
            if (!string.IsNullOrEmpty(envPath) && File.Exists(envPath))
            {
                Env.Load(envPath);
            }
        }

        /// <summary>
        /// Найти .env файл в различных локациях
        /// </summary>
        private static string? FindEnvFile()
        {
            // Try different locations for .env file
            var currentDir = Directory.GetCurrentDirectory();

            // 1. Current directory (when running from project root)
            var envPath = Path.Combine(currentDir, ".env");
            if (File.Exists(envPath))
                return envPath;

            // 2. Parent directory (when running from API project)
            envPath = Path.Combine(currentDir, "..", ".env");
            if (File.Exists(envPath))
                return envPath;

            // 3. Look for solution file and use its directory
            var dir = new DirectoryInfo(currentDir);
            while (dir != null)
            {
                if (dir.GetFiles("*.sln").Any())
                {
                    envPath = Path.Combine(dir.FullName, ".env");
                    if (File.Exists(envPath))
                        return envPath;
                    break;
                }
                dir = dir.Parent;
            }

            return null;
        }
    }
}
