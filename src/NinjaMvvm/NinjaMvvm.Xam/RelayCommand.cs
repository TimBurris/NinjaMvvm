using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NinjaMvvm.Xam
{
    public class RelayCommand : Xamvvm.BaseCommand
    {
        public RelayCommand(Action<object> execute)
        : base(execute)
        { }

        //CanExecute does not seem to work in xamarin. even if you manaully call the RaiseCanExecutechanged.
        // for that reason i've commented out those constructors so that you don't get something that should work, but doesn't

        //public RelayCommand(Action<object> execute, Func<bool> canExecute)
        //    : base(new Func<object, Task>(async (obj) => execute(obj)), canExecute)
        //{
        //}
        public RelayCommand(Func<object, Task> execute)
            : base(execute)
        { }


        //CanExecute does not seem to work in xamarin. even if you manaully call the RaiseCanExecutechanged.
        // for that reason i've commented out those constructors so that you don't get something that should work, but doesn't
        //public RelayCommand(Func<object, Task> execute, Func<bool> canExecute)
        //    : base(execute, canExecute)
        //{ }
    }

    public class RelayCommand<T> : Xamvvm.BaseCommand<T>
    {
        public RelayCommand(Action<T> execute)
            : base(execute)
        {
        }

        //CanExecute does not seem to work in xamarin. even if you manaully call the RaiseCanExecutechanged.
        // for that reason i've commented out those constructors so that you don't get something that should work, but doesn't
        //public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
        //    : base(new Func<T, Task>(async (obj) => execute(obj)), canExecute)
        //{
        //}

        public RelayCommand(Func<T, Task> execute)
            : base(execute)
        {
        }

        //CanExecute does not seem to work in xamarin. even if you manaully call the RaiseCanExecutechanged.
        // for that reason i've commented out those constructors so that you don't get something that should work, but doesn't
        //public RelayCommand(Func<T, Task> execute, Func<T, bool> canExecute)
        //    : base(execute, canExecute)
        //{
        //}
    }
}
