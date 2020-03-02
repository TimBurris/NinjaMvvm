using System;
using System.Collections.Generic;
using System.Text;
using Xamvvm;

namespace NinjaMvvm.Xam
{
    public static class NavigationComponent
    {
        /// <summary>
        /// Initializes NinjaMvvm Required components
        /// </summary>
        /// <typeparam name="TPageModel"></typeparam>
        /// <param name="app"></param>
        /// <param name="navigator"></param>
        /// <param name="wrapWithNavigationPage">specifies whether or not a Generic navigation page should be created to host the starting page <typeparamref name="TPageModel"/></param>
        public static void Init<TPageModel>(Xamarin.Forms.Application app, Abstractions.INavigator navigator, bool wrapWithNavigationPage = true) where TPageModel : class, Abstractions.IPageModel
        {
            var factory = new Xamvvm.XamvvmFormsFactory(app);

            Xamvvm.XamvvmCore.SetCurrentFactory(factory);

            if (wrapWithNavigationPage)
            {
                factory.RegisterNavigationPage<MainNavigationPageModel>(() => app.GetPageFromCache<TPageModel>());
                var navPage = app.GetPageFromCache<MainNavigationPageModel>() as Xamarin.Forms.NavigationPage;
                navigator.SetRootPage(navPage);
            }
            else
            {
                var page = app.GetPageFromCache<TPageModel>();
                navigator.SetRootPage<TPageModel>();
            }
        }

        private class MainNavigationPageModel : NinjaMvvm.Xam.XamPageModelBase
        {
        }
    }
}
