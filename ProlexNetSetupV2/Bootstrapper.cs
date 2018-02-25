using Autofac;
using Prism.Autofac;
using Prism.Modularity;
using ProlexNetSetupV2.Views;
using System.Windows;

namespace ProlexNetSetupV2
{
    internal class Bootstrapper : AutofacBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureModuleCatalog()
        {
            var moduleCatalog = (ModuleCatalog)ModuleCatalog;
            //moduleCatalog.AddModule(typeof(YOUR_MODULE));
        }
    }
}