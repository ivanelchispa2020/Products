using System.Threading.Tasks;
using Xamarin.Forms;

namespace Products.Services
{
    public class DialogService
    {
   
        public async Task ShowMessage(string title, string message)  /// PARA MOSTRAR MENSAJES EMERGENTES
        {
            await Application.Current.MainPage.DisplayAlert(
                title,
                message,
                "Accept");
        }

        public async Task<bool> ShowConfirm(string title, string message)  /// PARA MOSTRAR MENSAJES EMERGENTES
        {
            return await Application.Current.MainPage.DisplayAlert(
                title,
                message,
                "Aceptar",
                "Cancelar");

        }


    }
}
