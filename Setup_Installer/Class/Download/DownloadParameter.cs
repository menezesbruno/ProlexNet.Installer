using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProlexNetSetup.Class.Download
{
    public class DownloadParameters
    {
        public static DownloadParameters Instance { get; private set; }

        // Parâmentros contidos no arquivo JSON servido pelo servidor da Automatiza
        // Atenção: case sensitive e underline
        public string Firebird_X86_Url { get; set; }
        public string Firebird_X86_Hash { get; set; }
        public string Firebird_X64_Url { get; set; }
        public string Firebird_X64_Hash { get; set; }
        public string Prolex6_Url { get; set; }
        public string Prolex6_Hash { get; set; }
        public string ProlexTDPJ_Url { get; set; }
        public string ProlexTDPJ_Hash { get; set; }
        public string ProlexNet_Server_Url { get; set; }
        public string ProlexNet_Server_Hash { get; set; }
        public string ProlexNet_Client_Url { get; set; }
        public string ProlexNet_Client_Hash { get; set; }
        public string ProlexNet_Database_Url { get; set; }
        public string ProlexNet_Database_Hash { get; set; }
        public string DotNet46_Url { get; set; }
        public string DotNet46_Hash { get; set; }
        public string DotNet47_Url { get; set; }
        public string DotNet47_Hash { get; set; }
        public string VisualC2103_X86_Url { get; set; }
        public string VisualC2103_X86_Hash { get; set; }
        public string VisualC2103_X64_Url { get; set; }
        public string VisualC2103_X64_Hash { get; set; }

        public const string AplicationsUrl = "https://automatizabox.azurewebsites.net/uploads/applicationlist.json";

        public static async Task ApplicationListAsync()
        {
            WebClient client = new WebClient();

            var json = await client.DownloadStringTaskAsync(AplicationsUrl);
            Instance = DeserializeJson(json);
        }

        public static DownloadParameters DeserializeJson(string json)
        {
            return JsonConvert.DeserializeObject<DownloadParameters>(json);
        }
    }
}
