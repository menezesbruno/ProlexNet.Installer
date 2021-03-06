﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using ProlexNetSetup.Models;

namespace ProlexNetSetup.Library
{
    internal class DownloadParameters
    {
        public static AppList AppList { get; private set; }

        public static async Task AppListAsync()
        {
            WebClient client = new WebClient();

            try
            {
                var appList = await client.DownloadStringTaskAsync(Constants.AppListUrl);
                AppList = JsonConvert.DeserializeObject<AppList>(appList);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"{nameof(DownloadParameters)}:{nameof(AppListAsync)}:{ex.Message}");
                MessageBox.Show("Servidor de downloads fora do ar. Informe ao setor de desenvolvimento.", "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);

                Application.Current.Shutdown();
            }
        }
    }
}