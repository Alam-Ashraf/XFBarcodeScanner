using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private async void CameraView_OnDetected(object sender, GoogleVisionBarCodeScanner.OnDetectedEventArg e)
        {
            List<GoogleVisionBarCodeScanner.BarcodeResult> obj = e.BarcodeResults;

            string result = string.Empty;
            for (int i = 0; i < obj.Count; i++)
            {
                result += $"{i + 1}. BarcodeType : {obj[i].BarcodeType}, Barcode : {obj[i].DisplayValue}{Environment.NewLine}";
            }

            Device.BeginInvokeOnMainThread(async () =>
            {
                LblBarcodeValue.Text = result;

                //if you want to keep scanning the next barcode, do not close the scanning page and call below function
                GoogleVisionBarCodeScanner.Methods.SetIsScanning(true);
            });

        }
    }
}
