using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamvvm;
using Unity;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace NinjaMvvm.Xam.Samples
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //  IoCContainer.Initialize(Xamvvm.XamvvmIoC.Instance);
            var container = this.InitIoC();


            var navigator = container.Resolve<Abstractions.INavigator>();

            NinjaMvvm.Xam.NavigationComponent.Init<PageModels.MainPageModel>(this, navigator);
        }
        private IUnityContainer InitIoC()
        {
            var container = new UnityContainer();
            NinjaMvvm.Xam.UnityIoC.Init(container);


            return container;
        }
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }

}
