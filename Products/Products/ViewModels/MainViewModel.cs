using System;
using System.Collections.Generic;
using System.Text;

namespace Products.ViewModels
{
    public class MainViewModel
    {
        #region propiedades
        public LoginViewModel Login { get; set; }
        #endregion

        #region constructor
        public MainViewModel()
        {
            Login = new LoginViewModel();  // PARA QUE EMPIEZE QUE POR EL LOGIN
        }
        #endregion

    }
}
