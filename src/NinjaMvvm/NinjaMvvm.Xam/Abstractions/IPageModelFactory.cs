namespace NinjaMvvm.Xam.Abstractions
{
    public interface IPageModelFactory
    {
        TPageModel GetPageModel<TPageModel>();// where TPageModel : Contracts.IPageModel;
    }

}
