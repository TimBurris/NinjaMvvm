using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NinjaMvvm.Wpf.Samples.ViewModels
{
    public class HomeViewModel : WpfViewModelBase
    {
        private readonly Abstractions.INavigator _navigator;

        public HomeViewModel() { }//designer only 
        public HomeViewModel(Wpf.Abstractions.INavigator navigator)
        {
            this.ViewTitle = "Welcome Home";
            this._navigator = navigator;
        }


        #region ShowDemoAsDialog Command

        public NinjaMvvm.Wpf.RelayCommand ShowDemoAsDialogCommand => new NinjaMvvm.Wpf.RelayCommand((param) => this.ShowDemoAsDialog());

        public void ShowDemoAsDialog()
        {
            _navigator.ShowDialog<DemoViewModel>(initAction: (vm) => vm.SomeTextValue = "inited text value", modal: false);
        }

        #endregion
        #region ShowDemoAsModalDialog Command

        public NinjaMvvm.Wpf.RelayCommand ShowDemoAsModalDialogCommand => new NinjaMvvm.Wpf.RelayCommand((param) => this.ShowDemoAsModalDialog());

        public void ShowDemoAsModalDialog()
        {
            _navigator.ShowDialog<DemoViewModel>(initAction: (vm) => vm.SomeTextValue = "inited text value");
        }

        #endregion



        #region NavigateToDemo Command

        public NinjaMvvm.Wpf.RelayCommand NavigateToDemoCommand => new NinjaMvvm.Wpf.RelayCommand((param) => this.NavigateToDemo());

        public void NavigateToDemo()
        {
            _navigator.NavigateTo<DemoViewModel>(initAction: (vm) => vm.SomeTextValue = "inited text value");
        }

        #endregion

    }
}
