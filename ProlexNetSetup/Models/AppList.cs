namespace ProlexNetSetup.Models
{
    public class AppList
    {
        public Files ProlexNet { get; set; }

        public Files Database { get; set; }

        public Files NetCore21 { get; set; }

        public Files LINQPad5 { get; set; }

        public States StateList { get; set; }
    }

    public class Files
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Hash { get; set; }
    }

    public class States
    {
        public string StateAcronym { get; set; }
        public string Url { get; set; }
        public string Hash { get; set; }
    }
}