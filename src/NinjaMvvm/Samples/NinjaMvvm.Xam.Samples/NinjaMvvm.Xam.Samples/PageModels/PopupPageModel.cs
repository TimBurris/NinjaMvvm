using NinjaMvvm.Xam.Abstractions;

namespace NinjaMvvm.Xam.Samples.PageModels
{
    public class PopupPageModel : XamPageModelBase
    {
        private readonly INavigator _navigator;

        public PopupPageModel(Abstractions.INavigator navigator)
        {
            _navigator = navigator;
        }
        #region Close Command

        private RelayCommand _closeCommand;
        public RelayCommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                    _closeCommand = new RelayCommand((param) => this.Close());
                return _closeCommand;
            }
        }

        /// <summary>
        /// Executes the Close command 
        /// </summary>
        public void Close()
        {
            _navigator.PopModalAsync();
        }

        #endregion

    }
}
