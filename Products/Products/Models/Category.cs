using GalaSoft.MvvmLight.Command;
using Products.ViewModels;
using Products.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Products.Models
{
    public class Category
    {
        #region Propiedades

        public int CategoryId { get; set; }

        public string Description { get; set; }

        public List<Product> Products { get; set; }


        #endregion


        #region Comandos

        public ICommand SelectCategoryCommand { get { return new RelayCommand(SelectCategoryAsync); }  }

        #endregion


        #region Metodos
        async void SelectCategoryAsync()
        {
            var mainViewModel = MainViewModel.getInstance();
            mainViewModel.Products = new ProductsViewModel(Products);
            await Application.Current.MainPage.Navigation.PushAsync(new ProductsView());
        } 
        #endregion





    }
}
