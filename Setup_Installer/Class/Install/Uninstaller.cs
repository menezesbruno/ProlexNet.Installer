using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace ProlexNetSetup.Class.Install
{
    public class Uninstaller
    {
        public static void ProlexNetClient(string applicationGuid, string windowsUninstallPath)
        {
            string[] args = Environment.GetCommandLineArgs();
            foreach (string arg in args)
            {
                if (arg == "/uninstallprompt")
                {
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(windowsUninstallPath, true))
                    {
                        if (key != null)
                        {
                            try
                            {
                                RegistryKey child = key.OpenSubKey(applicationGuid);
                                if (child != null)
                                {
                                    var uninstall = MessageBox.Show("Tem certeza que deseja remover o ProlexNet Client? O banco de dados localizado na pasta Database NÃO será removido.", "Aviso!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                    if (uninstall == MessageBoxResult.Yes)
                                    {
                                        var installedPath = child.GetValue("InstallLocation");
                                        child.Close();
                                        try
                                        {
                                            Directory.Delete(installedPath.ToString(), true);
                                            key.DeleteSubKey(applicationGuid, false);
                                            MessageBox.Show("ProlexNet Client removido com sucesso!", "Aviso!", MessageBoxButton.OK, MessageBoxImage.Information);
                                            Environment.Exit(1);
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex.Message, "Aviso!", MessageBoxButton.OK, MessageBoxImage.Error);
                                            Environment.Exit(1);
                                        }
                                    }
                                    else
                                    {
                                        Environment.Exit(1);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                                Environment.Exit(1);
                            }
                        }
                    }
                }
            }
        }
    }
}
