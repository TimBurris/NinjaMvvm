using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace NinjaMvvm.Xam.Behaviors
{

    /// <summary>
    /// ListView Item Tapped Behavior.  
    /// Fires the <see cref="Command"/> when an item of the ListView is Tapped.  the CommandParameter is the Binding of the ListViewItem that was Tapped
    /// </summary>
    public class ItemTappedBehavior : BehaviorBase<ListView>
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(ItemTappedBehavior), null);
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }


        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);
            AssociatedObject.ItemTapped += OnItemTapped;
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            base.OnDetachingFrom(bindable);
            AssociatedObject.ItemTapped -= OnItemTapped;
        }

        void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (Command == null || e.Item == null) return;

            if (Command.CanExecute(e.Item))
                Command.Execute(e.Item);
        }
    }
}
