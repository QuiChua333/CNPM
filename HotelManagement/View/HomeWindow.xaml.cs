using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HotelManagement.View
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        public static Label lbPageName;
        public HomeWindow()
        {
            InitializeComponent();
            lbPageName = this.PageName;
        }
        private void Tg_Btn_Checked(object sender, RoutedEventArgs e)
        {
            Mask.Visibility = Visibility.Visible;
        }

        private void Tg_Btn_Unchecked(object sender, RoutedEventArgs e)
        {
            Mask.Visibility = Visibility.Collapsed;
        }

        private void Mask_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Mask.Visibility = Visibility.Collapsed;

            DoubleAnimation animation = new DoubleAnimation(62, TimeSpan.FromSeconds(0.2));
            GridNav.BeginAnimation(Border.WidthProperty, animation);
            Tg_Btn.IsChecked = false;
        }

        private void MenuItems_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Mask.Visibility = Visibility.Collapsed;

            DoubleAnimation animation = new DoubleAnimation(62, TimeSpan.FromSeconds(0.2));
            GridNav.BeginAnimation(Border.WidthProperty, animation);
            Tg_Btn.IsChecked = false;
        }
    }
}
