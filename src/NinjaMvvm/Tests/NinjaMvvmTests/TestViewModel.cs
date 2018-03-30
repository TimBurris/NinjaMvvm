using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NinjaMvvmTests
{
    public class DesignModeTestViewModel : TestViewModel
    {
        protected override bool IsInDesignMode()
        {
            return true;
        }
    }

    public class TestViewModel : NinjaMvvm.ViewModelBase
    {
        public bool CalledLoadDesignData { get; set; }

        protected override void OnLoadDesignData()
        {
            this.CalledLoadDesignData = true;
        }

        protected override bool IsInDesignMode()
        {
            return false;
        }

        public bool Exposed_LoadWhenBound
        {
            get { return this.LoadWhenBound; }
            set { this.LoadWhenBound = value; }
        }

        protected override Task<bool> OnReloadDataAsync(CancellationToken cancellationToken)
        {
            return this.Exposed_OnReloadDataAsync(cancellationToken);
        }


        public virtual async Task<bool> Exposed_OnReloadDataAsync(CancellationToken cancellationToken)
        {
            return true;
        }
        public void Exposed_CancelReloadDataAsync()
        {
            this.CancelReloadDataAsync();
        }
    }
}
