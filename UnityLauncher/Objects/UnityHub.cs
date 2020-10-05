using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace UnityLauncher
{
    public class UnityHub
    {
        private const string HEADLESS_ARG = "--headless";

        private readonly string _hubPath;

        public UnityHub(string hubPath)
        {
            _hubPath = hubPath;
        }

        /// <summary>
        /// Validates that the path to the hub is an existing file
        /// </summary>
        /// <returns>Returns if the path exists or not</returns>
        public bool PathExists()
        {
            var exists = File.Exists(_hubPath);
            Logger.Debug("Hub file exists: {0}", exists);
            return exists;
        }

        public UnityLocalInstall GetInstall(string version)
        {
            var unityInstalls = GetAllInstalled();
            foreach (var unityInstall in unityInstalls)
            {
                if (unityInstall.Version == version)
                    return unityInstall;
            }

            Logger.Debug($"Could not find a matching Unity install for version '{version}'");
            return null;
        }

        /// <summary>
        /// Get all the installed Unity versions that are registered in the hub
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidDataException"></exception>
        public IEnumerable<UnityLocalInstall> GetAllInstalled()
        {
            var (output, _, exitCode) = RunHeadlessCommand("editors -i");
            if (exitCode != 0)
                yield break;

            using var stringReader = new StringReader(output);
            string line;
            do
            {
                line = stringReader.ReadLine();
                if (line == null)
                    continue;

                var strings = line.Split(',');
                if (strings.Length != 2)
                    continue;

                var version = strings[0].Trim();
                var path = strings[1].Replace("installed at", "").Trim();
                yield return new UnityLocalInstall(path, version);
            } while (line != null);
        }

        /// <summary>
        /// Install the specified Unity version
        /// </summary>
        /// <param name="version">Version to install</param>
        /// <param name="changeset">Changeset of the version, can be left null in some cases</param>
        /// <returns>Will return both if the install was successful and the full output of the installation process</returns>
        public (bool success, string output) Install(string version, string changeset)
        {
            if (string.IsNullOrWhiteSpace(version))
                return (false, null);

            var stringBuilder = new StringBuilder();
            stringBuilder.Append("install --version ");
            stringBuilder.Append(version);
            if (!string.IsNullOrWhiteSpace(changeset))
            {
                stringBuilder.Append(" --changeset ");
                stringBuilder.Append(changeset);
            }

            Logger.Info("Starting with installing");
            var (output, _, exitCode) = RunHeadlessCommand(stringBuilder.ToString(), true);
            if (exitCode != 0)
                return (false, output);

            return (!output.Contains("Error:"), output);
        }

        /// <summary>
        /// Install a module into a specific unity version
        /// </summary>
        /// <param name="unityVersion">Unity version to install the modules into</param>
        /// <param name="module">Module to install</param>
        /// <param name="installChildModules">Install child modules of the passed modules</param>
        /// <returns>Returns both if the installation was successful and the full output of the installation process</returns>
        public (bool success, string output) InstallModule(string unityVersion, string module, bool installChildModules)
        {
            if (string.IsNullOrWhiteSpace(unityVersion) || string.IsNullOrWhiteSpace(module))
                return (false, null);

            var stringBuilder = new StringBuilder();
            stringBuilder.Append("install-modules");
            stringBuilder.Append(" --version ");
            stringBuilder.Append(unityVersion);
            stringBuilder.Append(" --module ");
            stringBuilder.Append(module);

            if (installChildModules)
                stringBuilder.Append(" --childModules");

            Logger.Info("Starting with installing modules");
            var (output, error, exitCode) = RunHeadlessCommand(stringBuilder.ToString(), true);
            if (exitCode != 0)
                return (false, output);

            return (!output.Contains("Error:"), output);
        }

        /// <summary>
        /// Start the Unity hub with the passed arguments
        /// </summary>
        /// <param name="command">Arguments to pass</param>
        /// <param name="outputAsInfo">Log the output as Info instead of Debug (which requires Verbose)</param>
        /// <returns>Returns the full output and exit code</returns>
        private (string output, string error, int ExitCode) RunHeadlessCommand(string command,
            bool outputAsInfo = false)
        {
            var arguments = $"{HEADLESS_ARG} {command}";
            Logger.Debug("Starting hub process with arguments '{0}'", arguments);
            using var process = new Process
            {
                StartInfo =
                {
                    FileName = _hubPath,
                    Arguments = arguments,
                    CreateNoWindow = false,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };
            var output = new StringBuilder();
            var error = new StringBuilder();
            process.OutputDataReceived += (sender, args) =>
            {
                if (args.Data == null) return;
                if (outputAsInfo)
                    Logger.Info(args.Data);
                else
                    Logger.Debug(args.Data);
                output.AppendLine(args.Data);
            };
            process.ErrorDataReceived += (sender, args) =>
            {
                if (args.Data == null) return;
                Logger.Error(args.Data);
                error.AppendLine(args.Data);
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            if (error.Length > 0)
            {
                Logger.Error("Failed to run command '{0} {1}' with the following error:", _hubPath, arguments);
                Logger.Error(error.ToString());
            }

            Logger.Debug("Exit code: {0}", process.ExitCode);

            return (output.ToString(), error.ToString(), process.ExitCode);
        }
    }
}