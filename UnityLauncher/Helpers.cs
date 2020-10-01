namespace UnityLauncher
{
    public static class Helpers
    {
        public static void CopyTo(this IUnityArguments from, IUnityArguments to)
        {
            from.WaitForExit = to.WaitForExit;
            from.BatchMode = to.BatchMode;
            from.BuildTarget = to.BuildTarget;
            from.ExecuteMethod = to.ExecuteMethod;
            from.NoGraphics = to.NoGraphics;
            from.LogFile = to.LogFile;
            from.SilentCrashes = to.SilentCrashes;
            from.Quit = to.Quit;
        }

        public static (string version, string changeset) ParseHubUri(string uri)
        {
            if (!uri.StartsWith("unityhub://"))
                return (null, null);

            uri = uri.Replace("unityhub://", "");
            var split = uri.Split('/');
            return split.Length != 2
                ? (null, null)
                : (split[0], split[1]);
        }
    }
}