using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Concurrency.Tpl.MockProcesses;
using Microsoft.Practices.Unity;

namespace Concurrency.Tpl
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            UnityContainer myUnityContainer = new UnityContainer();
            //make sure your container is configured
            myUnityContainer.RegisterType<IIndipendentProcessesMock, IndipendentProcessesMock>();
            myUnityContainer.RegisterType<IMainWindow, MainWindow>();

            myUnityContainer.Resolve<IMainWindow>().Show();
        }
    }

}
