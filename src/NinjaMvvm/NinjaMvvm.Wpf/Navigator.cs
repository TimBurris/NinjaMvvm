using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaMvvm.Wpf
{
    public class Navigator<TMainWindow, TDialogWindow> : Abstractions.INavigator
        where TDialogWindow : System.Windows.Window, new()
        where TMainWindow : System.Windows.Window, Abstractions.IMainWindow
    {
        private TMainWindow _window;
        private readonly Abstractions.IViewModelResolver _viewModelResolver;
        private readonly Common.Logging.ILog _logger;

        public Navigator(Abstractions.IViewModelResolver viewModelResolver, Common.Logging.ILog logger)
        {
            _window = (TMainWindow)System.Windows.Application.Current.MainWindow;
            this._viewModelResolver = viewModelResolver;
            this._logger = logger;
        }

        private List<DialogView> _dialogViews = new List<DialogView>();

        public void CloseDialog(ViewModelBase viewModel)
        {
            var dialog = _dialogViews.SingleOrDefault(x => x.ViewModel == viewModel);

            if (dialog == null)
            {
                _logger.Warn($@"A ViewModel was requested to be closed but not dialog corresponding the ViewModel was in our list of open dialogs.
                            While not an error, probably means someone is either closing something that did never actually opened, closing something multiple times, or closed a dialog that was not opened by Navigator.
                            ViewModel was asked to be closed was: {viewModel?.GetType()?.FullName}");
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
            _window.AssignToContent(vm);

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

            dialogWindow.DataContext = viewModel;
            dialogWindow.Closed += DialogWindow_Closed;

            dialogWindow.Owner = _window;//TODO: maybe the owner should be the "current dialog"?
            dialogWindow.ShowDialog();

            return viewModel;
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
                _logger.Warn($@"A dialog was closed, but that dialog was not found in our list of open dialogs.  
                            The windows that closed was: {sender.GetType()?.FullName}");
            else
                CloseDialogView(dialog);
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
