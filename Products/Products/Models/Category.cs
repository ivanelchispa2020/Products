using GalaSoft.MvvmLight.Command;
using Products.Services;
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

       

        #region Constructor
        public Category()
        {
            NavigationService = new NavigationService();
            DialogService = new DialogService();
        } 
        #endregion


        #region Comandos

        public ICommand SelectCategoryCommand { get { return new RelayCommand(SelectCategoryAsync); }  }
        public ICommand EditCommand { get { return new RelayCommand(Edit); } }

        public ICommand DeleteCommand { get { return new RelayCommand(Delete); } }

    
        #endregion


        #region Metodos
        async void SelectCategoryAsync()
        {
            var mainViewModel = MainViewModel.getInstance();
            mainViewModel.Products = new ProductsViewModel(Products);
            await Application.Current.MainPage.Navigation.PushAsync(new ProductsView());
        }



        async void Edit()
        {
            MainViewModel.getInstance().EditCategory = new EditCategoryViewModel(this);  // MANDO LA CATEGORIA.
            await NavigationService.Navigate("EditCategoryView");
        }

        async void Delete()
        {
            var res = await DialogService.ShowConfirm("Eliminar Categoria",String.Format("¿Realmente desea eliminar la categoria {0} ?.",Description));
            if (!res) { return; };
            CategoriesViewModel.GetInstance().DeleteCategory(this);


        }


        public override int GetHashCode()   /// ES NECESARIO PARA EL METODO PUT
        {
            return CategoryId;
        }

        #endregion

        #region Servicios
        NavigationService NavigationService;
        DialogService DialogService;
        #endregion



    }
}
