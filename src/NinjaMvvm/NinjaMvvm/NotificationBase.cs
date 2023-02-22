using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace NinjaMvvm
{
    /// <summary>
    /// the Base INotifyPropertyChangedimplementation, providing many various ways to notify property changed
    /// </summary>
    public class NotificationBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly object syncRoot = new object();

        private readonly Dictionary<string, object> fieldValuesDictionary = new Dictionary<string, object>();

        /// <summary>
        /// Sets the field and calls OnPropertyChanged when field value was changed. 
        /// Uses internal dictionary for field storing
        /// </summary>
        /// <returns><c>true</c>, if field was changed, <c>false</c> otherwise.</returns>
        /// <param name="value">Value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <typeparam name="T">The property type.</typeparam>
        protected bool SetField<T>(T value, [CallerMemberName] string propertyName = null)
        {
            T field = GetField<T>(propertyName);

            if (fieldValuesDictionary.ContainsKey(propertyName) && EqualityComparer<T>.Default.Equals(field, value))
                return false;

            lock (syncRoot)
            {
                fieldValuesDictionary[propertyName] = value;
            }

            OnPropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Sets the field and calls OnPropertyChanged when field value was changed. 
        /// Uses internal dictionary for field storing
        /// </summary>
        /// <returns><c>true</c>, if field was changed, <c>false</c> otherwise.</returns>
        /// <param name="value">Value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="additonalPropertiesToNotify">Additonal properties to notify when changed.</param>
        /// <typeparam name="T">The property type.</typeparam>
        protected bool SetField<T>(T value, [CallerMemberName] string propertyName = null, params Expression<Func<object>>[] additonalPropertiesToNotify)
        {
            T field = GetField<T>(propertyName);

            if (fieldValuesDictionary.ContainsKey(propertyName) && EqualityComparer<T>.Default.Equals(field, value))
                return false;

            lock (syncRoot)
            {
                fieldValuesDictionary[propertyName] = value;
            }

            OnPropertyChanged(propertyName);

            foreach (var item in additonalPropertiesToNotify)
                NotifyPropertyChanged(item);

            return true;
        }

        /// <summary>
        /// Sets the field and calls OnPropertyChanged when field value was changed. 
        /// Uses internal dictionary for field storing
        /// </summary>
        /// <returns><c>true</c>, if field was changed, <c>false</c> otherwise.</returns>
        /// <param name="value">Value.</param>
        /// <typeparam name="T">Property type.</typeparam>
        /// <param name="propertyNameSelector">Property name selector.</param>
        /// <param name="additonalPropertiesToNotify">Additonal properties to notify when changed.</param>
        protected bool SetField<T>(T value, Expression<Func<T>> propertyNameSelector, params Expression<Func<object>>[] additonalPropertiesToNotify)
        {
            var propertyName = GetPropertyNameFromExpression(propertyNameSelector);
            T field = GetField<T>(propertyName);

            if (fieldValuesDictionary.ContainsKey(propertyName) && EqualityComparer<T>.Default.Equals(field, value))
                return false;

            lock (fieldValuesDictionary)
            {
                fieldValuesDictionary[propertyName] = value;
            }

            OnPropertyChanged(propertyName);

            foreach (var item in additonalPropertiesToNotify)
                NotifyPropertyChanged(item);

            return true;
        }

        /// <summary>
        /// Gets the field.
        /// Uses internal dictionary to get field value
        /// </summary>
        /// <returns>The field.</returns>
        /// <param name="propertyName">Property name.</param>
        /// <typeparam name="T">The property type.</typeparam>
        protected T GetField<T>([CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException("propertyName cannot be null");

            object value = null;
            if (fieldValuesDictionary.TryGetValue(propertyName, out value))
            {
                return value == null ? default(T) : (T)value;
            }

            return default(T);
        }

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the field and calls OnPropertyChanged when field value was changed. 
        /// </summary>
        /// <returns><c>true</c>, if field was changed, <c>false</c> otherwise.</returns>
        /// <param name="field">Field.</param>
        /// <param name="value">Value.</param>
        /// <param name="propertyNameSelector">Property name selector expression.</param>
        /// <param name="additonalPropertiesToNotify">Additonal properties names to notify when changed.</param>
        /// <typeparam name="T">Property type.</typeparam>
        protected bool SetField<T>(ref T field, T value, Expression<Func<T>> propertyNameSelector, params Expression<Func<object>>[] additonalPropertiesToNotify)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            NotifyPropertyChanged(propertyNameSelector);

            foreach (var item in additonalPropertiesToNotify)
                NotifyPropertyChanged(item);

            return true;
        }

        /// <summary>
        /// Sets the field and calls OnPropertyChanged when field value was changed. 
        /// </summary>
        /// <returns><c>true</c>, if field was changed, <c>false</c> otherwise.</returns>
        /// <param name="field">Field.</param>
        /// <param name="value">Value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <typeparam name="T">Property type.</typeparam>
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException("propertyName cannot be null");

            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Sets the field and calls OnPropertyChanged when field value was changed. 
        /// Executes specified Action if field value was changed
        /// </summary>
        /// <returns><c>true</c>, if field was changed, <c>false</c> otherwise.</returns>
        /// <param name="field">Field.</param>
        /// <param name="value">Value.</param>
        /// <param name="propertyNameSelector">Property name selector expression.</param>
        /// <param name="propertyChanged">Action if property was changed.</param>
        /// <param name="additonalPropertiesToNotify">Additonal properties names to notify when changed.</param>
        /// <typeparam name="T">Property type.</typeparam>
        protected bool SetField<T>(ref T field, T value, Expression<Func<T>> propertyNameSelector,
            Action propertyChanged, params Expression<Func<object>>[] additonalPropertiesToNotify)
        {
            bool changed = SetField(ref field, value, propertyNameSelector, additonalPropertiesToNotify);

            if (changed)
            {
                propertyChanged();
            }

            return changed;
        }

        /// <summary>
        /// Notifies the property has changed.
        /// </summary>
        /// <param name="propertyNameSelector">Property name selector expression.</param>
        /// <typeparam name="T">Property type.</typeparam>
        protected void NotifyPropertyChanged<T>(Expression<Func<T>> propertyNameSelector)
        {
            OnPropertyChanged(GetPropertyNameFromExpression(propertyNameSelector));
        }

        /// <summary>
        /// Gets the property name from expression.
        /// </summary>
        /// <returns>The property name from expression.</returns>
        /// <param name="propertyNameSelector">Property name selector.</param>
        /// <typeparam name="T">Property type type parameter.</typeparam>
        protected string GetPropertyNameFromExpression<T>(Expression<Func<T>> propertyNameSelector)
        {
            if (propertyNameSelector == null)
                throw new ArgumentNullException("NotifyPropertyChanged Selector Expression");
            var me = propertyNameSelector.Body as MemberExpression;

            // Nullable properties can be nested inside of a convert function
            if (me == null)
            {
                var ue = propertyNameSelector.Body as UnaryExpression;
                if (ue != null)
                    me = ue.Operand as MemberExpression;
            }

            if (me == null)
                throw new ArgumentException("The body must be a member expression");

            return me.Member.Name;
        }

        /// <summary>
        /// Notifies that all properties have changed.
        /// </summary>
        public void NotifyAllPropertiesChanged()
        {
            var properties = GetType().GetRuntimeProperties();

            foreach (var prop in properties)
            {
                OnPropertyChanged(prop.Name);
            }
        }
    }
}
