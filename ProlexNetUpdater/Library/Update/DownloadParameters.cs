using Newtonsoft.Json;
using ProlexNetUpdater.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProlexNetUpdater.Library.Update
{
    public class DownloadParameters
    {
        public static ApplicationList ApplicationList { get; set; }

        public static void LoadApplicationList()
        {
            WebClient client = new WebClient();

            try
            {
                var appList = client.DownloadString(Constants.AppListUrl);
                ApplicationList = JsonConvert.DeserializeObject<ApplicationList>(appList);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"{nameof(DownloadParameters)}:{nameof(ApplicationList)}:{ex.Message}");
            }
        }
    }
}