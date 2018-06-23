using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace ProlexNetSetup.Library
{
    internal class CreateRegistryKey
    {
        public static void ProlexNet(string servicePath, string installationPath)
        {
            try
            {
                var setupFileName = Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().Location);
                var setupFile = Path.Combine(Directory.GetCurrentDirectory(), setupFileName);
                var uninstallFile = Path.Combine(servicePath, setupFileName);

                File.Copy(setupFile, uninstallFile, true);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"{nameof(CreateRegistryKey)}:{nameof(ProlexNet)}:{ex.Message}");
            }

            using (RegistryKey parent = Registry.LocalMachine.OpenSubKey(Constants.RegistryUninstallPath, true))
            {
                if (parent == null)
                    throw new Exception("Chave de desinstalação não encontrada.");

                try
                {
                    RegistryKey key = null;
                    try
                    {
                        key = parent.OpenSubKey(Constants.ApplicationGuid, true) ??
                              parent.CreateSubKey(Constants.ApplicationGuid);

                        if (key == null)
                            throw new Exception(String.Format("Não foi possível criar entrada do desinstalador. '{0}\\{1}'", Constants.RegistryUninstallPath, Constants.ApplicationGuid));

                        var uninstallerExe = Path.Combine(servicePath, "ProlexNet.Setup.exe");
                        var installedPath = Path.Combine(installationPath, "ProlexNet", "wwwroot");
                        var rootPath = Path.Combine(installationPath, "ProlexNet");
                        var iconPath = Path.Combine(installedPath, "images", "favicon.ico");
                        var version = "1.0.0.0";

                        key.SetValue("DisplayName", "ProlexNet");
                        key.SetValue("ApplicationVersion", version);
                        key.SetValue("Publisher", "Automatiza Tecnologia e Automação Ltda.");
                        key.SetValue("DisplayIcon", iconPath);
                        key.SetValue("InstallLocation", installedPath);
                        key.SetValue("RootLocation", rootPath);
                        key.SetValue("DisplayVersion", version);
                        key.SetValue("URLInfoAbout", "http://www.automatizatec.com.br/");
                        key.SetValue("Contact", "automatiza@automatizatec.com.br");
                        key.SetValue("InstallDate", DateTime.Now.ToString("yyyyMMdd"));
                        key.SetValue("UninstallString", uninstallerExe + " /uninstall");
                    }
                    finally
                    {
                        if (key != null)
                            key.Close();
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"{nameof(CreateRegistryKey)}:{nameof(ProlexNet)}:{ex.Message}");
                    throw new Exception(
                        "Um erro ocorreu durante a criação da entrada de registro no sistema. O programa foi instalado com sucesso, mas a sua desinstalação deverá ser feita manualmente caso seja necessário.",
                        ex);
                }
            }
        }
    }
}