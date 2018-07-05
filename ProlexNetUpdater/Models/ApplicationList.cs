using System.Collections.Generic;

namespace ProlexNetUpdater.Models
{
    public class ApplicationList
    {
        public Files ProlexNet { get; set; }
        public List<Scripts> ScriptList { get; set; }
    }
    public class Files
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Hash { get; set; }
    }

    public class Scripts
    {
        public string State { get; set; }
        public double Version { get; set; }
        public string Url { get; set; }
        public string Hash { get; set; }
    }
}
