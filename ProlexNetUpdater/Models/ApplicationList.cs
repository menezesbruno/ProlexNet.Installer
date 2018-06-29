namespace ProlexNetUpdater.Models
{
    public class ApplicationList
    {
        public Files ProlexNet { get; set; }
        public Files Script { get; set; }
    }
    public class Files
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Hash { get; set; }
    }
}
