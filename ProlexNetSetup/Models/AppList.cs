using System.Collections.Generic;

namespace ProlexNetSetup.Models
{
    public class AppList
    {
        public Files ProlexNet { get; set; }

        public Files Database { get; set; }

        public Files NetCore { get; set; }

        public Files LinqPad { get; set; }

        public List<Files> StateList { get; set; }
    }

    public class Files
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Hash { get; set; }
    }
}