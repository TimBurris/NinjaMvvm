using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NinjaMvvm.Xam.Samples
{
    public partial class MainPage : ContentPage, Xamvvm.IBasePage<PageModels.MainPageModel>
    {
        public MainPage()
        {
            InitializeComponent();

        }
    }
}
