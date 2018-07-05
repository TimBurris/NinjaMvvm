using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaMvvm.Wpf.Abstractions
{
    public interface IMainWindow
    {
        /// <summary>
        /// used to assign the viewmodel when someing is "NavigatedTo".  a typical implementation would assign the VM to the Content of a ContentPresenter
        /// </summary>
        /// <param name="viewModel"></param>
        void AssignToContent(ViewModelBase viewModel);
    }
}
