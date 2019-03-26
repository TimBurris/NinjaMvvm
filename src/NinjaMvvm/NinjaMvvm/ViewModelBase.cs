using System;
using System.Linq;
using System.Threading.Tasks;

namespace NinjaMvvm
{
    public abstract class ViewModelBase : NotificationBase
    {
        private object _reloadCheckLock = new object();

        public ViewModelBase()
        {
            if (this.IsInDesignMode())
            {
                this.OnLoadDesignData();
                return;
            }

            this.LoadWhenBound = true;
            this.PreventDuplicateReload = true;
        }

        /// <summary>
        /// Common utility property that can be used by UI to know the viewmodel's displayname/title
        /// </summary>
        public string ViewTitle
        {
            get { return GetField<string>(); }
            set { SetField(value); }
        }

        /// <summary>
        /// this property is used to signal to the viewModel that it's corresponding ui (control) has been rendered
        ///     Typically you simply bind this property in your UI
        ///     alternately, in a custom control you can simple "get" the value to kick off the process
        /// </summary>
        public object ViewBound
        {
            get
            {
                //don't fire in design mode
                if (this.IsInDesignMode())
                    return null;

                if (this.LoadWhenBound)
                    this.ReloadDataAsync();

                return null;
            }
            set { }
        }
        public void ViewHasBeenUnbound()
        {
            this.OnUnloaded();
        }
        /// <summary>
        /// if using the viewmodel on a designer, override this method and do a test to see if you are in design mode to prevent loading.
        /// 
        /// </summary>
        /// <returns>false, unless overridden by a derrived class</returns>
        protected virtual bool IsInDesignMode()
        {
            return false;
        }

        /// <summary>
        /// will be called when the ViewModel is removed from it's bound view
        /// </summary>
        protected virtual void OnUnloaded() { }

        /// <summary>
        /// will be called when viewmodel is determined to be in design mode, override to provide designtime data <seealso cref="IsInDesignMode"/>
        /// </summary>
        protected virtual void OnLoadDesignData() { }


        /// <summary>
        /// specifies whethor not <see cref="ReloadDataAsync"/> will automatically fire when page/control is bound
        /// </summary>
        /// <remarks>
        /// this requires consumer to utilize the <see cref="ViewBound"/> property in page/view/control
        /// </remarks>
        protected bool LoadWhenBound { get; set; }

        /// <summary>
        /// Common utility property that can be used by UI to know the viewmodel is busy doing some work.  
        /// </summary>
        /// <remarks>
        /// Isbusy is automatically turned on and off during <see cref="ReloadDataAsync"/>
        /// </remarks>
        public bool IsBusy
        {
            get { return GetField<bool>(); }
            set { SetField(value); }
        }

        /// <summary>
        /// indicates whether viewmodel is currently running <see cref="ReloadDataAsync"/>
        /// </summary>
        /// <remarks>
        /// This differes from <see cref="IsBusy"/> in that IsBusy can indicate any type of busy, where IsReloading is specifically a ViewModel Reload
        /// </remarks>
        public bool IsReloading
        {
            get { return GetField<bool>(); }
            set { SetField(value); }
        }

        /// <summary>
        /// if true, will abandon reload if already in process of reloading
        /// </summary>
        public bool PreventDuplicateReload
        {
            get { return GetField<bool>(); }
            set { SetField(value); }
        }

        /// <summary>
        /// Indicates whether the last call to <see cref="ReloadDataAsync"/> was cancelled.
        /// </summary>
        public bool LoadCancelled
        {
            get { return GetField<bool>(); }
            private set { SetField(value); }
        }

        /// <summary>
        /// Indicates whether the last call to <see cref="ReloadDataAsync"/> resulted in a Fail.  Cancel is not considered a fail
        /// </summary>
        public bool LoadFailed
        {
            get { return GetField<bool>(); }
            private set { SetField(value); }
        }

        /// <summary>
        /// Returns the Exception thrown by the last call to <see cref="ReloadDataAsync"/>.
        /// </summary>
        public Exception LoadFailedException
        {
            get { return GetField<Exception>(); }
            set { SetField(value); }
        }

        #region Reload Data Functionality
        private System.Threading.CancellationTokenSource _currentCancellationTokenSource;


        /// <summary>
        /// override this method to perform asyncronous data loading.  may be called when the viewmodel is bound <see cref="LoadWhenBound"/> and on explicit calls to <see cref="ReloadDataAsync"/>
        /// </summary>
        /// <param name="cancellationToken">Cancellation token used to cancel your async loading methods</param>
        /// <returns></returns>
        protected async virtual Task<bool> OnReloadDataAsync(System.Threading.CancellationToken cancellationToken) { return true; }

        /// <summary>
        /// will be called when an error, not a TaskCanceledException, is thrown during <see cref="OnReloadDataAsync"/>, consumer can get the exception by checking the <see cref="LoadFailedException"/> property
        /// </summary>
        protected virtual void OnReloadDataFailed() { }

        /// <summary>
        /// Intitiates an error safe asyncronous invocation of <see cref="OnReloadDataAsync(System.Threading.CancellationToken)"/>
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// This method will utilize <see cref="IsBusy"/>, <see cref="LoadCancelled"/>, <see cref="LoadFailed"/>, <see cref="LoadFailedException"/>, and can be cancelled by using <see cref="CancelReloadDataAsync"/>
        /// </remarks>
        public async System.Threading.Tasks.Task ReloadDataAsync()
        {
            lock (_reloadCheckLock)
            {
                if (this.IsReloading && this.PreventDuplicateReload)
                {
                    return;
                }
                this.IsBusy = true;
                this.IsReloading = true;
            }

            if (_currentCancellationTokenSource != null)
            {
                _currentCancellationTokenSource.Cancel();
                _currentCancellationTokenSource = null;
            }

            //we need a local copy so that after await, we can check the token cacellation while knowing multiple threads would expect different answer to whether or not they were cancelled
            var localCancellationTokenSource = new System.Threading.CancellationTokenSource();
            _currentCancellationTokenSource = localCancellationTokenSource;

            bool wasSuccessful = false;
            bool wasCancelled = false;
            try
            {

                var result = await OnReloadDataAsync(localCancellationTokenSource.Token);
                wasCancelled = localCancellationTokenSource.IsCancellationRequested;
                //if cancel we still say successful because it was not a fail/error
                wasSuccessful = wasCancelled || result;
            }
            catch (System.Threading.Tasks.TaskCanceledException)
            {
                wasCancelled = true;
                //if cancel we still say successful because it was not a fail/error
                wasSuccessful = true;
            }
            catch (Exception ex)
            {
                this.LoadFailedException = ex;
                this.OnReloadDataFailed();
            }
            finally
            {
                this.IsReloading = false;
                this.IsBusy = false;
                this.LoadFailed = !wasSuccessful;
                this.LoadCancelled = wasCancelled;
            }
        }

        /// <summary>
        /// Used to cancel an ongoing <see cref="ReloadDataAsync"/>
        /// </summary>
        /// <remarks>
        /// If <see cref="ReloadDataAsync"/> has not been called, this method will open a black hole and swallow you for needlessly wasting time.
        /// </remarks>
        public void CancelReloadDataAsync()
        {
            _currentCancellationTokenSource?.Cancel();
        }

        #endregion

    }
}
