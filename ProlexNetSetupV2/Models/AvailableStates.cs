using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProlexNetSetup.Models
{
    public class AvailableStates
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Hash { get; set; }
        public Database Database { get; set; }
    }

    public class Database
    {
        public string Url { get; set; }
        public string Hash { get; set; }
    }
}