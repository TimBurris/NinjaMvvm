using System;
using System.Collections.Generic;
using System.Text;
using Xamvvm;
using Unity;

namespace NinjaMvvm.Xam
{
    public class UnityXamvvmIoC : Xamvvm.IXamvvmIoC
    {
        public void RegisterMultiInstance(Type registerType, Type registerImplementation)
        {
            UnityIoC.IoCContainer.RegisterType(registerType, registerImplementation);
        }
        void IXamvvmIoC.RegisterMultiInstance<RegisterType, RegisterImplementation>()
        {
            UnityIoC.IoCContainer.RegisterType<RegisterType, RegisterImplementation>();
        }

        public void RegisterSingleton(Type registerType, Type registerImplementation)
        {
            UnityIoC.IoCContainer.RegisterSingleton(registerType, registerImplementation);
        }

        void IXamvvmIoC.RegisterSingleton<RegisterType, RegisterImplementation>()
        {
            UnityIoC.IoCContainer.RegisterSingleton<RegisterType, RegisterImplementation>();
        }

        public void RegisterSingleton<RegisterType>(RegisterType instance) where RegisterType : class
        {
            UnityIoC.IoCContainer.RegisterInstance<RegisterType>(instance);
        }

        public TResolveType Resolve<TResolveType>() where TResolveType : class
        {
            return UnityIoC.IoCContainer.Resolve<TResolveType>();
        }

        public object Resolve(Type resolveType)
        {
            return UnityIoC.IoCContainer.Resolve(resolveType);
        }
    }
}
