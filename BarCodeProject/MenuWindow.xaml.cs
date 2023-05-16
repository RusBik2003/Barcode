using System;
using System.Windows;

namespace BarCodeProject
{
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
        }

        private void CreateBarcodeWindowTransition_Click(object sender, RoutedEventArgs e)
        {
            CreateBarcodeWindow window = new CreateBarcodeWindow();
            window.Show();
            this.Close();
        }

        private void ReadBarcodeWindowTransition_Click(object sender, RoutedEventArgs e)
        {
            ReadBarCodeWindow window = new ReadBarCodeWindow();
            window.Show();
            this.Close();
        }

        private void WindowMenuClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
