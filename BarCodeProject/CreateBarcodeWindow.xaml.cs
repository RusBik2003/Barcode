using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.IO;
using ZXing;

namespace BarCodeProject
{
    public partial class CreateBarcodeWindow : Window
    {
        public CreateBarcodeWindow()
        {
            InitializeComponent();
        }

        private void Save(System.Drawing.Image im)
        {
            View.Source = null;
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.FileName = "Barcode"; // Имя по-умолчанию
            dlg.DefaultExt = ".jpg"; // Расширение по-умолчанию
            dlg.Filter = "JPEG (*.jpg)|*.jpg|PNG (*.png)|*.png|BMP (*.bmp)|*.bmp"; // Фильтр по расширениям
            Nullable<bool> result = dlg.ShowDialog();
            // Показываем диалог пользователю
            string filename = dlg.FileName;
            if (result == true)
            {
                im.Save(filename);

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.UriSource = new Uri(filename);
                bitmapImage.EndInit();

                View.Source = bitmapImage;
            }
        }

        private string GenControlNumber(string code)
        {
            long cod = Convert.ToInt64(code);
            long sum2 = 0;
            long sum1 = 0;
            int c = 12;
            long temp = 0;

            while (c > 0)
            {
                temp = cod % 10;
                if (c % 2 != 0)
                {
                    sum1 += temp;
                }
                else sum2 += temp;
                cod /= 10;
                c--;
            }
            long res = 0 ;
            if (TypeBarCode.Text == "EAN-13")
            {
                res = sum1 + sum2 * 3;
            }
            if (TypeBarCode.Text == "ITF-14")
            {
                res = sum2 + sum1 * 3;
            }
                long control = 10 - res % 10;

            if (control == 10)
            {
                control = 0;
            }
            string codeEan13 = code + control.ToString();
            return codeEan13;
        }

        private void CreateBarCode_Click(object sender, RoutedEventArgs e)
        {
            if (TypeBarCode.Text != "" && EnCodeText.Text != "")
            {
                Validation valid = new Validation(TypeBarCode.Text, EnCodeText.Text);

                if (valid.ValidationText())
                { 
                    BarcodeWriter barcodeWriter = new BarcodeWriter();

                    System.Drawing.Image barcodeImage=null;
                    string encodetxt = "";
                    switch (TypeBarCode.Text) {
                        case "EAN-13":
                            barcodeWriter.Format = BarcodeFormat.EAN_13;
                            encodetxt = GenControlNumber(EnCodeText.Text);
                            barcodeImage = barcodeWriter.Write(encodetxt);
                            break;
                        case "Code-128":
                            barcodeWriter.Format = BarcodeFormat.CODE_128;
                            barcodeImage = barcodeWriter.Write(EnCodeText.Text);
                            break;
                        case "ITF-14":
                            barcodeWriter.Format = BarcodeFormat.ITF;
                            encodetxt = GenControlNumber(EnCodeText.Text);
                            barcodeImage = barcodeWriter.Write(encodetxt);
                            break;
                        case "Codabar":
                            barcodeWriter.Format = BarcodeFormat.CODABAR;
                            barcodeImage = barcodeWriter.Write(EnCodeText.Text);
                            break;
                    }

                    
                    Save(barcodeImage);
                    barcodeImage.Dispose();
                }
            }
            else MessageBox.Show("все поля должны быть заполнены");


        }

        private void BtnFileDialog_Click(object sender, RoutedEventArgs e)
        {


        }

        private void EnCodeText_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            string pathInfo = System.AppContext.BaseDirectory;
            pathInfo += "InfoFormat\\";
            
            switch (TypeBarCode.Text)
            {
                case "EAN-13":
                    pathInfo += "Info_Ean-13.txt";
                    Message.Text = File.ReadAllText(pathInfo);
                    if (!(Char.IsDigit(e.Text, 0)) || EnCodeText.Text.Length >= 12) //запрет на ввод на не цифры и не больще 13 символов
                    {
                        e.Handled = true;
                    }
                    break;
                case "Code-128":
                    pathInfo += "Info_Code-128.txt";
                    Message.Text = File.ReadAllText(pathInfo);
                    Validation val = new Validation(TypeBarCode.Text, e.Text);
                    if (!val.ValidationText()) //запрет на ввод на не цифры и не больще 13 символов
                    {
                        e.Handled = true;
                    }
                    break;
                case "ITF-14":
                    pathInfo += "Info_ITF-14.txt";
                    Message.Text = File.ReadAllText(pathInfo);
                    
                    if (!(Char.IsDigit(e.Text, 0)) || EnCodeText.Text.Length >= 13) //запрет на ввод на не цифры и не больще 13 символов
                    {
                        e.Handled = true;
                    }
                    break;
                case "Codabar":
                    pathInfo += "Info_Codabar.txt";
                    Message.Text = File.ReadAllText(pathInfo);
                    if ((e.Text[0] < '0' || e.Text[0] > '9') && e.Text[0] != '+' && e.Text[0] != '-' 
                        && e.Text[0] != ':' && e.Text[0] != '/' && e.Text[0] != '.' && e.Text[0] != '$') //запрет на ввод на не цифры и не больще 13 символов
                    {
                        e.Handled = true;
                    }
                    break;

            }
                
        }

        private void TypeBarCode_SelectionChanged(object sender, SelectionChangedEventArgs e) //не удалять
        {
            View.Source = null;
            Message.Text = "";
            EnCodeText.Text = "";
        }

        private void MenuTransition_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow window = new MenuWindow();
            window.Show();
            this.Close();
        }
    }
}
