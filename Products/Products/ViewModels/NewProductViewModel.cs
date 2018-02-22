using GalaSoft.MvvmLight.Command;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Products.Helpers;
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
    public class NewProductViewModel : INotifyPropertyChanged
    {


        #region atributos
        string _Description;
        string _Price;
        bool _IsActive;
        DateTime _LastPurchase;
        string _Stock;
        string _Remarks;
        bool _IsRunning;
        bool _IsEnabled;
      

        // PARA LAS IMAGENES
        ImageSource _ImageSource;
        MediaFile file;

        #endregion


        #region Propiedades

        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Description)));
                }
            }
        }

        public string Price
        {
            get
            {
                return _Price;
            }
            set
            {
                if (_Price != value)
                {
                    _Price = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Price)));
                }
            }
        }


        public bool IsActive
        {
            get
            {
                return _IsActive;
            }
            set
            {
                if (_IsActive != value)
                {
                    _IsActive = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsActive)));
                }
            }
        }

        public DateTime LastPurchase
        {
            get
            {
                return _LastPurchase;
            }
            set
            {
                if (_LastPurchase != value)
                {
                    _LastPurchase = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(LastPurchase)));
                }
            }
        }

        public string Stock
        {
            get
            {
                return _Stock;
            }
            set
            {
                if (_Stock != value)
                {
                    _Stock = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Stock)));
                }
            }
        }

        public string Remarks
        {
            get
            {
                return _Remarks;
            }
            set
            {
                if (_Remarks != value)
                {
                    _Remarks = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Remarks)));
                }
            }
        }

      
        public bool IsRunning
        {
            get
            {
                return _IsRunning;
            }
            set
            {
                if (_IsRunning != value)
                {
                    _IsRunning = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsRunning)));
                }
            }
        }

        public bool IsEnabled
        {
            get
            {
                return _IsEnabled;
            }
            set
            {
                if (_IsEnabled != value)
                {
                    _IsEnabled = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsEnabled)));
                }
            }
        }



        public ImageSource ImageSource
        {
            set
            {
                if (_ImageSource != value)
                {
                    _ImageSource = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(ImageSource)));
                }
            }
            get
            {
                return _ImageSource;
            }
        }



        #endregion


        #region Constructor
        public NewProductViewModel()
        {
            DialogService = new DialogService();
            ApiService = new ApiService();
            NavigationService=new NavigationService();
            IsEnabled = true;
            LastPurchase = DateTime.Today;
            IsActive = true;
            ImageSource = "noImage";
        }

      
        #endregion


        #region Servicios
        DialogService DialogService;
        ApiService ApiService;
        NavigationService NavigationService;
        #endregion



        #region Eventos
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion



        #region Comandos

        public ICommand ChangeImageCommand { get { return new RelayCommand(ChangeImage); } }  // PARA LA IMAGEN
        public ICommand SaveCommand { get { return new RelayCommand(Save); } }

        #endregion



        #region Metodos
        async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            if (CrossMedia.Current.IsCameraAvailable &&
                CrossMedia.Current.IsTakePhotoSupported)
            {
                var source = await DialogService.ShowImageOptions();

                if (source == "Cancel")
                {
                    file = null;
                    return;
                }

                if (source == "From Camera")
                {
                    file = await CrossMedia.Current.TakePhotoAsync(
                        new StoreCameraMediaOptions
                        {
                            Directory = "Sample",
                            Name = "test.jpg",
                            PhotoSize = PhotoSize.Small,
                        }
                    );
                }
                else
                {
                    file = await CrossMedia.Current.PickPhotoAsync();
                }
            }
            else
            {
                file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (file != null)
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }

        }


        async void Save()
        {


            if (string.IsNullOrEmpty(Description))
            {
                await DialogService.ShowMessage("Error", "Debes Ingresar una Descripcion");
                return;
            }

            if (string.IsNullOrEmpty(Price))
            {
                await DialogService.ShowMessage("Error", "Debes Ingresar un Precio");
                return;
            }


            var price = decimal.Parse(Price);  // PARA SABER SI INGRESO NUMEROS
            if (price < 0)
            {
                await DialogService.ShowMessage(
                    "Error",
                    "Debes ingresar una cantidad mayor a 0.");
                return;
            }

            if (string.IsNullOrEmpty(Stock))
            {
                await DialogService.ShowMessage("Error", "Debes Ingresar un Stock de Mercaderia.");
                return;
            }

            var stock = double.Parse(Stock);  // PARA SABER SI INGRESO NUMEROS
            if (stock < 0)
            {
                await DialogService.ShowMessage(
                    "Error",
                    "Debe contener un stock mayor a 0.");
                return;
            }


            IsRunning = true;
            IsEnabled = false;

            // PARA LA IMAGEN
            byte[] imageArray = null;    
            if (file != null)
            {
                imageArray = FilesHelper.ReadFully(file.GetStream());
                file.Dispose();
            }

            var mainViewModel = MainViewModel.getInstance(); // OBTENGO UNA INSTANCIA DE LA MAINVIEMODEL
                                                                                                 //  PARA SACAR LA CATEGORIA EN MEMORIA

            var product = new Product // CREO EL PRODUCTO
            {
                CategoryId = mainViewModel.Category.CategoryId,
                Description = Description,
                ImageArray = imageArray,
                IsActive = IsActive,
                LastPurchase = LastPurchase,
                Price = price,
                Remarks = Remarks,
                Stock = stock,
            };
            


            var connection = await ApiService.CheckConnection();  // VERIFICO SI HAY CONEXION
            if (!connection.IsSuccess)
            {
                await DialogService.ShowMessage(
                     "Error conexion",
                     connection.Message);
            }
            else
            {
                var urlAPI = Application.Current.Resources["URLAPI"].ToString();

                var response = await ApiService.Post(  // ENVIO EL PRODUCTO
                    urlAPI,
                    "/api",
                    "/IProducts",
                    mainViewModel.Token.TokenType,
                    mainViewModel.Token.AccessToken,
                    product);

                if (!response.IsSuccess)        // PINTO ERROR SI NO PUEDO ENVIARLO
                {
                    IsRunning = false;
                    IsEnabled = true;
                    await DialogService.ShowMessage(
                        "Error respuesta",
                        response.Message);
                    return;
                }

                product = (Product)response.Result;  // CASTEO EL PRODUCTO
            }

            var productsViewModel = ProductsViewModel.getInstance(); // OBTENGO UNA INSTANCIA DE LOS PRODUCTOS
            productsViewModel.Add(product);   // AGREGO EL PRODUCTO A LA LISTA

            await NavigationService.BackOnMaster();  // LO VUEVO PA LA PAGINA ANTERIOR

            IsRunning = false;
            IsEnabled = true;




        }


        #endregion



    }
}
