using System;
using Unity;
using Unity.Injection;

namespace NinjaMvvm.Wpf.Unity
{
    public static class Component
    {
        public static void Init<TNavigator>(IUnityContainer container)
            where TNavigator : Abstractions.INavigator
        {
            container.RegisterInstance<IUnityContainer>(container);
            container.RegisterType<Abstractions.IViewModelResolver, ViewModelResolver>();

            container.RegisterType<Abstractions.INavigator, TNavigator>();

        }
        public static IUnityContainer Init<TNavigator>()
            where TNavigator : Abstractions.INavigator
        {
            var container = new UnityContainer();
            Init<TNavigator>(container);
            return container;
        }
    }
}
