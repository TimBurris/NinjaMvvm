using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaMvvm.Wpf
{
    public class Navigator<TMainWindow, TDialogWindow> : Abstractions.INavigator
        where TDialogWindow : System.Windows.Window, new()
        where TMainWindow : System.Windows.Window
    {
        private static List<DialogView> _dialogViews = new List<DialogView>();
        private readonly Abstractions.IViewModelResolver _viewModelResolver;

        public Navigator(Abstractions.IViewModelResolver viewModelResolver)
        {
            this._viewModelResolver = viewModelResolver;
        }

        private TMainWindow GetWindow()
        {
            return (TMainWindow)System.Windows.Application.Current.MainWindow;
        }

        #region INavigator Implementation
        public void CloseDialog(ViewModelBase viewModel)
        {

            var dialog = _dialogViews.SingleOrDefault(x => x.ViewModel == viewModel);

            if (dialog == null)
            {
                //_logger.Warn($@"A ViewModel was requested to be closed but not dialog corresponding the ViewModel was in our list of open dialogs.
                //            While not an error, probably means someone is either closing something that did never actually opened, closing something multiple times, or closed a dialog that was not opened by Navigator.
                //            ViewModel was asked to be closed was: {viewModel?.GetType()?.FullName}");
            }
            else
                CloseDialogView(dialog);
        }

        public TViewModel NavigateTo<TViewModel>() where TViewModel : ViewModelBase
        {
            return this.NavigateTo<TViewModel>(initAction: null);
        }

        public TViewModel NavigateTo<TViewModel>(Action<TViewModel> initAction) where TViewModel : ViewModelBase
        {
            var vm = this.CreateViewModel(initAction);

            this.BindViewModelToMainWindow(vm, this.GetWindow());
            object obj = vm.ViewBound;

            return vm;
        }

        public TViewModel ShowDialog<TViewModel>() where TViewModel : ViewModelBase
        {
            return ShowDialog<TViewModel>(initAction: null);
        }

        public TViewModel ShowDialog<TViewModel>(Action<TViewModel> initAction) where TViewModel : ViewModelBase
        {
            var viewModel = this.CreateViewModel(initAction);

            var dialogWindow = new TDialogWindow();
            _dialogViews.Add(new DialogView() { ViewModel = viewModel, Window = dialogWindow });

            dialogWindow.Closed += DialogWindow_Closed;

            dialogWindow.Owner = this.GetWindow();//TODO: maybe the owner should be the "current dialog"?

            this.BindViewModelToDialogWindow(viewModel, dialogWindow);
            object obj = viewModel.ViewBound;

            dialogWindow.ShowDialog();

            return viewModel;
        }

        #endregion

        /// <summary>
        /// assigns the viewmodel to the DataContext of the MainWindow.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="mainWindow"></param>
        /// <remarks>If you would rather not change the window's datacontext, you can override this method and maybe just assign it to a contentpresenter or something</remarks>
        protected virtual void BindViewModelToMainWindow<TViewModel>(TViewModel viewModel, TMainWindow mainWindow)
            where TViewModel : ViewModelBase
        {
            mainWindow.DataContext = viewModel;
        }

        /// <summary>
        /// assigns the viewmodel to the DataContext of the DialogWindow.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="mainWindow"></param>
        /// <remarks>If you would rather not change the window's datacontext, you can override this method and maybe just assign it to a contentpresenter or something</remarks>
        protected virtual void BindViewModelToDialogWindow<TViewModel>(TViewModel viewModel, TDialogWindow dialogWindow)
            where TViewModel : ViewModelBase
        {
            dialogWindow.DataContext = viewModel;
        }

        private TViewModel CreateViewModel<TViewModel>(Action<TViewModel> initAction) where TViewModel : ViewModelBase
        {
            var vm = _viewModelResolver.Resolve<TViewModel>();

            //if they gave us something to init with, init it!
            initAction?.Invoke(vm);

            return vm;
        }

        private void DialogWindow_Closed(object sender, EventArgs e)
        {
            var window = (TDialogWindow)sender;
            var dialog = _dialogViews.SingleOrDefault(x => x.Window == window);

            if (dialog == null)
            {
                //_logger.Warn($@"A dialog was closed, but that dialog was not found in our list of open dialogs.  
                //            The windows that closed was: {sender.GetType()?.FullName}");
            }
            else
            {
                CloseDialogView(dialog);
            }
        }

        private void CloseDialogView(DialogView dialog)
        {
            dialog.Window.Close();
            dialog.Window.Closed -= DialogWindow_Closed;
            _dialogViews.Remove(dialog);
        }

        private class DialogView
        {
            public ViewModelBase ViewModel { get; set; }
            public TDialogWindow Window { get; set; }
        }
    }
}
