using System;

namespace NinjaMvvm.Xam.Abstractions
{
    public interface IPageResolver
    {
        Xamarin.Forms.Page GetAndBindPage<TPageModel>(TPageModel pageModel) where TPageModel : class, IPageModel;
        Xamarin.Forms.Page GetAndBindPage<TPageModel>(Action<TPageModel> initAction = null) where TPageModel : class, IPageModel;
    }

}
