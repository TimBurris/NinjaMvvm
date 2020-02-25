using NinjaMvvm.Xam.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NinjaMvvm.Xam
{
    public class XamvvmPageResolver : Abstractions.IPageResolver
    {
        public Xamarin.Forms.Page GetAndBindPage<TPageModel>(TPageModel pageModel) where TPageModel : class, IPageModel
        {
            return Xamvvm.XamvvmCore.CurrentFactory.GetPageAsNewInstance(pageModel) as Xamarin.Forms.Page;
        }
    }
}
