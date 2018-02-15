using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Products.Models;
using Products.Services;

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
        #endregion



        #region Atributos
        List<Product> products;
        ObservableCollection<Product> _products;
        #endregion


        #region Constructor
        public ProductsViewModel(List<Product> products)
        {
            DialogService = new DialogService();
            this.products = products;
            Products = new ObservableCollection<Product>(products.OrderBy(c => c.Description));
         
        }

        
        
        #endregion

        #region Servicios
        DialogService DialogService;
        #endregion

        #region Eventos
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

    }
}
