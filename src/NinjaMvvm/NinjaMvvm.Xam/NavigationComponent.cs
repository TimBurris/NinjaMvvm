using System;
using System.Collections.Generic;
using System.Text;
using Xamvvm;

namespace NinjaMvvm.Xam
{
    public static class NavigationComponent
    {
        public static void Init<TPageModel>(Xamarin.Forms.Application app, Abstractions.INavigator navigator) where TPageModel : class, IBasePageModel
        {
            var factory = new Xamvvm.XamvvmFormsFactory(app);

            factory.RegisterNavigationPage<MainNavigationPageModel>(() => app.GetPageFromCache<TPageModel>());
            Xamvvm.XamvvmCore.SetCurrentFactory(factory);

            var navPage = app.GetPageFromCache<MainNavigationPageModel>() as Xamarin.Forms.NavigationPage;
            navigator.SetRootPage(navPage);
            app.MainPage = navPage;
        }

        private class MainNavigationPageModel : NinjaMvvm.Xam.XamPageModelBase
        {
        }
    }
}
