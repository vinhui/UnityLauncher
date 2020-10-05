namespace UnityLauncher.Objects
{
    public class UnityLocalInstallModule
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DownloadUrl { get; set; }
        public string Category { get; set; }
        public object InstalledSize { get; set; }
        public int DownloadSize { get; set; }
        public bool Visible { get; set; }
        public bool Selected { get; set; }
        public string Destination { get; set; }
        public string Checksum { get; set; }
        public string Sync { get; set; }
        public string Parent { get; set; }
        public string EulaUrl1 { get; set; }
        public string EulaLabel1 { get; set; }
        public string EulaMessage { get; set; }
        public string RenameTo { get; set; }
        public string RenameFrom { get; set; }
    }
}