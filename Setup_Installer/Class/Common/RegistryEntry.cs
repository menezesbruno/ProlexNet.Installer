using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace ProlexNetSetup.Class.Common
{
    public class RegistryEntry
    {
        public static void CreateProlexNetClientUninstaller(string servicePath, string installationPath, string applicationGuid, string windowsUninstallPath)
        {
            try
            {
                var setupFileName = Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().Location);
                var setupFile = Path.Combine(Directory.GetCurrentDirectory(), setupFileName);
                var uninstallFile = Path.Combine(servicePath, setupFileName);

                File.Copy(setupFile, uninstallFile, true);
            }
            catch
            {

            }
            using (RegistryKey parent = Registry.LocalMachine.OpenSubKey(windowsUninstallPath, true))
            {
                if (parent == null)
                {
                    throw new Exception("Chave de desinstalação não encontrada.");
                }
                try
                {
                    RegistryKey key = null;
                    try
                    {
                        key = parent.OpenSubKey(applicationGuid, true) ??
                              parent.CreateSubKey(applicationGuid);

                        if (key == null)
                        {
                            throw new Exception(String.Format("Não foi possível criar entrada do desinstalador. '{0}\\{1}'", windowsUninstallPath, applicationGuid));
                        }

                        var uninstallerExe = Path.Combine(servicePath, "ProlexNet.Setup.exe");
                        var exePath = Path.Combine(installationPath, "ProlexNet Client", "bin", "ProlexNet.ExtHost.exe");
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
                        {
                            key.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(
                        "Um erro ocorreu durante a criação da entrada de registro no sistema. O programa foi instalado com sucesso, mas a sua desinstalação deverá ser feita manualmente caso seja necessário.",
                        ex);
                }
            }
        }

        public static void CreateProlexNetServerUninstaller(string servicePath, string installationPath, string applicationGuid, string windowsUninstallPath)
        {
            try
            {
                var setupFileName = Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().Location);
                var setupFile = Path.Combine(Directory.GetCurrentDirectory(), setupFileName);
                var uninstallFile = Path.Combine(servicePath, setupFileName);

                File.Copy(setupFile, uninstallFile, true);
            }
            catch
            {

            }
            using (RegistryKey parent = Registry.LocalMachine.OpenSubKey(windowsUninstallPath, true))
            {
                if (parent == null)
                {
                    throw new Exception("Chave de desinstalação não encontrada.");
                }
                try
                {
                    RegistryKey key = null;
                    try
                    {
                        key = parent.OpenSubKey(applicationGuid, true) ??
                              parent.CreateSubKey(applicationGuid);

                        if (key == null)
                        {
                            throw new Exception(String.Format("Não foi possível criar entrada do desinstalador. '{0}\\{1}'", windowsUninstallPath, applicationGuid));
                        }

                        var uninstallerExe = Path.Combine(servicePath, "ProlexNet.Setup.exe");
                        var installedPath = Path.Combine(installationPath, "ProlexNet Server", "www");
                        var updaterPath = Path.Combine(installationPath, "ProlexNet Server", "updater");
                        var rootPath = Path.Combine(installationPath, "ProlexNet Server");
                        var iconPath = Path.Combine(installedPath, "favicon.ico");

                        key.SetValue("DisplayName", "ProlexNet Server");
                        key.SetValue("Publisher", "Automatiza Tecnologia e Automação Ltda.");
                        key.SetValue("DisplayIcon", iconPath);
                        key.SetValue("InstallLocation", installedPath);
                        key.SetValue("UpdaterLocation", updaterPath);
                        key.SetValue("RootLocation", rootPath);
                        key.SetValue("URLInfoAbout", "http://www.automatizatec.com.br/");
                        key.SetValue("Contact", "automatiza@automatizatec.com.br");
                        key.SetValue("InstallDate", DateTime.Now.ToString("yyyyMMdd"));
                        key.SetValue("UninstallString", uninstallerExe + " /serveruninstallprompt");
                    }
                    finally
                    {
                        if (key != null)
                        {
                            key.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(
                        "Um erro ocorreu durante a criação da entrada de registro no sistema. O programa foi instalado com sucesso, mas a sua desinstalação deverá ser feita manualmente caso seja necessário.",
                        ex);
                }
            }
        }
    }
}
