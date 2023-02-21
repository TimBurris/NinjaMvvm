namespace NinjaMvvm.Wpf.Controls
{
    public class BindableContentPresenter : System.Windows.Controls.ContentPresenter
    {
        public BindableContentPresenter()
        {
            this.DataContextChanged += BindableContentPresenter_DataContextChanged;
            this.Unloaded += BindableContentPresenter_Unloaded;
        }

        private void BindableContentPresenter_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            (this.DataContext as NinjaMvvm.Wpf.WpfViewModelBase)?.ViewHasBeenUnbound();
            this.DataContextChanged -= BindableContentPresenter_DataContextChanged;
            this.Unloaded -= BindableContentPresenter_Unloaded;
        }

        private void BindableContentPresenter_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            (e.OldValue as NinjaMvvm.Wpf.WpfViewModelBase)?.ViewHasBeenUnbound();

            var vm = e.NewValue as NinjaMvvm.Wpf.WpfViewModelBase;

            if (vm == null)
                return;
            else
            {
                //tells the viewmodel it has been bound
                object value = vm.ViewBound;
            }
        }
    }
}
