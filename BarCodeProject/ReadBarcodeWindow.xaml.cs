using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZXing; //библиотека для работы с штрих кодами

namespace BarCodeProject
{
    public partial class ReadBarCodeWindow : Window
    {
        public ReadBarCodeWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            View.Source = null;
            Result.Text = "";
            TypeBarcode.Text = "";
            PathFile.Text = "";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".jpg"; // Расширение по-умолчанию
            ofd.Filter = "JPEG (*.jpg)|*.jpg|PNG (*.png)|*.png|BMP (*.bmp)|*.bmp"; // Фильтр по расширениям
            var result = ofd.ShowDialog();
            if (result == true)
            {
                // Open document
                string filename = ofd.FileName;
                PathFile.Text = filename;

                //Картинку добавляем
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache; //Нужно чтобы прошлая картинка очистилась из кеша
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad; //загрузка картинки в кеш
                bitmapImage.UriSource = new Uri(filename);
                bitmapImage.EndInit();

                View.Stretch = Stretch.Uniform;
                View.Source = bitmapImage;
            }
        }

        private void Decode_Click(object sender, RoutedEventArgs e)
        {
            string path = PathFile.Text;
            FileInfo fileInfo = new FileInfo(path);

            if (fileInfo.Exists) //существует ли файл?
            {
                string ext = fileInfo.Extension; // получаем расширение файла
                if (ext == ".jpg" || ext == ".png" || ext == ".bmp" )
                {
                    string data = "";
                    BarcodeReader reader = new BarcodeReader() { AutoRotate = true, TryInverted = true };
                    Bitmap barcodeBitmap = (Bitmap)System.Drawing.Image.FromFile(path);
                    Result result = reader.Decode(barcodeBitmap);
                    if (result != null)
                    {
                        data = (result.ToString());
                        Result.Text = data;
                        TypeBarcode.Text = result.BarcodeFormat.ToString();
                    }
                    else MessageBox.Show("Штрих код на картинке не найден, попробуйте загрузить другую");
                }
                else MessageBox.Show("Формат файла не поддерживается");
            }
            else MessageBox.Show("Заданный файл не найден! Пожалуйста, проверьте правильность записи пути к файлу");
        }

        private void MenuTransition_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow window = new MenuWindow();
            window.Show();
            this.Close();
        }
    }
}
