using System;
using Xamvvm;

namespace NinjaMvvm.Xam
{
    public class XamPageModelBase : NinjaMvvm.ViewModelBase, Abstractions.IPageModel, Xamvvm.IPageVisibilityChange
    {
        void IPageVisibilityChange.OnAppearing()
        {
            var obj = this.ViewBound;
            this.OnAppearing();
        }

        void IPageVisibilityChange.OnDisappearing()
        {
            this.OnDisappearing();
        }

        public virtual void OnAppearing()
        {
        }

        public virtual void OnDisappearing()
        {
        }
    }
}
