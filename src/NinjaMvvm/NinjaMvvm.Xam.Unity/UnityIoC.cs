using System;
using Unity;

namespace NinjaMvvm.Xam
{
    public static class UnityIoC
    {
        internal static IUnityContainer IoCContainer;
        public static void Init(Unity.IUnityContainer container)
        {
            IoCContainer = container;
            container.RegisterType<Abstractions.INavigator, Navigator>();
            container.RegisterType<Abstractions.IPageResolver, XamvvmPageResolver>();
            container.RegisterType<Abstractions.IPageModelFactory, UnityPageModelFactory>();

            container.RegisterSingleton<Xamvvm.IXamvvmIoC, UnityXamvvmIoC>();

            //here we tell xamvvm that we want it to use you unity resolver instead of xamvvm's built in(tinyioc)
            Xamvvm.XamvvmIoC.Instance = container.Resolve<Xamvvm.IXamvvmIoC>();
        }
    }
}
