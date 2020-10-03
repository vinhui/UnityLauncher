using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

// ReSharper disable ClassNeverInstantiated.Global

namespace UnityLauncher
{
    public abstract class CliOptions
    {
        [Option(Required = false, HelpText = "Enable more logging")]
        public bool Verbose { get; set; }

        [Option("hub-path", Required = true, HelpText = "Path to the hub executable")]
        public string HubPath { get; set; }
    }

    [Verb("list", HelpText = "List all installed Unity editors")]
    public class ListOptions : CliOptions
    {
        [Usage]
        public static IEnumerable<Example> Examples
        {
            get { yield return new Example("List installs", new ListOptions {HubPath = "~/UnityHub.AppImage"}); }
        }
    }

    [Verb("launch", HelpText = "Launch a Unity project with the correct Unity version")]
    public class LaunchOptions : CliOptions, IUnityArguments
    {
        [Option("project", Required = true, HelpText = "Path to the project that should be opened")]
        public string ProjectPath { get; set; }

        [Option("wait-for-exit", HelpText = "Wait for the Unity process to exit before continuing")]
        public bool WaitForExit { get; set; }

        [Option("install-if-needed",
            HelpText = "Should the correct Unity version be installed if it's not installed yet")]
        public bool InstallIfNeeded { get; set; }

        [Option(HelpText = "Shortcut for batchmode, quit, no-graphics and silent-crashes")]
        public bool Headless { get; set; }

        [Option(HelpText = "https://docs.unity3d.com/Manual/CommandLineArguments.html")]
        public bool BatchMode { get; set; }

        [Option("build-target", HelpText = "https://docs.unity3d.com/Manual/CommandLineArguments.html")]
        public string BuildTarget { get; set; }

        [Option("execute-method", HelpText = "https://docs.unity3d.com/Manual/CommandLineArguments.html")]
        public string ExecuteMethod { get; set; }

        [Option("no-graphics", HelpText = "https://docs.unity3d.com/Manual/CommandLineArguments.html")]
        public bool NoGraphics { get; set; }

        [Option("log-file",
            HelpText =
                "Will be ignored if '--wait-for-exit' is passed https://docs.unity3d.com/Manual/CommandLineArguments.html")]
        public string LogFile { get; set; }

        [Option("silent-crashes", HelpText = "https://docs.unity3d.com/Manual/CommandLineArguments.html")]
        public bool SilentCrashes { get; set; }

        [Option(HelpText = "https://docs.unity3d.com/Manual/CommandLineArguments.html")]
        public bool Quit { get; set; }

        [Usage]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("Opening project", new LaunchOptions
                {
                    HubPath = "~/UnityHub.AppImage",
                    ProjectPath = "~/projects/MyUnityProject"
                });
                yield return new Example("Opening project headless", new LaunchOptions
                {
                    HubPath = "~/UnityHub.AppImage",
                    ProjectPath = "~/projects/MyUnityProject",
                    Headless = true,
                    ExecuteMethod = "My.Custom.Method",
                    WaitForExit = true
                });
            }
        }
    }

    [Verb("install", HelpText = "Install a new Unity version")]
    public class InstallOptions : CliOptions
    {
        [Option(Group = "type", HelpText = "Hub URI, for example 'unityhub://2020.1.6f1/fc477ca6df10'")]
        public string HubUri { get; set; }

        [Option(Group = "type", HelpText = "Version string, for example '2020.1.6f1'")]
        public string Version { get; set; }

        [Option(HelpText =
            "Changeset for the version, might be required if using '--version' and the version is not the latest minor version")]
        public string Changeset { get; set; }

        [Option(HelpText = "Additional modules to be installed")]
        public IEnumerable<string> Modules { get; set; }

        [Option("install-child-modules",
            HelpText = "Install all the child modules of the modules passed with '--modules'")]
        public bool InstallChildModules { get; set; }

        [Usage]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("Basic Install", new InstallOptions
                {
                    HubPath = "~/UnityHub.AppImage",
                    Version = "202.1.4f1"
                });
                yield return new Example("Hub URI with modules", new InstallOptions
                {
                    HubPath = "~/UnityHub.AppImage",
                    HubUri = "unityhub://2020.1.6f1/fc477ca6df10",
                    Modules = new[] {"android", "webgl"},
                    InstallChildModules = true
                });
            }
        }
    }
}