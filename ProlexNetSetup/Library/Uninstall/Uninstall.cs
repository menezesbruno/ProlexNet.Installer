using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace ProlexNetSetup.Library
{
    public static class Uninstall
    {
        public static void Run()
        {
            var applicationGuid = Constants.ApplicationGuid;
            var registryUninstallPath = Constants.RegistryUninstallPath;

            string[] args = Environment.GetCommandLineArgs();
            foreach (string item in args)
            {
                if (item == "/uninstall")
                    ProlexNet(registryUninstallPath, applicationGuid);
            }
        }

        public static void ProlexNet(string registryUninstallPath, string applicationGuid)
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryUninstallPath, true))
            {
                if (key != null)
                {
                    try
                    {
                        RegistryKey child = key.OpenSubKey(applicationGuid);
                        if (child != null)
                        {
                            var uninstall = MessageBox.Show("Tem certeza que deseja remover o ProlexNet? O banco de dados NÃO será removido.", "Aviso!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (uninstall == MessageBoxResult.Yes)
                            {
                                var rootPath = child.GetValue("RootLocation");
                                child.Close();
                                try
                                {
                                    ConfigIIS.RemoveSite("prolexnet");
                                    ConfigIIS.RemovePool("prolexnet");
                                    Firewall.RemoveRules();
                                    Directory.Delete(rootPath.ToString(), true);
                                    key.DeleteSubKey(applicationGuid, false);

                                    MessageBox.Show("ProlexNet removido com sucesso!", "Aviso!", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message, "Aviso!", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            Application.Current.Shutdown();
        }
    }
}