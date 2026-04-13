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

        protected readonly IDataAccess dataAccess;

        public BaseViewModel(IAlertService alertService, IDataAccess dataAccessService)
        {
            this.alertService = alertService;
            dataAccess = dataAccessService;
            pageTitle = string.Empty;
            isBusy = false;
        }

        [ObservableProperty]
        private string pageTitle;

        [ObservableProperty]
        private bool isBusy;

    }
}
