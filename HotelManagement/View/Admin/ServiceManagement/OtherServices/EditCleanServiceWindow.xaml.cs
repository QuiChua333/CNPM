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

namespace HotelManagement.View.Admin.ServiceManagement.OtherServices
{
    /// <summary>
    /// Interaction logic for EditCleanServiceWindow.xaml
    /// </summary>
    public partial class EditCleanServiceWindow : Window
    {
        public EditCleanServiceWindow()
        {
            InitializeComponent();
        }

        private void EditCleanServiceWD_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void PackIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
