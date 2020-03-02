using NinjaMvvm.Xam.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NinjaMvvm.Xam
{
    public class XamvvmPageResolver : Abstractions.IPageResolver
    {
        private readonly IPageModelFactory _pageModelFactory;

        public XamvvmPageResolver(IPageModelFactory pageModelFactory)
        {
            _pageModelFactory = pageModelFactory;
        }
        public Xamarin.Forms.Page GetAndBindPage<TPageModel>(TPageModel pageModel) where TPageModel : class, IPageModel
        {
            return Xamvvm.XamvvmCore.CurrentFactory.GetPageAsNewInstance(pageModel) as Xamarin.Forms.Page;
        }

        public Xamarin.Forms.Page GetAndBindPage<TPageModel>(Action<TPageModel> initAction = null) where TPageModel : class, IPageModel
        {
            var pageModel = _pageModelFactory.GetPageModel<TPageModel>();

            initAction?.Invoke(pageModel);

            return this.GetAndBindPage(pageModel);
        }
    }
}
