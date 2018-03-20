using Newtonsoft.Json;
using ProlexNetSetupV2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace ProlexNetSetupV2.Library
{
    internal class DownloadParameters
    {
        public static ApplicationList AppList { get; private set; }
        public static List<AvailableStates> StatesList { get; private set; }

        public static async Task ApplicationListAsync()
        {
            WebClient client = new WebClient();

            try
            {
                var appList = await client.DownloadStringTaskAsync(Constants.AppListUrl);
                var statesList = await client.DownloadStringTaskAsync(Constants.StatesListUrl);

                AppList = JsonConvert.DeserializeObject<ApplicationList>(appList);
                StatesList = JsonConvert.DeserializeObject<List<AvailableStates>>(statesList);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"{nameof(DownloadParameters)}:{nameof(ApplicationListAsync)}:{ex.Message}");
                MessageBox.Show("Servidor de downloads fora do ar. Informe ao setor de desenvolvimento.", "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);

                Application.Current.Shutdown();
            }
        }
    }
}