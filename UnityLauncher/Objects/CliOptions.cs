using CommandLine;

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

        [Option("silent-crashes")] public bool SilentCrashes { get; set; }

        [Option(HelpText = "https://docs.unity3d.com/Manual/CommandLineArguments.html")]
        public bool Quit { get; set; }
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
    }
}