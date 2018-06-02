using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamvvm;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace NinjaMvvm.Xam.Samples
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var factory = new Xamvvm.XamvvmFormsFactory(this);
            factory.RegisterNavigationPage<MainNavigationPageModel>(() => this.GetPageFromCache<PageModels.MainPageModel>());
            Xamvvm.XamvvmCore.SetCurrentFactory(factory);


          //  IoCContainer.Initialize(Xamvvm.XamvvmIoC.Instance);

            var navPage = this.GetPageFromCache<MainNavigationPageModel>() as NavigationPage;
            MainPage = navPage;
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
    public class MainNavigationPageModel : NinjaMvvm.Xam.XamPageModelBase
    {
    }
}
