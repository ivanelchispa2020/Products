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
    public class NewCategoryViewModel : INotifyPropertyChanged
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
        #endregion


        #region Constructor
        public NewCategoryViewModel()
        {
            ApiService=new ApiService();
            NavigationService=new NavigationService();
            DialogService=new DialogService();
            IsEnabled = true;
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
                await DialogService.ShowMessage("Error ","Debes seleccionar una descripcion en la categoria....");
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


            var category = new Category{ Description = Description, };  // CREO LA CATEGORIA

         
            var urlAPI = Application.Current.Resources["URLAPI"].ToString();

            MainViewModel mainViewModel = MainViewModel.getInstance();

            var response = await ApiService.Post(
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


            category =(Category) response.Result;

            var categoriesViewModel = CategoriesViewModel.GetInstance();
            categoriesViewModel.AddCategory(category);

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
