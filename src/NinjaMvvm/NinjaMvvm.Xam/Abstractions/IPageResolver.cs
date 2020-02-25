namespace NinjaMvvm.Xam.Abstractions
{
    public interface IPageResolver
    {
        Xamarin.Forms.Page GetAndBindPage<TPageModel>(TPageModel pageModel) where TPageModel : class, IPageModel;
    }

}
