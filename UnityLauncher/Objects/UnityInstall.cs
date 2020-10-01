using System;
using System.Diagnostics;

namespace UnityLauncher
{
    public class UnityInstall
    {
        public string Path { get; }
        public string Version { get; }

        public UnityInstall(string path, string version)
        {
            Path = path;
            Version = version;
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
            process.StartInfo.FileName = Path;
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
    }
}