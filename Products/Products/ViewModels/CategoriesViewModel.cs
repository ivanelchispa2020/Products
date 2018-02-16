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


        public bool IsRefreshing
        {
            get
            {
                return _IsRefreshing;
            }
            set
            {
                if (_IsRefreshing != value)
                {
                    _IsRefreshing = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsRefreshing)));
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
        bool _IsRefreshing;
        #endregion




        #region Metodos
        async void loadCategories()
        {
            IsRefreshing = true;

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

            IsRefreshing = false;
        }


        public void AddCategory(Category category)
        {
            IsRefreshing = true;
            categories.Add(category);
            Categories = new ObservableCollection<Category>(categories.OrderBy(c => c.Description));  // ORDENA ALFABETICAMENTE
            IsRefreshing = false;
        }

        public void UpdateCategory(Category category)
        {
            IsRefreshing = true;
            var oldCategory = categories.Where(c => c.CategoryId==category.CategoryId).FirstOrDefault(); // BUSCO LA CATEGORIA
            oldCategory = category;
            Categories = new ObservableCollection<Category>(categories.OrderBy(c => c.Description));  // ORDENA ALFABETICAMENTE
            IsRefreshing = false;
        }

       public  async void DeleteCategory(Category category)
        {
            IsRefreshing = true;

            var connection = await ApiService.CheckConnection();// VERIFICO SI HAY CONEXION
            if(!connection.IsSuccess)
            {
                IsRefreshing = false;
                await DialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var urlAPI = Application.Current.Resources["URLAPI"].ToString();

            var mainViewModel = MainViewModel.getInstance();

            var response = await ApiService.Delete(    //ELIMINA  LA CATEGORIA EN LA API
                    urlAPI,
                    "/api",
                    "/ICategories",
                    mainViewModel.Token.TokenType,
                    mainViewModel.Token.AccessToken,
                    category
               );

            if (!response.IsSuccess)
            {
                await DialogService.ShowMessage(
              "Error",
              response.Message);
                return;
            }

            categories.Remove(category);  // ELIMINA DE LA LISTA LIST

            Categories = new ObservableCollection<Category>(categories.OrderBy(c => c.Description));  // ORDENA ALFABETICAMENTE
            IsRefreshing = false;
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


        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(loadCategories);  // METODO QUE REFRESCA LA PAGINA
            }
        }

       
        #endregion






    }
}
