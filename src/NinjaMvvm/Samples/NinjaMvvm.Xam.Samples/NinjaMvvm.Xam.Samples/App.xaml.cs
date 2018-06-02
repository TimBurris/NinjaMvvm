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
            //  IoCContainer.Initialize(Xamvvm.XamvvmIoC.Instance);

            NinjaMvvm.Xam.NavigationComponent.Init<PageModels.MainPageModel>(this);
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
