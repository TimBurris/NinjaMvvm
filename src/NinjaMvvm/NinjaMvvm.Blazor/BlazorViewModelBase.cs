using System;
using System.Threading;
using System.Threading.Tasks;

namespace NinjaMvvm.Blazor
{
    public class BlazorViewModelBase : NinjaMvvm.ViewModelBase
    {

        public event EventHandler NotifyStateChanged;
        public BlazorViewModelBase()
        {
            //default
            this.NotifyStateChangedWhenPropertyChanged = true;
            this.NotifyStateChangedAfterSuccessfulReload = true;
            this.NotifyStateChangedAfterFailedReload = true;
        }

        protected virtual void OnNotifyStateChanged()
        {
            NotifyStateChanged?.Invoke(this, EventArgs.Empty);

        }

        protected override async Task<bool> OnReloadDataAsync(CancellationToken cancellationToken)
        {

            var result = await base.OnReloadDataAsync(cancellationToken);

            NotifyStateChanged?.Invoke(this, EventArgs.Empty);
            return result;
        }

        protected override void OnReloadDataFailed()
        {
            base.OnReloadDataFailed();

            if (NotifyStateChangedAfterFailedReload)
            {
                NotifyStateChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        protected override void OnReloadDataSucceeded()
        {
            base.OnReloadDataSucceeded();

            if (NotifyStateChangedAfterSuccessfulReload)
            {
                NotifyStateChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Whether or not <see cref="NotifyStateChanged"/> should be fired after a successful <see cref="OnReloadDataAsync(CancellationToken)"/>
        /// </summary>
        /// <remarks>default: true </remarks>
        protected bool NotifyStateChangedAfterSuccessfulReload { get; set; }

        /// <summary>
        /// Whether or not <see cref="NotifyStateChanged"/> should be fired after a unsuccessful <see cref="OnReloadDataAsync(CancellationToken)"/>
        /// </summary>
        /// <remarks>default: true </remarks>
        protected bool NotifyStateChangedAfterFailedReload { get; set; }

        private bool _notifyStateChangedWhenPropertyChanged;
        /// <summary>
        /// specifies whehter or not OnPropertyChanged should automatically invoke <see cref="NotifyStateChanged"/>
        /// </summary>
        /// <remarks>default: true 
        /// Even when if true, this will be supressed while <see cref="IsReloading"/>
        /// </remarks>
        protected bool NotifyStateChangedWhenPropertyChanged
        {
            get => _notifyStateChangedWhenPropertyChanged; set
            {
                if (value != _notifyStateChangedWhenPropertyChanged)
                {
                    _notifyStateChangedWhenPropertyChanged = value;

                    if (value)
                    {
                        this.PropertyChanged += BlazorViewModelBase_PropertyChanged;
                    }
                    else
                    {
                        this.PropertyChanged -= BlazorViewModelBase_PropertyChanged;
                    }
                }
            }
        }

        private void BlazorViewModelBase_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!this.IsReloading)
            {
                NotifyStateChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

}
