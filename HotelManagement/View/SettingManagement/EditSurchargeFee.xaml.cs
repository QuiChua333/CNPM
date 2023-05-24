using HotelManagement.Utilities;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HotelManagement.View.SettingManagement
{
    /// <summary>
    /// Interaction logic for EditSurchargeFee.xaml
    /// </summary>
    public partial class EditSurchargeFee : Window
    {
        public EditSurchargeFee()
        {
            InitializeComponent();
        }
        public EditSurchargeFee(PackIcon p)
        {
            EditIcon = p;
            InitializeComponent();
        }
        PackIcon EditIcon;
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }    
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            EditIcon.Kind = PackIconKind.Pencil;
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            EditIcon.Kind = PackIconKind.Pencil;
        }

        private void InputPercentage_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }
    }
}
