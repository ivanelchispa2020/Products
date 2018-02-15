using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Products.Models;
using Products.Services;
using Products.ViewModels;

namespace Products.ViewModels
{
    public class MainViewModel
    {
        #region propiedades

        public LoginViewModel Login { get; set; }
        public CategoriesViewModel Categories { get; set; }
        public ProductsViewModel Products{ get; set; }
        public TokenResponse Token { get; set; }
        public NewCategoryViewModel NewCategory{ get; set; }


        public ICommand NewCategoryCommand
        {
            get
            {
                return new RelayCommand(newCategory);
            }
        }


        #endregion

        #region constructor
        public MainViewModel()
        {
            DialogService = new DialogService();
            NavigationService = new NavigationService();
            instance = this;
            Login = new LoginViewModel();  // PARA QUE EMPIEZE QUE POR EL LOGIN

        }
        #endregion


        #region Singleton
        static MainViewModel instance;

        public static MainViewModel getInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }
            return instance;
        }
        #endregion


        #region Servicios
        DialogService DialogService;
        NavigationService NavigationService;
        #endregion

        #region metodos

        async void newCategory()
        {
            NewCategory = new NewCategoryViewModel(); // DEBO INSTANCIAR SI O SI LA VIEWMODEL
            await   NavigationService.Navigate("NewCategoryView");
        }
        #endregion


    }
}
