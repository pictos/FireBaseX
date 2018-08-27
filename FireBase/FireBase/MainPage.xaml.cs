using Firebase.Storage;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FireBase
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            btn.Clicked += Servico;
        }

        private async void Servico(object sender, EventArgs e)
        {
            var foto = await PegarFoto();

            await EnviarImg(foto);
        }

        async Task<Stream> PegarFoto()
        {
            try
            {
                await CrossMedia.Current.Initialize();

                var foto = await CrossMedia.Current.PickPhotoAsync();
                if (foto == null)
                {
                    await DisplayAlert("Aviso", "Não foi possível pegar a foto", "ok");
                    return null;
                }

                return foto.GetStream();
            }
            catch (Exception)
            {

                throw;
            }
        }

        async Task EnviarImg(Stream foto)
        {
            try
            {
                var envio = new FirebaseStorage("fir-57c96.appspot.com")
                                    .Child("Teste")
                                    .Child("imagem1.jpg")
                                    .PutAsync(foto);

                envio.Progress.ProgressChanged += Progress_ProgressChanged;



                var url = await envio;
                txt.Text = url;

                await DisplayAlert("Aviso", "Tudo Ok", "Ok");
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void Progress_ProgressChanged(object sender, FirebaseStorageProgress e)
        {
            progress.Text = $"{e.Percentage}%";
        }
    }
}
