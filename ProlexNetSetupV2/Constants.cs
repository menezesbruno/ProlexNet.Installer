using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProlexNetSetupV2
{
    public class Constants
    {
        public const string DownloadServerUrl = "https://automatizabox.azurewebsites.net/uploads/applicationlist.json";

        public const string WindowsUninstallPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

        // GUID usado para criar entrada de registro de desinstalação do Windows.
        // Deve ser ÚNICO por aplicativo e deve também ser IMUTÁVEL durante toda a vida do aplicativo.
        // Pode ser criado em: https://www.guidgenerator.com/online-guid-generator.aspx
        // Opções a serem selecionadas: Braces + Hyphens
        public const string ClientApplicationGuid = "{ee152ba9-9db3-47c5-ba10-b6d08cdb74f4}";

        public const string ServerApplicationGuid = "{5f64652c-65c0-4e53-9f55-cd25f0afa39c}";

        #region Components

        public const string IIS = "IIS - Serviços de Informações da Internet";
        public const string FirebirdX86 = "Firebird x86";
        public const string FirebirdX64 = "Firebird x64";
        public const string ProlexNetServer = "ProlexNet Server";
        public const string ProlexNetUpdater = "ProlexNet Updater";
        public const string ProlexNetClient = "ProlexNet Client";
        public const string ProlexNetDatabase = "ProlexNet Database";
        public const string DotNet46 = "Microsoft .NET Framework 4.6";
        public const string VisualC2013X86 = "Microsoft Visual C++ 2013 x86";
        public const string VisualC2013X64 = "Microsoft Visual C++ 2013 x64";
        public const string LinqPad = "LinqPad 5";
        public const string IBExpert = "IBExpert";

        #endregion Components
    }
}