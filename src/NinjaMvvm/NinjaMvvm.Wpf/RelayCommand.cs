using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaMvvm.Wpf
{
    /// <summary>
    /// A command whose sole purpose is to 
    /// relay its functionality to other
    /// objects by invoking delegates. The
    /// default return value for the CanExecute
    /// method is 'true'.
    /// </summary>
    public class RelayCommand<T> : RelayCommandBase<T>
    {
        public RelayCommand(Action<T> execute) : base(execute) { }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute) : base(execute, canExecute) { }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute, string label) : base(execute, canExecute, label) { }

        public override event EventHandler CanExecuteChanged
        {
            add { System.Windows.Input.CommandManager.RequerySuggested += value; }
            remove { System.Windows.Input.CommandManager.RequerySuggested -= value; }
        }
    }

    /// <summary>
    /// provides a generic object implementation of <see cref="RelayCommandBase&lt;T&gt;"/>
    /// </summary>
    public class RelayCommand : RelayCommand<object>
    {
        public RelayCommand(Action<object> execute) : base(execute) { }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute) : base(execute, canExecute) { }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute, string label) : base(execute, canExecute, label) { }
    }
}
