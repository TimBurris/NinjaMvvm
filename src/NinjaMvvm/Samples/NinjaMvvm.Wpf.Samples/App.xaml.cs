//*****************   UNITY  ********************************
#if false
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Unity;
using Unity.Injection;
namespace NinjaMvvm.Wpf.Samples
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IUnityContainer InitIoC()
        {
            var container = NinjaMvvm.Wpf.Unity.Component.Init<Navigator<MainWindow, DialogWindow>>();

            return container;
        }

        private void GoToStartingView(IUnityContainer container)
        {
            var nav = container.Resolve<Abstractions.INavigator>();

            nav.NavigateTo<ViewModels.HomeViewModel>();
        }

        private void InitMainWindow()
        {
            var x = new MainWindow();
            x.Show();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var container = this.InitIoC();
            this.InitMainWindow();
            this.GoToStartingView(container);
        }
    }
}

#endif


//*****************   NINJECT  ********************************
#if true
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Ninject;

namespace NinjaMvvm.Wpf.Samples
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IKernel InitIoC()
        {
            var container = NinjaMvvm.Wpf.Ninject.Component.Init<Navigator<MainWindow, DialogWindow>>();

            return container;
        }

        private void GoToStartingView(IKernel container)
        {
            var nav = container.Get<Abstractions.INavigator>();

            nav.NavigateTo<ViewModels.HomeViewModel>();
        }

        private void InitMainWindow()
        {
            var x = new MainWindow();
            x.Show();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var container = this.InitIoC();
            this.InitMainWindow();
            this.GoToStartingView(container);
        }
    }
}
#endif