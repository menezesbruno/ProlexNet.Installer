using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProlexNetSetup.Class
{
    public class Constants
    {
        public const string WindowsUninstallPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

        // GUID usado para criar entrada de registro de desinstalação do Windows.
        // Deve ser ÚNICO por aplicativo e deve também ser IMUTÁVEL durante toda a vida do aplicativo.
        // Pode ser criado em: https://www.guidgenerator.com/online-guid-generator.aspx
        // Opções a serem selecionadas: Braces + Hyphens
        public const string ClientApplicationGuid = "{ee152ba9-9db3-47c5-ba10-b6d08cdb74f4}";
        public const string ServerApplicationGuid = "{5f64652c-65c0-4e53-9f55-cd25f0afa39c}";
    }
}