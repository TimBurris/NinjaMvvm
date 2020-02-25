using System;
using System.Collections.Generic;
using System.Text;

namespace NinjaMvvm.Xam.Abstractions
{
    public interface IPage<TPageModel> : Xamvvm.IBasePage<TPageModel> where TPageModel : class, IPageModel
    {
    }
}
