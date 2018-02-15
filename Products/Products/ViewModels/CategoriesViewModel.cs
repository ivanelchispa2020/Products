using GalaSoft.MvvmLight.Command;
using Products.Models;
using Products.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Products.ViewModels 
{
    public class CategoriesViewModel : INotifyPropertyChanged
    {


        #region Propiedades

        public ObservableCollection<Category> Categories
        {
            get
            {
                return _categories;
            }
            set
            {
                if (_categories != value)
                {
                    _categories = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Categories)));
                }
            }
        }


        #endregion




        #region Constructor
        public   CategoriesViewModel()
        {
            instance = this;   // PARA EL SINGLETON
            ApiService = new ApiService();
            DialogService = new DialogService();
            loadCategories();
        }


        #endregion


        #region Atributos
        List<Category> categories;
        ObservableCollection<Category> _categories;
        #endregion




        #region Metodos
        async void loadCategories()
        {
            
            var connection = await ApiService.CheckConnection();
         
                if (!connection.IsSuccess)
                {
                    await DialogService.ShowMessage("Error", connection.Message);
                    return;
                }
           

                var urlAPI = Application.Current.Resources["URLAPI"].ToString();
                var mainViewModel = MainViewModel.getInstance();

                var response = await ApiService.GetList<Category>(
                    urlAPI,
                    "/api",
                    "/ICategories",
                    mainViewModel.Token.TokenType,
                    mainViewModel.Token.AccessToken
                    );

       
                if (!response.IsSuccess)
                {
                    await DialogService.ShowMessage("Error", response.Message);
                    return;
                    }

            categories =(List<Category>) response.Result;
            Categories = new ObservableCollection<Category>(categories.OrderBy(c => c.Description));  // ORDENA ALFABETICAMENTE

        }


        public void AddCategory(Category category)
        {
            categories.Add(category);
            Categories = new ObservableCollection<Category>(categories.OrderBy(c => c.Description));  // ORDENA ALFABETICAMENTE

        }

        #endregion

        #region services
        ApiService ApiService;
        DialogService DialogService;

        #endregion



        #region Eventos
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion



        #region Singleton
        
        static CategoriesViewModel instance;

        public static CategoriesViewModel GetInstance()
        {
            if (instance == null)
            {
                return new CategoriesViewModel();
            }

            return instance;
        }

        #endregion

        #region Comandos
        
        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(Search);
            }
        }

        async void Search()
        {
            await DialogService.ShowMessage("OK", "BUSCAR");
        }

      


        #endregion






    }
}
