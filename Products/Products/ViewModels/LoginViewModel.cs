using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Products.Services;
using Products.Views;
using Products.Models;

namespace Products.ViewModels
{

    public class LoginViewModel : INotifyPropertyChanged
    {

        #region Propiedades
        public string Email
        {
            get { return _Email; }
            set
            {
                if (_Email != value)
                {
                    _Email = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Email)));
                }
            }
        }

        public string Password
        {
            get { return _Password; }
            set
            {
                if (_Password != value)
                {
                    _Password = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
                }
            }
        }

        public bool IsToggled
        {
            get { return _IsToggled; }
            set
            {
                if (_IsToggled != value)
                {
                    _IsToggled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsToggled)));
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


        #region Atributos
        string _Email;
        string _Password;
        bool _IsToggled;
        bool _IsRunning;
        bool _IsEnabled;
        #endregion


        #region Eventos
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Servicios
        DialogService dialogService;
        ApiService apiService;
        NavigationService navigationService;
        #endregion



        #region Constructor
        public LoginViewModel()
        {
            navigationService = new NavigationService();
            dialogService = new DialogService();
            apiService = new ApiService();
            IsEnabled = true;
            IsToggled = true;
          
        }
        #endregion

        #region Comandos
        public ICommand RecoverPasswordCommand
        {
            get
            {
                return new RelayCommand(recoverPasswordCommand);
            }
        }

        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(loginCommand);
            }
        }

        public ICommand RegisterNewUserCommand
        {
            get
            {
                return new RelayCommand(registerNewUserCommand);
            }
        }
        public ICommand LoginWithFacebookCommand
        {
            get
            {
                return new RelayCommand(loginWithFacebookCommand);
            }
        }

  


        #endregion

        #region Metodos
        private void recoverPasswordCommand()
        {
            Application.Current.MainPage.DisplayAlert("Ok","Has pulsado el recordar contraseña","Aceptar");
        }


        async void loginCommand()
        {

            if (string.IsNullOrEmpty(Email))
            {
                await dialogService.ShowMessage("Error", "Debes ingresar un Email ");
            }

            if (string.IsNullOrEmpty(Password))
            {
                await dialogService.ShowMessage("Error", "Debes ingresar una Contraseña ");
            }

            IsRunning = true;
            IsEnabled = false;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }
        

            var urlAPI = Application.Current.Resources["URLAPI"].ToString();

            var response = await apiService.GetToken(
                urlAPI,
                Email,
                Password);
            

            if (response == null)
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage(
                    "Error",
                    "El servicio no esta disponible, por favor intentelo mas tarde...");
                Password = null;
                return;
            }

            if (string.IsNullOrEmpty(response.AccessToken))
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage(
                    "Error",
                    response.ErrorDescription);
                Password = null;
                return;
            }

          
            var mainViewModel = MainViewModel.getInstance();   // SINGLETON
            mainViewModel.Token = response;   // PARA OBTENER EL TOKEN Y TENERLO ALMACENADO
            mainViewModel.Categories = new CategoriesViewModel();  // LA LIGAMOS CON LA VIEWMODEL

          await  navigationService.Navigate("CategoriesView");// LO ENVIA A OTRA PAGINA

            Email = null;
            Password = null;

            IsRunning = false;
            IsEnabled = true;



        }

        private void registerNewUserCommand()
        {
            Application.Current.MainPage.DisplayAlert("Ok", "Has pulsado registrar nuevo usuario", "Aceptar");
        }

        private void loginWithFacebookCommand()
        {
            Application.Current.MainPage.DisplayAlert("Ok", "Has pulsado loguearse con facebook", "Aceptar");
        }


        #endregion

    }
}
