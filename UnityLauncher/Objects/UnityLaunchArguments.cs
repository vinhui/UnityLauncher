using System.Text;

namespace UnityLauncher.Objects
{
    public interface IUnityArguments
    {
        public bool WaitForExit { get; set; }
        public bool BatchMode { get; set; }
        public string BuildTarget { get; set; }
        public string ExecuteMethod { get; set; }
        public bool NoGraphics { get; set; }
        public string LogFile { get; set; }
        public bool SilentCrashes { get; set; }
        public bool Quit { get; set; }
    }

    public class UnityLaunchArguments : IUnityArguments
    {
        public UnityProject UnityProject { get; }

        public bool WaitForExit { get; set; }

        public bool BatchMode { get; set; }
        public string BuildTarget { get; set; }
        public string ExecuteMethod { get; set; }
        public bool NoGraphics { get; set; }
        public string LogFile { get; set; }
        public bool SilentCrashes { get; set; }
        public bool Quit { get; set; }

        public UnityLaunchArguments(UnityProject unityProject)
        {
            UnityProject = unityProject;
        }

        public UnityLaunchArguments SetHeadless()
        {
            BatchMode = true;
            Quit = true;
            NoGraphics = true;
            SilentCrashes = true;
            return this;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("-projectPath \"");
            stringBuilder.Append(UnityProject.Path);
            stringBuilder.Append("\"");

            if (BatchMode) stringBuilder.Append(" -batchmode");

            if (BuildTarget != null)
            {
                stringBuilder.Append(" -buildtarget ");
                stringBuilder.Append(BuildTarget);
            }

            if (ExecuteMethod != null)
            {
                stringBuilder.Append(" -executemethod ");
                stringBuilder.Append(ExecuteMethod);
            }

            if (NoGraphics) stringBuilder.Append(" -nographics");

            if (WaitForExit)
                stringBuilder.Append(" -logFile -");
            else if (LogFile != null)
            {
                stringBuilder.Append(" -logFile \"");
                stringBuilder.Append(LogFile);
                stringBuilder.Append("\"");
            }

            if (SilentCrashes) stringBuilder.Append(" -silent-crashes");

            if (Quit) stringBuilder.Append(" -quit");

            return stringBuilder.ToString();
        }
    }
}