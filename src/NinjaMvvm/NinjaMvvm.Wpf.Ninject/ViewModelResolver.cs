using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace NinjaMvvm.Wpf.Ninject
{
    public class ViewModelResolver : Abstractions.IViewModelResolver
    {
        private readonly IKernel _container;

        public ViewModelResolver(IKernel container)
        {
            this._container = container;
        }
        public TViewModel Resolve<TViewModel>()
        {
            return _container.Get<TViewModel>();
        }
    }
}
