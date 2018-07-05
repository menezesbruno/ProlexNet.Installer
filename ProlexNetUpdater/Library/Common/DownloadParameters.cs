using Newtonsoft.Json;
using ProlexNetUpdater.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProlexNetUpdater.Library.Common
{
    public class DownloadParameters
    {
        public static ApplicationList ApplicationList { get; set; }
        public static ApplicationList ScriptList { get; set; }

        public static void LoadApplicationList()
        {
            WebClient client = new WebClient();

            try
            {
                var appList = client.DownloadString(Constants.AppListUrl);
                var scriptList = client.DownloadString(Constants.ScriptListUrl);
                ApplicationList = JsonConvert.DeserializeObject<ApplicationList>(appList);
                ScriptList = JsonConvert.DeserializeObject<ApplicationList>(scriptList);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"{nameof(DownloadParameters)}:{nameof(ApplicationList)}:{ex.Message}");
            }
        }
    }
}