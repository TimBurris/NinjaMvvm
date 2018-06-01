﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NinjaMvvm.Xam.Samples.PageModels
{

    public class MainPageModel : NinjaMvvm.ViewModelBase
    {
        public MainPageModel()
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

        protected override async Task<bool> OnReloadDataAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(this.NumberOfSecondsReloadShouldRun * 1000, cancellationToken);
            this.StampMessage = $"Dataload just completed {DateTime.Now.ToString()}";
            return true;
        }


        #region Toggle Command

        public NinjaMvvm.Xam.RelayCommand ToggleCommand => new NinjaMvvm.Xam.RelayCommand((param) => this.Toggle());

        public void Toggle()
        {
            this.IsStampAllowed = !this.IsStampAllowed;
        }

        #endregion


        #region Stamp Command

        public NinjaMvvm.Xam.RelayCommand StampCommand => new NinjaMvvm.Xam.RelayCommand((param) => this.Stamp());

        public void Stamp()
        {
            this.StampMessage = $"Last stamp was @ {DateTime.Now.ToString()}";
        }

        #endregion

        #region DoLoad Command

        public NinjaMvvm.Xam.RelayCommand DoLoadCommand => new NinjaMvvm.Xam.RelayCommand((param) => this.DoLoad());

        public async void DoLoad()
        {
            await this.ReloadDataAsync();
        }

        #endregion

        #region CancelReload Command

        public NinjaMvvm.Xam.RelayCommand CancelReloadCommand => new NinjaMvvm.Xam.RelayCommand((param) => this.CancelReload());

        public void CancelReload()
        {
            this.CancelReloadDataAsync();
        }

        #endregion
    }
}