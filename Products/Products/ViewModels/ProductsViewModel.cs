using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Products.Models;
using Products.Services;
using Xamarin.Forms;

namespace Products.ViewModels
{
    public class ProductsViewModel : INotifyPropertyChanged
    {

        #region Prpiedades
      public  ObservableCollection<Product> Products
        {
            get { return _products; }
            set
            {
                if (_products != value)
                {
                    _products = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Products)));
                }
            }
        }

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                if (_isRefreshing != value)
                {
                    _isRefreshing = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRefreshing)));
                }
            }
        }
        #endregion



        #region Atributos
        List<Product> products;
        ObservableCollection<Product> _products;
        bool _isRefreshing;
        #endregion


        #region Constructor
        public ProductsViewModel(List<Product> products)
        {
            instance = this;
            DialogService = new DialogService();
            ApiService = new ApiService();
            this.products = products;
            Products = new ObservableCollection<Product>(products.OrderBy(c => c.Description));
            
        }

       



        #endregion



        #region Singleton
        static ProductsViewModel instance;

        public static ProductsViewModel getInstance()  /// ME DEVUELVE ESTA INSTANCIA
        {
            return instance;
        }
        #endregion



        #region Servicios
        DialogService DialogService;
        ApiService ApiService;

        #endregion

        #region Eventos
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Metodos
        public void Add(Product product)  // METODO QUE AGREGA EL NUEVO PRODUCTO A LA LIST 
        {
            IsRefreshing = true;
            products.Add(product);
            Products = new ObservableCollection<Product>(
            products.OrderBy(c => c.Description));
            IsRefreshing = false;
        }

        public void Update(Product product) // METODO QUE REFRESCA  LA LIST 
        {
            
                IsRefreshing = true;
                var oldProduct = products.Where(p => p.ProductId == product.ProductId).FirstOrDefault();
                oldProduct = product;
                Products = new ObservableCollection<Product>(products.OrderBy(c => c.Description));
                IsRefreshing = false;
      

        }
        
        public async Task Delete(Product product)  // METODO QUE ELIMINA EL PRODUCTO DE LA BASE DE DATOS Y REFRESCA LA LISTA
        {
            IsRefreshing = true;

            var connection = await ApiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await DialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var mainViewModel = MainViewModel.getInstance();


            var urlAPI = Application.Current.Resources["URLAPI"].ToString();

            var response = await ApiService.Delete(
               urlAPI,
                "/api",
                "/IProducts",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken,
                product);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await DialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            products.Remove(product);
            Products = new ObservableCollection<Product>(products.OrderBy(c => c.Description));

            IsRefreshing = false;
        }




        #endregion



    }
}
