using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace NinjaMvvm.Wpf.Unity
{
    public class ViewModelResolver : Abstractions.IViewModelResolver
    {
        private readonly IUnityContainer _container;

        public ViewModelResolver(IUnityContainer container)
        {
            this._container = container;
        }
        public TViewModel Resolve<TViewModel>()
        {
            return _container.Resolve<TViewModel>();
        }
    }
}
