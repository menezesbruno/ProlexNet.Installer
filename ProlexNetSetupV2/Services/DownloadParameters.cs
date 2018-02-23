using Newtonsoft.Json;
using ProlexNetSetupV2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProlexNetSetupV2.Services
{
    class DownloadParameters
    {
        public static ApplicationList Instance { get; private set; }

        public static async Task<bool> ApplicationListAsync()
        {
            WebClient client = new WebClient();

            try
            {
                var json = await client.DownloadStringTaskAsync(Constants.DownloadServerUrl);
                Instance = JsonConvert.DeserializeObject<ApplicationList>(json);
                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"{nameof(DownloadParameters)}:{nameof(ApplicationListAsync)}:{ex.Message}");
                MessageBox.Show("Servidor de downloads fora do ar. Informe ao setor de desenvolvimento.", "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}