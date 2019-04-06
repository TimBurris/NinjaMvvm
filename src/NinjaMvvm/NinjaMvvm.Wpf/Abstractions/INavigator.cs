using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaMvvm.Wpf.Abstractions
{
    public interface INavigator
    {
        /// <summary>
        /// opens a new view
        /// </summary>
        /// <param name="viewModel"></param>
        TViewModel ShowDialog<TViewModel>() where TViewModel : ViewModelBase;

        /// <summary>
        /// opens a new view
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="modal"></param>
        /// <returns></returns>
        TViewModel ShowDialog<TViewModel>(bool modal) where TViewModel : ViewModelBase;

        /// <summary>
        /// opens a new view
        /// </summary>
        TViewModel ShowDialog<TViewModel>(Action<TViewModel> initAction) where TViewModel : ViewModelBase;

        /// <summary>
        /// opens a new view
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="initAction"></param>
        /// <param name="modal"></param>
        /// <returns></returns>
        TViewModel ShowDialog<TViewModel>(Action<TViewModel> initAction, bool modal) where TViewModel : ViewModelBase;

        /// <summary>
        /// closes a view
        /// </summary>
        /// <param name="viewModel"></param>
        void CloseDialog(ViewModelBase viewModel);

        /// <summary>
        /// navigates to a viewmodel
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <returns></returns>
        TViewModel NavigateTo<TViewModel>() where TViewModel : ViewModelBase;

        /// <summary>
        /// navigates to a viewmodel
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="initAction"></param>
        /// <returns></returns>
        TViewModel NavigateTo<TViewModel>(Action<TViewModel> initAction) where TViewModel : ViewModelBase;
    }
}
