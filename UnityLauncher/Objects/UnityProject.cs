using System.IO;

namespace UnityLauncher
{
    public class UnityProject
    {
        public string Path { get; private set; }
        public string UnityVersion { get; private set; }
        public string UnityVersionChangeset { get; set; }

        public UnityProject(string path)
        {
            Path = path;

            (UnityVersion, UnityVersionChangeset) = GetUnityVersion();
            Logger.Debug("Unity project '{0}' seems to be using Unity version '{1}'", Path, UnityVersion);
        }

        /// <summary>
        /// Check if the project is a valid unity project
        /// </summary>
        /// <returns>Returns if the project is valid or not</returns>
        public bool IsValid()
        {
            return Directory.Exists(Path) &&
                   File.Exists(GetProjectVersionFilePath());
        }

        private string GetProjectVersionFilePath()
        {
            return System.IO.Path.Combine(Path, "ProjectSettings/", "ProjectVersion.txt");
        }

        private (string version, string changeset) GetUnityVersion()
        {
            if (!IsValid())
                return (null, null);

            var projectVersionFilePath = GetProjectVersionFilePath();
            foreach (var line in File.ReadLines(projectVersionFilePath))
            {
                var keyValuePair = line.Split(':');
                if (keyValuePair.Length != 2)
                    continue;

                if (keyValuePair[0].Trim() != "m_EditorVersionWithRevision")
                    continue;

                var versionSplit = keyValuePair[1].Trim().Split(' ');
                if (versionSplit.Length != 2)
                    continue;

                return (versionSplit[0].Trim(),
                    versionSplit[1]
                        .Replace("(", "")
                        .Replace(")", "")
                        .Trim());
            }

            return (null, null);
        }
    }
}