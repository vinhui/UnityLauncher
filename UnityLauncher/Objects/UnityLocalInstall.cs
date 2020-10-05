using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace UnityLauncher
{
    public class UnityLocalInstall
    {
        public string ExecutablePath { get; }
        public string RootPath { get; }
        public string Version { get; }

        public UnityLocalInstall(string executablePath, string version)
        {
            ExecutablePath = executablePath;
            Version = version;
            RootPath = Directory.GetParent(executablePath).Parent?.FullName;
            GetModules().Wait(1000);
        }

        /// <summary>
        /// Launch this Unity install
        /// </summary>
        /// <param name="arguments">Arguments to launch Unity with</param>
        /// <returns>Exit code from unity if <see cref="UnityLaunchArguments.WaitForExit"/> is true, otherwise 0</returns>
        public int Launch(UnityLaunchArguments arguments)
        {
            var argumentsString = arguments.ToString();
            Logger.Info("Launching Unity '{0}' with the following arguments: '{1}'", Version, arguments);

            using var process = new Process();
            process.StartInfo.FileName = ExecutablePath;
            process.StartInfo.Arguments = argumentsString;
            process.StartInfo.WindowStyle = arguments.BatchMode ? ProcessWindowStyle.Hidden : ProcessWindowStyle.Normal;

            if (arguments.WaitForExit)
            {
                process.StartInfo.CreateNoWindow = false;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
                process.ErrorDataReceived += (sender, args) => Console.WriteLine(args.Data);
            }
            else
            {
                process.StartInfo.UseShellExecute = true;
            }

            process.Start();
            if (arguments.WaitForExit)
            {
                Logger.Info("Unity Output:");
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();

                Logger.Debug("Unity has finished with exit code {0}", process.ExitCode);
                return process.ExitCode;
            }

            return 0;
        }

        public async Task<UnityLocalInstallModule[]> GetModules()
        {
            var path = Path.Combine(RootPath, "modules.json");
            await using var fileStream = new FileStream(path, FileMode.Open);
            var modules = await JsonSerializer.DeserializeAsync<UnityLocalInstallModule[]>(fileStream,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            return modules;
        }
    }
}