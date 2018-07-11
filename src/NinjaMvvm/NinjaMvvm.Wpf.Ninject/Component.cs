using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NinjaMvvm.Wpf.Ninject
{
    public static class Component
    {
        public static void Init<TNavigator>(IKernel container)
            where TNavigator : Abstractions.INavigator
        {
            container.Bind<Abstractions.IViewModelResolver>().To<ViewModelResolver>();

            container.Bind<Abstractions.INavigator>().To<TNavigator>();

        }
        public static IKernel Init<TNavigator>()
            where TNavigator : Abstractions.INavigator
        {
            var container = new StandardKernel();
            Init<TNavigator>(container);
            return container;
        }
    }
}
