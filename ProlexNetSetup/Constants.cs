namespace ProlexNetSetup
{
    public class Constants
    {
        public const string AppListUrl = "https://automatizabox.azurewebsites.net/uploads/applicationlist.json";
        public const string RegistryUninstallPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

        // GUID usado para criar entrada de registro de desinstalação do Windows.
        // Deve ser ÚNICO por aplicativo e deve também ser IMUTÁVEL durante toda a vida do aplicativo.
        // Pode ser criado em: https://www.guidgenerator.com/online-guid-generator.aspx
        // Opções a serem selecionadas: Braces + Hyphens
        public const string ApplicationGuid = "{054dddb2-d4c7-4657-97ce-39daa82e9a1f}";

        #region Components

        public const string IIS = "IIS - Serviços de Informações da Internet";
        public const string ProlexNet = "ProlexNet";
        public const string Database = "Banco de dados";
        public const string NetCore21 = "Microsoft .NET Core 2.1 Runtime";
        public const string LINQPad5 = "LINQPad 5";

        #endregion Components
    }
}