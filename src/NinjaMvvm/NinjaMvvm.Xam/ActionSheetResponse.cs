using System;
using System.Collections.Generic;
using System.Text;

namespace NinjaMvvm.Xam
{
    /// <summary>
    /// Used as a response to a NavigationPage's DisplayActionSheet;  provides the seleteditem, but also a more helpful boolean to help user test whether it was cancelled
    /// </summary>
    public class ActionSheetResponse
    {
        public string SelectedItem { get; set; }
        public bool WasCancelled { get; set; }
    }
}
