using System;
using System.Collections.Generic;
using System.Linq;
using UnityLauncher.Objects;

namespace UnityLauncher
{
    public static class Verbs
    {
        public static void ListInstalls(ListOptions options)
        {
            Logger.Verbose = options.Verbose;

            var unityHub = new UnityHub(options.HubPath);
            if (!unityHub.PathExists())
            {
                Logger.Error("Unity hub was not found");
                Environment.ExitCode = 1;
                return;
            }

            Logger.Info("Installed Unity versions:");
            foreach (var unityInstall in unityHub.GetAllInstalled())
                Logger.Info("  - {0} ({1})", unityInstall.Version, unityInstall.ExecutablePath);
        }

        public static void Launch(LaunchOptions options)
        {
            Logger.Verbose = options.Verbose;

            var unityProject = new UnityProject(options.ProjectPath);
            Logger.Info("Attempting to launch Unity with project '{0}'", options.ProjectPath);
            if (!unityProject.IsValid())
            {
                Logger.Error("The passed project doesn't seem like a valid Unity project.");
                Environment.ExitCode = 1;
                return;
            }

            var unityHub = new UnityHub(options.HubPath);
            if (!unityHub.PathExists())
            {
                Logger.Error("Unity hub was not found");
                Environment.ExitCode = 1;
                return;
            }

            var unityInstall = unityHub.GetInstall(unityProject.UnityVersion);
            if (unityInstall == null)
            {
                if (!options.InstallIfNeeded)
                {
                    Logger.Error("Unity version '{0}' doesn't seem to be installed", unityProject.UnityVersion);
                    Logger.Info("You can use the '--install-if-needed' option to automatically install it");
                    Environment.ExitCode = 1;
                    return;
                }

                Logger.Info("Unity version is not yet installed, doing that next");
                if (!InstallVersion(unityHub, unityProject.UnityVersion, unityProject.UnityVersionChangeset))
                {
                    Logger.Error("Failed to install, not opening project");
                    Environment.ExitCode = 1;
                    return;
                }

                unityInstall = unityHub.GetInstall(unityProject.UnityVersion);
            }

            var unityArgs = new UnityLaunchArguments(unityProject);
            options.CopyTo(unityArgs);
            if (options.Headless)
                unityArgs.SetHeadless();
            var exitCode = unityInstall.Launch(unityArgs);

            Environment.ExitCode = exitCode;
        }

        public static void Install(InstallOptions options)
        {
            Logger.Verbose = options.Verbose;

            var unityHub = new UnityHub(options.HubPath);
            if (!unityHub.PathExists())
            {
                Logger.Error("Unity hub was not found");
                Environment.ExitCode = 1;
                return;
            }

            var version = options.Version;
            var changeset = options.Changeset;
            if (!string.IsNullOrWhiteSpace(options.HubUri))
            {
                (version, changeset) = Helpers.ParseHubUri(options.HubUri);
                if (version == null || changeset == null)
                {
                    Logger.Error("The passed hub URI is not in a valid format");
                    Environment.ExitCode = 1;
                    return;
                }

                Logger.Debug("Uri parsed version: {0}, changeset: {1}", version, changeset);
            }

            Logger.Debug("Checking if version is already installed");
            var unityInstall = unityHub.GetInstall(version);
            if (unityInstall != null)
            {
                if (!options.Modules.Any())
                {
                    Logger.Error("Unity version '{0}' seems to be installed already", version);
                    Environment.ExitCode = 1;
                    return;
                }
            }

            if (unityInstall == null)
            {
                Logger.Debug("Version is not yet installed");

                if (!InstallVersion(unityHub, version, changeset))
                {
                    Logger.Error("Failed to install Unity version '{0}'", version);
                    Environment.ExitCode = 1;
                    return;
                }

                Logger.Info("Base installation done");
            }
            else
            {
                Logger.Info("Unity version '{0}' is already installed", version);
                Logger.Info("Continuing with the modules");
            }

            if (options.Modules.Any())
            {
                if (!InstallModules(options, unityHub, version)) 
                    return;
            }

            Logger.Info("Installation complete");
        }

        private static bool InstallVersion(UnityHub unityHub, string version, string changeset)
        {
            var (success, output) = unityHub.Install(version, changeset);
            if (success)
                Logger.Info("Installations was successful");
            else
                Logger.Error("Installation failed");

            if (output == null)
            {
                Environment.ExitCode = 1;
                return false;
            }

            if (output.Contains("No editor version matched") && changeset == null)
                Logger.Info("You might also need to pass the 'changeset' option");

            Environment.ExitCode = success ? 0 : 1;
            return success;
        }

        private static bool InstallModules(InstallOptions options, UnityHub unityHub, string version)
        {
            Logger.Info("Beginning with installing modules. Include child modules: {0}",
                options.InstallChildModules);
            foreach (var module in options.Modules)
            {
                Logger.Info("Installing module '{0}'", module);
                var (success, output) =
                    unityHub.InstallModule(version, module, options.InstallChildModules);
                if (!success)
                {
                    Logger.Error("Failed to install module '{0}'", module);
                    Environment.ExitCode = 1;
                    return false;
                }
            }

            Logger.Info("Modules installation complete");
            return true;
        }
    }
}