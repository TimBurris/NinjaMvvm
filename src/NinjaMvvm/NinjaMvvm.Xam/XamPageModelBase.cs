using System;

namespace NinjaMvvm.Xam
{
    public class XamPageModelBase : NinjaMvvm.ViewModelBase, Xamvvm.IBasePageModel, Xamvvm.IPageVisibilityChange
    {

        /// <summary>
        /// if you override, you still need to call base.OnAppearing if you want the base class to signal that it ahs been bound (which kicks off reload)
        /// </summary>
        public virtual void OnAppearing()
        {
            var obj = this.ViewBound;
        }

        public virtual void OnDisappearing()
        {
        }

    }
}
