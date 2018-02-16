using GalaSoft.MvvmLight.Command;
using Products.Models;
using Products.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Products.ViewModels
{
    public class EditCategoryViewModel :INotifyPropertyChanged
    {

        #region Propiedades
        public string Description
        {
            get { return _Description; }
            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
                }
            }
        }

        public bool IsRunning
        {
            get { return _IsRunning; }
            set
            {
                if (_IsRunning != value)
                {
                    _IsRunning = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRunning)));
                }
            }
        }

        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set
            {
                if (_IsEnabled != value)
                {
                    _IsEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEnabled)));
                }
            }
        }


      

        #endregion


        #region Servicios
        ApiService ApiService;
        NavigationService NavigationService;
        DialogService DialogService;
        #endregion



        #region Atributos
        string _Description;
        bool _IsRunning;
        bool _IsEnabled;
        private Category category;
        #endregion


        #region Constructor

       
        public EditCategoryViewModel(Category category)
        {
           
            this.category = category;
            ApiService = new ApiService();
            NavigationService = new NavigationService();
            DialogService = new DialogService();
            IsEnabled = true;

            Description = category.Description;

        }


        #endregion

    
        #region Comandos

        public ICommand SaveCommand { get { return new RelayCommand(Save); } }


        #endregion


        #region Metodos

        async void Save()
        {
          

            if (string.IsNullOrEmpty(Description))
            {
                await DialogService.ShowMessage("Error ", "Debes agregar una descripcion....");
                return;
            }

            IsEnabled = false;
            IsRunning = true;

            var connection = await ApiService.CheckConnection();  // VERIFICO SI HAY CONEXION
            if (!connection.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await DialogService.ShowMessage("Error", connection.Message);
                return;
            }



            category.Description = Description;

            var urlAPI = Application.Current.Resources["URLAPI"].ToString();

            var mainViewModel = MainViewModel.getInstance();

            var response = await ApiService.Put(    //ENVIA LA CATEGORIA
                    urlAPI,
                    "/api",
                    "/ICategories",
                    mainViewModel.Token.TokenType,
                    mainViewModel.Token.AccessToken,
                    category
               );


            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await DialogService.ShowMessage(
                    "Error",
                    response.Message);

                return;
            }
            
            var categoriesViewModel = CategoriesViewModel.GetInstance();
            categoriesViewModel.UpdateCategory(category);  // REFRESCO LA LISTA

            await NavigationService.Back();

            IsEnabled = true;
            IsRunning = false;



        }


        #endregion




        #region Eventos
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

    }
}
