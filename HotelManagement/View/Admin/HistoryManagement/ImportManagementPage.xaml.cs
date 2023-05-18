using HotelManagement.DTOs;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HotelManagement.View.Admin.HistoryManagement
{
    /// <summary>
    /// Interaction logic for ImportManagementPage.xaml
    /// </summary>
    public partial class ImportManagementPage : Page
    {
        public ImportManagementPage()
        {
            InitializeComponent();
        }

        private void cbb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (cbbmonth != null)
            {
                if(comboBox.SelectedIndex == 0)
                {
                    bodermonth.Visibility= Visibility.Collapsed;
                }
                if(comboBox.SelectedIndex == 1)
                {
                    comboBox.Visibility= Visibility.Visible;
                }
            }
        }

        private bool Filter(object item)   //can sua//
        {
            if (String.IsNullOrEmpty(FilterBox.Text)) return true;
            switch (cbbFilter.SelectedValue)
            {
                case "Mã khách hàng":
                    return ((item as StaffDTO).StaffId.IndexOf(FilterBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                case "Tên khách hàng":
                    return ((item as StaffDTO).StaffName.IndexOf(FilterBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                case "Số điện thoại":
                    return ((item as StaffDTO).PhoneNumber.IndexOf(FilterBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                default:
                    return ((item as StaffDTO).StaffId.IndexOf(FilterBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }
        private void filterbox_textchange(object sender, EventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(_ListView.ItemsSource);
            view.Filter = Filter;
            CollectionViewSource.GetDefaultView(_ListView.ItemsSource).Refresh();
        }
    }
}
