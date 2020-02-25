using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NinjaMvvm.Xam.Abstractions
{
    public interface INavigator
    {
        /// <summary>
        /// Pops the most recent page off the Navigation stack.  Havent tested what this method does on Windows Phone without a Navigation Page.
        /// </summary>
        /// <returns></returns>
        Task<IPageModel> PopAsync();

        /// <summary>
        /// Pops the most recent Model context and returns to the most recent Navigation context.
        /// </summary>
        /// <returns></returns>
        Task<IPageModel> PopModalAsync();

        /// <summary>
        /// pops to the first page in the Navigation stack.  Used very commonly with InsertPageBefore to change the root page for an application.
        /// </summary>
        /// <returns></returns>
        Task PopToRootAsync();


        /// <summary>
        /// Pushes a new page onto the navigation stack, or replaces the current content with a new page if no Navigation stack is present (Windows Phone only).  Without a NavigationPage this method will not work (throws an exception) on Android and iOS.
        /// </summary>
        /// <returns></returns>
        Task<TPageModel> PushAsync<TPageModel>() where TPageModel : class, IPageModel;

        /// <summary>
        /// Pushes a new page onto the navigation stack, or replaces the current content with a new page if no Navigation stack is present (Windows Phone only).  Without a NavigationPage this method will not work (throws an exception) on Android and iOS.
        /// </summary>
        /// <param name="initAction">an action that can be performed on the viewmodel after it's constructed (e.g. init/assign values)</param>
        /// <returns></returns>
        Task<TPageModel> PushAsync<TPageModel>(Action<TPageModel> initAction) where TPageModel : class, IPageModel;

        /// <summary>
        /// Push a page into a modal context.  This will create a new, independent, Navigation context within the application.  The modal that is created can be dismissed with a hardware back button; there appears to no way to stop this functionality.  On Android and Windows Phone there really is no difference, to the user, between this method and PushAsync, on iOS however you will see the traditional cover vertical animation
        /// </summary>
        /// <returns></returns>
        Task<TPageModel> PushModalAsync<TPageModel>() where TPageModel : class, IPageModel;

        /// <summary>
        /// Push a page into a modal context.  This will create a new, independent, Navigation context within the application.  The modal that is created can be dismissed with a hardware back button; there appears to no way to stop this functionality.  On Android and Windows Phone there really is no difference, to the user, between this method and PushAsync, on iOS however you will see the traditional cover vertical animation
        /// </summary>
        /// <param name="initAction">an action that can be performed on the viewmodel after it's constructed (e.g. init/assign values)</param>
        /// <returns></returns>
        Task<TPageModel> PushModalAsync<TPageModel>(Action<TPageModel> initAction) where TPageModel : class, IPageModel;

        /// <summary>
        ///  Presents an alert dialog to the application user with a single cancel button.
        /// </summary>
        /// <param name="title">The title of the alert dialog.</param>
        /// <param name="message">The body text of the alert dialog.</param>
        /// <param name="accept">Text to be displayed on the 'Accept' button.</param>
        /// <param name="cancel">Text to be displayed on the 'Cancel' button.</param>
        /// <returns></returns>
        Task<bool> DisplayAlert(string title, string message, string accept, string cancel);

        /// <summary>
        ///  Presents an alert dialog to the application user with a single cancel button.
        /// </summary>
        /// <param name="title">The title of the alert dialog.</param>
        /// <param name="message">The body text of the alert dialog.</param>
        /// <param name="cancel">Text to be displayed on the 'Cancel' button.</param>
        /// <returns></returns>
        Task DisplayAlert(string title, string message, string cancel);

        /// <summary>
        /// used at begining of app to assign the base page, all navigation will drive from there
        /// </summary>
        /// <param name="page"></param>
        void SetRootPage(Xamarin.Forms.NavigationPage page);

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
        Task<ActionSheetResponse> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons);

    }

}
