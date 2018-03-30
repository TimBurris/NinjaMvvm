using System;
using System.Collections.Generic;
using System.Text;

namespace NinjaMvvm
{
    /// <summary>
    /// A command whose sole purpose is to 
    /// relay its functionality to other
    /// objects by invoking delegates. The
    /// default return value for the CanExecute
    /// method is 'true'.
    /// </summary>
    public abstract class RelayCommandBase<T> : System.Windows.Input.ICommand
    {
        public RelayCommandBase(Action<T> execute) : this(execute, null) { }

        public RelayCommandBase(Action<T> execute, Predicate<T> canExecute) : this(execute, canExecute, label: "") { }

        public RelayCommandBase(Action<T> execute, Predicate<T> canExecute, string label)
        {
            _Execute = execute;
            _CanExecute = canExecute;

            Label = label;
        }

        readonly Action<T> _Execute = null;
        readonly Predicate<T> _CanExecute = null;

        public string Label { get; set; }

        public void Execute(object parameter)
        {
            _Execute((T)parameter);
        }

        public bool CanExecute(object parameter)
        {
            //they can execute if it has not been forcefully disabled and their canexecute code says yes
            return (_CanExecute == null ? true : _CanExecute((T)parameter));
        }
        public abstract event EventHandler CanExecuteChanged;
    }

    /// <summary>
    /// provides a generic object implementation of <see cref="RelayCommandBase&lt;T&gt;"/>
    /// </summary>
    public abstract class RelayCommandBase : RelayCommandBase<object>
    {
        public RelayCommandBase(Action<object> execute) : this(execute, null) { }

        public RelayCommandBase(Action<object> execute, Predicate<object> canExecute) : this(execute, canExecute, "") { }

        public RelayCommandBase(Action<object> execute, Predicate<object> canExecute, string label) : base(execute, canExecute, label) { }
    }
}
