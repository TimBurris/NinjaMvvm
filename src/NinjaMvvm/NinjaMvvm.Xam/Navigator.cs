using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NinjaMvvm.Xam
{
    public class Navigator : Abstractions.INavigator
    {
        //the xamarin has one "root page", from that root you can navigator or carosel or tab around
        private static Xamarin.Forms.Page _rootPage;

        public Navigator(Abstractions.IPageResolver viewResolver, Abstractions.IPageModelFactory pageModelFactory)
        {
            this.ViewResolver = viewResolver;
            this.PageModelFactory = pageModelFactory;
        }

        private Xamarin.Forms.NavigationPage GetNavigationPageThrowIfNot()
        {
            if (_rootPage == null)
            {
                throw new ApplicationException("A root page as not yet been assigned to the Navigator");
            }

            //if the root IS a nav page, all good, else, if they are using Tabbed view, the best we can do is try to see if the current page in the tab is a nav page.
            var page = (_rootPage as Xamarin.Forms.NavigationPage);

            if (page != null)
                return page;


            //NOTE:  we can't support Carrousel because Carousel can only contain content pages, not Navigation pages, which are needed to "navigate"

            var tabbed = (_rootPage as Xamarin.Forms.TabbedPage);
            if (tabbed == null)
                throw new ApplicationException($"This action requires the root be Navigation Page or a Tabbed Page, but the root is {_rootPage.GetType()}");

            if (tabbed.CurrentPage == null)
                throw new ApplicationException($"This action requires the 'CurrentPage' to be a NavigationPage, but the 'CurrentPage' is not set");


            page = tabbed.CurrentPage as Xamarin.Forms.NavigationPage;
            if (page == null)
                throw new ApplicationException($"This action requires the 'CurrentPage' to be a NavigationPage, but the 'CurrentPage' is {tabbed.CurrentPage.GetType()} ");

            return page;
        }
        #region INavigator Implementation

        #region "Pop"
        public async Task<Abstractions.IPageModel> PopAsync()
        {
            var currentPageModel = CurrentPageModel;
            await this.GetNavigationPageThrowIfNot().PopAsync();

            return currentPageModel;
        }

        public async Task<Abstractions.IPageModel> PopModalAsync()
        {
            var poppedPage = await _rootPage.Navigation.PopModalAsync();
            return poppedPage.BindingContext as Abstractions.IPageModel;
        }

        public Task PopToRootAsync()
        {
            return this.GetNavigationPageThrowIfNot().PopToRootAsync();
        }
        #endregion

        #region " Push "

        /// <summary>
        /// Pushes a new page onto the navigation stack, or replaces the current content with a new page if no Navigation stack is present (Windows Phone only).  Without a NavigationPage this method will not work (throws an exception) on Android and iOS.
        /// </summary>
        /// <returns></returns>
        public Task<TPageModel> PushAsync<TPageModel>() where TPageModel : class, Abstractions.IPageModel
        {
            return this.PushAsync<TPageModel>(initAction: null);
        }

        /// <summary>
        /// Pushes a new page onto the navigation stack, or replaces the current content with a new page if no Navigation stack is present (Windows Phone only).  Without a NavigationPage this method will not work (throws an exception) on Android and iOS.
        /// </summary>
        /// <param name="initAction">an action that can be performed on the viewmodel after it's constructed (e.g. init/assign values)</param>
        /// <returns></returns>
        public Task<TPageModel> PushAsync<TPageModel>(Action<TPageModel> initAction) where TPageModel : class, Abstractions.IPageModel
        {
            TPageModel vm;
            try
            {
                vm = this.PageModelFactory.GetPageModel<TPageModel>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, message: $"failed to resolve PageModel!!! {ex.Message}");
                throw ex;
            }

            var p = this.GetPage(vm);

            if (p == null)
                return Task.FromResult(vm);

            //if they gave us something to init with, init it!
            initAction?.Invoke(vm);

            return this.GetNavigationPageThrowIfNot().PushAsync(p)
                .ContinueWith((t) => vm);// here we are creating a continue that will just return the viewmodel

        }

        public Task<TPageModel> PushModalAsync<TPageModel>() where TPageModel : class, Abstractions.IPageModel
        {
            return this.PushModalAsync<TPageModel>(initAction: null);
        }

        public Task<TPageModel> PushModalAsync<TPageModel>(Action<TPageModel> initAction) where TPageModel : class, Abstractions.IPageModel
        {
            var vm = this.PageModelFactory.GetPageModel<TPageModel>();
            var p = this.GetPage(vm);

            if (p == null)
                return Task.FromResult(vm);

            //if they gave us something to init with, init it!
            initAction?.Invoke(vm);

            return _rootPage.Navigation.PushModalAsync(p)
              .ContinueWith((t) => vm);// here we are creating a continue that will just return the viewmodel
        }

        #endregion

        /// <summary>
        ///     Presents an alert dialog to the application user with an accept and a cancel button
        /// </summary>
        /// <param name="title">The title of the alert dialog.</param>
        /// <param name="message">The body text of the alert dialog.</param>
        /// <param name="accept">Text to be displayed on the 'Accept' button.</param>
        /// <param name="cancel">to be displayed on the 'Cancel' button.</param>
        /// <returns> </returns>
        public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            return await _rootPage.DisplayAlert(title, message, accept, cancel);
        }

        /// <summary>Presents an alert dialog to the application user with a single cancel button.</summary>
        /// <param name="title">The title of the alert dialog.</param>
        /// <param name="message">The body text of the alert dialog.</param>
        /// <param name="cancel">Text to be displayed on the 'Cancel' button.param>
        /// <returns> </returns>
        public async Task DisplayAlert(string title, string message, string cancel)
        {
            await _rootPage.DisplayAlert(title, message, cancel);
        }

        /// <summary>
        ///     Displays a native platform action sheet, allowing the application user to choose from several buttons.
        /// </summary>
        /// <param name="title">Title of the displayed action sheet. Must not be null.</param>
        /// <param name="cancel">Text to be displayed in the 'Cancel' button. Can be null to hide the cancel action.</param>
        /// <param name="destruction">Text to be displayed in the 'Destruct' button. Can be null to hide the destructive option.</param>
        /// <param name="buttons">Text labels for additional buttons. Must not be null.</param>
        /// <returns>An awaitable Task that displays an action sheet and returns an ActionSheetResponse indicating the seleted item and whether or not it was cancelled.</returns>
        /// <remarks>
        ///     Developers should be aware that Windows' line endings, CR-LF, only work on Windows systems, and are incompatible with iOS and Android. A particular consequence
        ///     of this is that characters that appear after a CR-LF, (For example, in the title.) may not be displayed on non-Windows platforms. Developers must use the correct
        ///     line endings for each of the targeted systems.
        /// </remarks>
        public async Task<ActionSheetResponse> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons)
        {
            var selectedItem = await _rootPage.DisplayActionSheet(title, cancel, destruction, buttons);
            var response = new ActionSheetResponse();

            if (selectedItem == cancel)
                response.WasCancelled = true;
            else
                response.SelectedItem = selectedItem;

            return response;
        }

        public void SetRootPage(Xamarin.Forms.Page page)
        {
            _rootPage = page;
            Xamarin.Forms.Application.Current.MainPage = _rootPage;
        }

        public void SetRootPage<TPageModel>() where TPageModel : class, Abstractions.IPageModel
        {
            var vm = this.PageModelFactory.GetPageModel<TPageModel>();
            var p = this.GetPage(vm);

            _rootPage = p;
            Xamarin.Forms.Application.Current.MainPage = _rootPage;
        }
        #endregion

        private Xamarin.Forms.Page GetPage<TPageModel>(TPageModel pageModel) where TPageModel : class, Abstractions.IPageModel
        {
            if (pageModel == null)
                throw new ArgumentNullException(paramName: nameof(pageModel));

            try
            {
                var newPage = this.ViewResolver.GetAndBindPage(pageModel);
                return newPage;
            }
            catch (Exception ex)
            {
                throw new ResolveMappedPageException(pageModel, innerException: ex);
            }
        }

        public Abstractions.IPageModelFactory PageModelFactory { get; set; }

        public Abstractions.IPageResolver ViewResolver { get; set; }

        public Abstractions.IPageModel CurrentPageModel
        {
            get
            {
                var nav = _rootPage as Xamarin.Forms.NavigationPage;
                if (nav != null)
                    return nav.CurrentPage?.BindingContext as Abstractions.IPageModel;

                var tabbed = _rootPage as Xamarin.Forms.TabbedPage;
                if (tabbed != null)
                    return tabbed.CurrentPage?.BindingContext as Abstractions.IPageModel;

                var carousel = _rootPage as Xamarin.Forms.CarouselPage;
                if (carousel != null)
                    return carousel.CurrentPage?.BindingContext as Abstractions.IPageModel;

                return _rootPage?.BindingContext as Abstractions.IPageModel;
            }
        }
    }
}
