using CommunityToolkit.Mvvm.ComponentModel;
using ProjetPOO.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.ViewModel
{
    public partial class BaseViewModel : ObservableObject
    {
        protected readonly IAlertService alertService;

        public BaseViewModel(IAlertService alertService)
        {
            this.alertService = alertService;
            pageTitle = string.Empty;
            isBusy = false;
        }

        [ObservableProperty]
        private string pageTitle;

        [ObservableProperty]
        private bool isBusy;

    }
}
