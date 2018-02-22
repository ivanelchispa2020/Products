using GalaSoft.MvvmLight.Command;
using Products.Services;
using Products.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Products.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public int CategoryId { get; set; }
    
        public Category Category { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public DateTime LastPurchase { get; set; }

        public double Stock { get; set; }

        public string Remarks { get; set; }

        public byte[] ImageArray { get; set; }  // ESTO ES PARA MANDAR LA IMAGENES POR EL API


        public string ImageFullPath // PARA QUE CONCATENE TODA LA IMAGEN DE LA RUTA
        {
            get
            {

                if (!string.IsNullOrEmpty(Image))
                {
                    return string.Format("http://productosivanapi.somee.com/{0}", Image.Substring(1));
                }
                else
                {
                    return "noImage";  // SI NO ENCUENTRA LA IMAGEN LA BUSCA EN LO RECURSOS SI NO PONER 
                  //  return "http://pruebaproductos.somee.com/Content/Images/no-image.png"; //LA BUSCA DESDE LA URL DEL BACKEND
                }

            }
        }


        #region Comandos
        public ICommand EditCommand { get { return new RelayCommand(Edit); } }
        public ICommand DeleteCommand { get { return new RelayCommand(Delete); } }

        #endregion




        #region Metodos

        async void Edit()
        {
            MainViewModel.getInstance().EditProduct = new EditProductViewModel(this);  // MANDO LA CATEGORIA.
            await NavigationService.Navigate("EditProductView");
        }

        async void Delete()
        {
            var res = await DialogService.ShowConfirm("Eliminar Producto", String.Format("¿Realmente desea eliminar la categoria {0} ?.", Description));
            if (!res) { return; };
            await ProductsViewModel.getInstance().Delete(this);  // ESTO ES LO QUE ME LO ELIMINA 


        }

        #endregion


        #region Servicios
        DialogService DialogService;
        NavigationService NavigationService;
        #endregion


        public Product()
        {
            DialogService = new DialogService();
            NavigationService = new NavigationService();
        }


        public override int GetHashCode()   // ES PARA EL API PUT// DELETE
        {
            return ProductId;
        }


    }
}
