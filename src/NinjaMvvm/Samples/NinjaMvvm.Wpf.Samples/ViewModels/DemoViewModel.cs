using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NinjaMvvm.Wpf.Samples.ViewModels
{
    public class DemoViewModel : WpfViewModelBase
    {
        public DemoViewModel()
        {
            this.ViewTitle = "This is my Bound view Title";
            this.NumberOfSecondsReloadShouldRun = 3;
        }

        public bool IsStampAllowed
        {
            get { return GetField<bool>(); }
            set { SetField(value); }
        }


        public string StampMessage
        {
            get { return GetField<string>(); }
            set { SetField(value); }
        }

        public string SomeTextValue
        {
            get { return GetField<string>(); }
            set { SetField(value); }
        }


        public int NumberOfSecondsReloadShouldRun
        {
            get { return GetField<int>(); }
            set { SetField(value); }
        }


        protected override void OnLoadDesignData()
        {
            this.StampMessage = "This message is from design data";
            this.IsStampAllowed = true;
        }

        protected override async Task<bool> OnReloadDataAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(this.NumberOfSecondsReloadShouldRun * 1000, cancellationToken);
            this.StampMessage = $"Dataload just completed {DateTime.Now.ToString()}";
            return true;
        }

        #region Toggle Command

        private RelayCommand _toggleCommand;
        public RelayCommand ToggleCommand
        {
            get
            {
                if (_toggleCommand == null)
                    _toggleCommand = new RelayCommand((param) => this.Toggle(), (param) => this.CanToggle());
                return _toggleCommand;
            }
        }

        public bool CanToggle()
        {
            return true;
        }

        /// <summary>
        /// Executes the Toggle command 
        /// </summary>
        public void Toggle()
        {
            this.IsStampAllowed = !this.IsStampAllowed;
        }

        #endregion

        #region Stamp Command

        private RelayCommand _stampCommand;
        public RelayCommand StampCommand
        {
            get
            {
                if (_stampCommand == null)
                    _stampCommand = new RelayCommand((param) => this.Stamp(), (param) => this.CanStamp());
                return _stampCommand;
            }
        }

        public bool CanStamp()
        {
            return this.IsStampAllowed;
        }

        /// <summary>
        /// Executes the Stamp command 
        /// </summary>
        public void Stamp()
        {
            this.StampMessage = $"Last stamp was @ {DateTime.Now.ToString()}";
        }

        #endregion

        #region ValidateMe Command

        private RelayCommand _validateMeCommand;
        public RelayCommand ValidateMeCommand
        {
            get
            {
                if (_validateMeCommand == null)
                    _validateMeCommand = new RelayCommand((param) => this.ValidateMe(), (param) => this.CanValidateMe());
                return _validateMeCommand;
            }
        }

        public bool CanValidateMe()
        {
            return true;
        }

        /// <summary>
        /// Executes the ValidateMe command 
        /// </summary>
        public void ValidateMe()
        {
            var validateResult = GetValidationResult();
            if (!validateResult.IsValid)
            {
                ShowErrors = true;
                this.StampMessage = String.Join(", ", validateResult.Errors);
                return;
            }
            this.StampMessage = "All Validation passed";
        }

        #endregion

        #region DoLoad Command

        private RelayCommand _doLoadCommand;
        public RelayCommand DoLoadCommand
        {
            get
            {
                if (_doLoadCommand == null)
                    _doLoadCommand = new RelayCommand((param) => this.DoLoadAsync(), (param) => this.CanDoLoad());
                return _doLoadCommand;
            }
        }

        public bool CanDoLoad()
        {
            return true;
        }

        /// <summary>
        /// Executes the DoLoad command 
        /// </summary>
        public async void DoLoadAsync()
        {
            await this.ReloadDataAsync();
        }

        #endregion

        #region CancelReload Command

        private RelayCommand _cancelReloadCommand;
        public RelayCommand CancelReloadCommand
        {
            get
            {
                if (_cancelReloadCommand == null)
                    _cancelReloadCommand = new RelayCommand((param) => this.CancelReload(), (param) => this.CanCancelReload());
                return _cancelReloadCommand;
            }
        }

        public bool CanCancelReload()
        {
            return true;
        }

        /// <summary>
        /// Executes the CancelReload command 
        /// </summary>
        public void CancelReload()
        {
            this.CancelReloadDataAsync();
        }

        #endregion

        #region Validation

        class DemoViewModelValidator : AbstractValidator<DemoViewModel>
        {
            public DemoViewModelValidator()
            {
                RuleFor(obj => obj.SomeTextValue)
                    .NotEmpty()
                    .Length(min: 1, max: 4);
            }
        }

        protected override IValidator GetValidator()
        {
            return new DemoViewModelValidator();
        }

        #endregion
    }
}
