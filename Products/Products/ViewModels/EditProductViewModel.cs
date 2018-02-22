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
   public  class EditProductViewModel :INotifyPropertyChanged
    {

        #region propiedades
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
                if (_imageSource != value)
                {
                    _imageSource = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(ImageSource)));
                }
            }
            get
            {
                return _imageSource;
            }
        }

        #endregion


        #region atributos
        string _Description;
        string _Price;
        bool _IsActive;
        DateTime _LastPurchase;
        string _Stock;
        string _Remarks;
        bool _IsRunning;
        bool _IsEnabled;
        Product product;
        ImageSource _imageSource;
        MediaFile file;

        #endregion


        #region Eventos
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion



        #region Constructor

        public EditProductViewModel(Product product)
        {
            this.product = product;

            Description = product.Description;
            ImageSource = product.ImageFullPath;
            Price = product.Price.ToString();
            IsActive = product.IsActive;
            LastPurchase = product.LastPurchase;
            Stock = product.Stock.ToString();
            Remarks = product.Remarks;

            DialogService = new DialogService();
            ApiService = new ApiService();
            NavigationService = new NavigationService();
            IsEnabled = true;
            IsActive = true;

        }
        #endregion


        #region Servicios
        DialogService DialogService;
        NavigationService NavigationService;
        ApiService ApiService;
        #endregion



        #region Comandos

        public ICommand ChangeImageCommand { get { return new RelayCommand(ChangeImage); } }

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

            // VALIDO LO QUE ME DIGITAN
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

            // PONGO LOS CONTROLES EN ARRANQUE
            IsRunning = true;
            IsEnabled = false;
            
            // REVISO SI HAY INTERNET SINO PA FUERA
            var connection = await ApiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await DialogService.ShowMessage("Error conexion", connection.Message);
                return;
            }

            // CREO UNA INSTANCIA DE LA MAINVIEMODEL
            var mainViewModel = MainViewModel.getInstance();


            // ESTO ES PARA LAS IMAGENES
            byte[] imageArray = null;
            if (file != null)
            {
                imageArray = FilesHelper.ReadFully(file.GetStream());
                file.Dispose();
            }

        
            // ESTO ES PARA LA VIEW -- PARA TENER LOS CAMPOS ANTERIORES
            product.Description = Description;
            product.IsActive = IsActive;
            product.LastPurchase = LastPurchase;
            product.Price = price;
            product.Remarks = Remarks;
            product.Stock = stock;
            product.ImageArray = imageArray;

       

            // RECURSO ESTATICO DE LA APP.XAML
            var urlAPI = Application.Current.Resources["URLAPI"].ToString();

            // CON ESTO LO ENVIO A LA BASE DE DATOS
            var response = await ApiService.Put(
               urlAPI,
               "/api",
               "/IProducts",
               mainViewModel.Token.TokenType,
               mainViewModel.Token.AccessToken,
               product);

            // SI OCURRIO UN ERROR AL MANDARLO A LA BBDD LO INFORMO Y PA FUERA
            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await DialogService.ShowMessage(
                    "Error respuesta",
                    response.Message);
                return;
            }

            // ESTO ES PARA REFRESCAR LA LISTA DE LA VIEW 
            ProductsViewModel.getInstance().Update(product);

            // PA LA PAGINA ANTERIOR
            await NavigationService.BackOnMaster();

            // DESACTIVO LOS CONTROLES
            IsRunning = false;
            IsEnabled = true;


        }








        #endregion



    }
}
