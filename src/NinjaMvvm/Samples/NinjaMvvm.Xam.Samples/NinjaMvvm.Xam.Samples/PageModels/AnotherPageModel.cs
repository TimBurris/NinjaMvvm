using NinjaMvvm.Xam.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NinjaMvvm.Xam.Samples.PageModels
{
    public class AnotherPageModel : XamPageModelBase
    {
        private readonly INavigator _navigator;

        public AnotherPageModel(Abstractions.INavigator navigator)
        {
            _navigator = navigator;
        }

        public void Init(string incomingValue)
        {
            this.IncomingValue = incomingValue;
        }

        public string IncomingValue
        {
            get { return GetField<string>(); }
            set { SetField(value); }
        }

        public string SelectedAlertActionText
        {
            get { return GetField<string>(); }
            set { SetField(value); }
        }

        #region GotoMain Command
        public NinjaMvvm.Xam.RelayCommand GotoMainCommand => new NinjaMvvm.Xam.RelayCommand((param) => this.GotoMain());

        public void GotoMain()
        {
            _navigator.PushAsync<MainPageModel>();
        }

        #endregion


        #region ShowAPopup Command

        public NinjaMvvm.Xam.RelayCommand ShowAPopupCommand => new NinjaMvvm.Xam.RelayCommand((param) => this.ShowAPopup());

        public void ShowAPopup()
        {
            _navigator.PushModalAsync<PopupPageModel>();
        }

        #endregion



        #region ShowAlert Command

        public NinjaMvvm.Xam.RelayCommand ShowAlertCommand => new NinjaMvvm.Xam.RelayCommand((param) => this.ShowAlertAsync());

        public async Task ShowAlertAsync()
        {
            var result = await _navigator.DisplayAlert(title: "AOL Notice", message: "You've got Mail", accept: "okay", cancel: "die");

            if (result)
            {
                this.SelectedAlertActionText = "you chose: okay";
            }
            else
            {
                this.SelectedAlertActionText = "you chose: die";
            }
        }

        #endregion


    }
}
