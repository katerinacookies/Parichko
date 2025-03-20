using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
//using Microsoft.UI.Xaml.Controls.Primitives;
using Parichko.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parichko.ViewModels
{
    internal class PopupViewModel : ObservableObject
    {
        public void ShowPopupPlus()
        {
            var popupPlus = new PopupPlus();
            Shell.Current.ShowPopup(popupPlus);
        }
    }
}
