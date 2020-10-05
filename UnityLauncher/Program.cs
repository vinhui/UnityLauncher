using CommandLine;
using UnityLauncher.Objects;

namespace UnityLauncher
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<ListOptions, LaunchOptions, InstallOptions>(args)
                .WithParsed<ListOptions>(Verbs.ListInstalls)
                .WithParsed<LaunchOptions>(Verbs.Launch)
                .WithParsed<InstallOptions>(Verbs.Install);
        }
    }
}