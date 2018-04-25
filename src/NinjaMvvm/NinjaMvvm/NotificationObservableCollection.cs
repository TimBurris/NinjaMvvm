using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace NinjaMvvm
{
    /// <summary>
    /// an implementation of ObservableCollection that will raise propertychanged events for items contained in the collection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NotificationObservableCollection<T> : ObservableCollection<T>
     where T : INotifyPropertyChanged
    {
        #region Property change notification

        public event ItemPropertyChangedEventHandler<T> ItemPropertyChangedEvent;

        void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnItemPropertyChanged((T)sender, e.PropertyName);
        }

        protected virtual void OnItemPropertyChanged(T item, string propertyName)
        {
            ItemPropertyChangedEvent?.Invoke(this, new ItemPropertyChangedEventArgs<T>(item, propertyName));
        }

        #endregion

        #region ObservableCollection<T> overrides

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            item.PropertyChanged += item_PropertyChanged;
        }

        protected override void RemoveItem(int index)
        {
            var item = this[index];
            base.RemoveItem(index);
            item.PropertyChanged -= item_PropertyChanged;
        }

        protected override void ClearItems()
        {
            foreach (var item in this)
                item.PropertyChanged -= item_PropertyChanged;

            base.ClearItems();
        }

        protected override void SetItem(int index, T item)
        {
            var oldItem = this[index];
            oldItem.PropertyChanged -= item_PropertyChanged;
            base.SetItem(index, item);
            item.PropertyChanged += item_PropertyChanged;
        }

        #endregion
    }

}
