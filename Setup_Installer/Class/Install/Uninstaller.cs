using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
                                    var uninstall = MessageBox.Show("Tem certeza que deseja remover o ProlexNet Client?", "Aviso!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                    if (uninstall == MessageBoxResult.Yes)
                                    {
                                        var installedPath = child.GetValue("InstallLocation");
                                        child.Close();
                                        try
                                        {
                                            var deleteShortcut = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory), "ProlexNet.lnk");
                                            Directory.Delete(installedPath.ToString(), true);
                                            key.DeleteSubKey(applicationGuid, false);
                                            if (File.Exists(deleteShortcut))
                                            {
                                                try
                                                {
                                                    File.Delete(deleteShortcut);
                                                }
                                                catch
                                                {

                                                }
                                            }
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

        public static void ProlexNetServer(string applicationGuid, string windowsUninstallPath)
        {
            string[] args = Environment.GetCommandLineArgs();
            foreach (string arg in args)
            {
                if (arg == "/serveruninstallprompt")
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
                                    var uninstall = MessageBox.Show("Tem certeza que deseja remover o ProlexNet Server? O banco de dados localizado na pasta Database NÃO será removido.", "Aviso!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                    if (uninstall == MessageBoxResult.Yes)
                                    {
                                        var rootPath = child.GetValue("RootLocation");
                                        child.Close();
                                        try
                                        {
                                            Common.IISManipulationClass.RemoveSite("prolexnet");
                                            Common.IISManipulationClass.RemoveSite("prolexnet_updater");
                                            Common.IISManipulationClass.RemovePool("prolexnet");
                                            Directory.Delete(rootPath.ToString(), true);
                                            key.DeleteSubKey(applicationGuid, false);

                                            MessageBox.Show("ProlexNet Server removido com sucesso!", "Aviso!", MessageBoxButton.OK, MessageBoxImage.Information);
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

        public static void Firebird()
        {
            // Desinstala o Firebird
            try
            {
                var firebirdUninstaller = Directory.GetFiles(@"C:\Program Files\Firebird", "unins*.exe", SearchOption.AllDirectories).FirstOrDefault();
                Process process = new Process();
                process.StartInfo.FileName = firebirdUninstaller;
                process.StartInfo.Arguments = "/CLEAN";
                process.Start();
                process.WaitForExit();
            }
            catch
            {

            }

            try
            {
                var firebirdUninstaller = Directory.GetFiles(@"C:\Program Files (x86)\Firebird", "unins*.exe", SearchOption.AllDirectories).FirstOrDefault();
                Process process = new Process();
                process.StartInfo.FileName = firebirdUninstaller;
                process.StartInfo.Arguments = "/CLEAN";
                process.Start();
                process.WaitForExit();
            }
            catch
            {

            }
        }
    }
}
