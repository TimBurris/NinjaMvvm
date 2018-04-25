using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NinjaMvvm
{
    public delegate void ItemPropertyChangedEventHandler<T>(object sender, ItemPropertyChangedEventArgs<T> e)
          where T : INotifyPropertyChanged;
    public class ItemPropertyChangedEventArgs<T> : EventArgs
         where T : INotifyPropertyChanged
    {
        public ItemPropertyChangedEventArgs(T item, string propertyName)
        {
            Item = item;
            PropertyName = propertyName;
        }

        public T Item { get; set; }
        public string PropertyName { get; set; }
    }
}
