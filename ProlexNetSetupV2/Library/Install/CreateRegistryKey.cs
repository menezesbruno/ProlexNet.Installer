using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;

namespace ProlexNetSetupV2.Library
{
    internal class CreateRegistryKey
    {
        public static void ProlexNetClient(string servicePath, string installationPath)
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
                Trace.WriteLine($"{nameof(CreateRegistryKey)}:{nameof(ProlexNetClient)}:{ex.Message}");
            }

            using (RegistryKey parent = Registry.LocalMachine.OpenSubKey(Constants.WindowsUninstallPath, true))
            {
                if (parent == null)
                    throw new Exception("Chave de desinstalação não encontrada.");

                try
                {
                    RegistryKey key = null;
                    try
                    {
                        key = parent.OpenSubKey(Constants.ClientApplicationGuid, true) ??
                              parent.CreateSubKey(Constants.ClientApplicationGuid);

                        if (key == null)
                            throw new Exception(String.Format("Não foi possível criar entrada do desinstalador. '{0}\\{1}'", Constants.WindowsUninstallPath, Constants.ClientApplicationGuid));

                        var uninstallerExe = Path.Combine(servicePath, "ProlexNet.Setup.exe");
                        var exePath = Path.Combine(installationPath, "ProlexNet Client", "ProlexNet.ExtHost.exe");
                        var installedPath = Path.Combine(installationPath, "ProlexNet Client");
                        var versionInfo = FileVersionInfo.GetVersionInfo(exePath);
                        var version = versionInfo.ProductVersion;

                        key.SetValue("DisplayName", "ProlexNet Client");
                        key.SetValue("ApplicationVersion", version.ToString());
                        key.SetValue("Publisher", "Automatiza Tecnologia e Automação Ltda.");
                        key.SetValue("DisplayIcon", exePath);
                        key.SetValue("InstallLocation", installedPath);
                        key.SetValue("DisplayVersion", version.ToString());
                        key.SetValue("URLInfoAbout", "http://www.automatizatec.com.br/");
                        key.SetValue("Contact", "automatiza@automatizatec.com.br");
                        key.SetValue("InstallDate", DateTime.Now.ToString("yyyyMMdd"));
                        key.SetValue("UninstallString", uninstallerExe + " /uninstallprompt");
                    }
                    finally
                    {
                        if (key != null)
                            key.Close();
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"{nameof(CreateRegistryKey)}:{nameof(ProlexNetClient)}:{ex.Message}");
                    throw new Exception(
                        "Um erro ocorreu durante a criação da entrada de registro no sistema. O programa foi instalado com sucesso, mas a sua desinstalação deverá ser feita manualmente caso seja necessário.",
                        ex);
                }
            }
        }

        public static void ProlexNetServer(string servicePath, string installationPath)
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
                Trace.WriteLine($"{nameof(CreateRegistryKey)}:{nameof(ProlexNetServer)}:{ex.Message}");
            }

            using (RegistryKey parent = Registry.LocalMachine.OpenSubKey(Constants.WindowsUninstallPath, true))
            {
                if (parent == null)
                    throw new Exception("Chave de desinstalação não encontrada.");

                try
                {
                    RegistryKey key = null;
                    try
                    {
                        key = parent.OpenSubKey(Constants.ServerApplicationGuid, true) ??
                              parent.CreateSubKey(Constants.ServerApplicationGuid);

                        if (key == null)
                            throw new Exception(String.Format("Não foi possível criar entrada do desinstalador. '{0}\\{1}'", Constants.WindowsUninstallPath, Constants.ServerApplicationGuid));

                        var uninstallerExe = Path.Combine(servicePath, "ProlexNet.Setup.exe");
                        var installedPath = Path.Combine(installationPath, "ProlexNet Server", "www");
                        var updaterPath = Path.Combine(installationPath, "ProlexNet Server", "updater");
                        var rootPath = Path.Combine(installationPath, "ProlexNet Server");
                        var iconPath = Path.Combine(installedPath, "images", "favicon.ico");
                        var version = "1.0.0.0";

                        key.SetValue("DisplayName", "ProlexNet Server");
                        key.SetValue("ApplicationVersion", version);
                        key.SetValue("Publisher", "Automatiza Tecnologia e Automação Ltda.");
                        key.SetValue("DisplayIcon", iconPath);
                        key.SetValue("InstallLocation", installedPath);
                        key.SetValue("UpdaterLocation", updaterPath);
                        key.SetValue("RootLocation", rootPath);
                        key.SetValue("DisplayVersion", version);
                        key.SetValue("URLInfoAbout", "http://www.automatizatec.com.br/");
                        key.SetValue("Contact", "automatiza@automatizatec.com.br");
                        key.SetValue("InstallDate", DateTime.Now.ToString("yyyyMMdd"));
                        key.SetValue("UninstallString", uninstallerExe + " /serveruninstallprompt");
                    }
                    finally
                    {
                        if (key != null)
                            key.Close();
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"{nameof(CreateRegistryKey)}:{nameof(ProlexNetServer)}:{ex.Message}");
                    throw new Exception(
                        "Um erro ocorreu durante a criação da entrada de registro no sistema. O programa foi instalado com sucesso, mas a sua desinstalação deverá ser feita manualmente caso seja necessário.",
                        ex);
                }
            }
        }
    }
}