using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HotelManagement.View.RoomLookupManagement
{
    /// <summary>
    /// Interaction logic for BillWindow.xaml
    /// </summary>
    public partial class BillWindow : Window
    {
        public BillWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();   
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                this.IsEnabled = false;
                PrintDialog printDialog = new PrintDialog();
                btnExit.Visibility=Visibility.Hidden;
                btnPrint.Visibility=Visibility.Hidden;
                lbHoaDon.Visibility = Visibility.Visible;
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(bill, "RentalContract");
                }
            }
            finally
            {
                this.IsEnabled = true;
                CustomMessageBox.ShowOk("In thành công!", "Thông báo", "Xác nhận", CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                this.Close();
            }

        }
    }
}
