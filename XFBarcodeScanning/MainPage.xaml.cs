using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace XFBarcodeScanning
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            GoogleVisionBarCodeScanner.Methods.AskForRequiredPermission();
        }

        void OnScanBarCode(System.Object sender, System.EventArgs e)
        {
            // Start scanning
            GoogleVisionBarCodeScanner.Methods.SetIsScanning(true);
        }

        void OnStopScanCode(System.Object sender, System.EventArgs e)
        {
            // Stop scanning
            GoogleVisionBarCodeScanner.Methods.SetIsScanning(false);
        }

        async void OnScanBarcodeFromImage(System.Object sender, System.EventArgs e)
        {
            var result = await MediaPicker.PickPhotoAsync();

            if (result != null)
            {
                var stream = await result.OpenReadAsync();

                var bytes = new byte[stream.Length];
                await stream.ReadAsync(bytes, 0, bytes.Length);
                stream.Seek(0, SeekOrigin.Begin);

                List<GoogleVisionBarCodeScanner.BarcodeResult> obj = await GoogleVisionBarCodeScanner.Methods.ScanFromImage(bytes);

                string ss = string.Empty;

                for (int i = 0; i < obj.Count; i++)
                {
                    ss += $"{i + 1}. BarcodeType : {obj[i].BarcodeType}, Barcode : {obj[i].DisplayValue}{Environment.NewLine}";
                }

                Device.BeginInvokeOnMainThread(async () =>
                {
                    LblBarcodeValue.Text = ss;

                    await DisplayAlert("Selected Barcode Detail", ss, "ok");
                });
            }
        }


        private void CameraView_OnDetected(object sender, GoogleVisionBarCodeScanner.OnDetectedEventArg e)
        {
            List<GoogleVisionBarCodeScanner.BarcodeResult> obj = e.BarcodeResults;

            string result = string.Empty;
            for (int i = 0; i < obj.Count; i++)
            {
                result += $"{i + 1}. BarcodeType : {obj[i].BarcodeType}, Barcode : {obj[i].DisplayValue}{Environment.NewLine}";
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                LblBarcodeValue.Text = result;

                //if you want to keep scanning the next barcode, do not close the scanning page and call below function
                GoogleVisionBarCodeScanner.Methods.SetIsScanning(true);
            });

        }

    }

    public static class StreamExtensions
    {
        public static byte[] ReadAllBytes(this Stream instream)
        {
            if (instream is MemoryStream)
                return ((MemoryStream)instream).ToArray();

            using (var memoryStream = new MemoryStream())
            {
                instream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
