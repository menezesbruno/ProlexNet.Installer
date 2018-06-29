using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProlexNetUpdater
{
    public class Constants
    {
        public const string AppListUrl = "https://automatizabox.azurewebsites.net/uploads/update.json";
        public const string RegistryUninstallPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

        // GUID usado para criar entrada de registro de desinstalação do Windows.
        // Deve ser ÚNICO por aplicativo e deve também ser IMUTÁVEL durante toda a vida do aplicativo.
        // Pode ser criado em: https://www.guidgenerator.com/online-guid-generator.aspx
        // Opções a serem selecionadas: Braces + Hyphens
        public const string ApplicationGuid = "{054dddb2-d4c7-4657-97ce-39daa82e9a1f}";
    }
}