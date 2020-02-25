using Unity;

namespace NinjaMvvm.Xam
{
    public class UnityPageModelFactory : Abstractions.IPageModelFactory
    {
        public TPageModel GetPageModel<TPageModel>()
        {
            return UnityIoC.IoCContainer.Resolve<TPageModel>();
        }
    }
}
